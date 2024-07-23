using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
public class PlayerController : MonoBehaviour
{
    private Animator anim;
    //private CharacterController Player;
    private NavMeshAgent agent;
    private GameObject AttackTarget;
    private float LastAttackTime = 0.5f;
    private CharacterStats characterStats;
    private bool isDead = false;


    private void Awake()
    {
        //Player = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();     
        // 脚本的获取也可以向组件一样进行获取（也相当于是一个组件了）
        characterStats = GetComponent<CharacterStats>();
    }

    private void OnEnable()
    {
        // 相当与一个委托了，将Action注册到方法中，使得OnMouseClick调用的时候就会全部注册过Action的方法
        MouseManager.Instance.OnMouseClick += MoveToTraget;
        MouseManager.Instance.OnEnemyClick += EventAttack;
        GameManager.Instance.RigisterPlayer(characterStats);
    }

    private void Start()
    {
        SaveManger.Instance.LoadPlayerData();
    }

    private void OnDisable()
    {
        MouseManager.Instance.OnMouseClick -= MoveToTraget;
        MouseManager.Instance.OnEnemyClick -= EventAttack;
    }

    private void Update()
    {
        isDead = characterStats.CurrentHealth == 0;
        if (isDead)
        {
            GameManager.Instance.NotifyObservers();
            MouseManager.Instance.OnMouseClick -= MoveToTraget;
        }
        SwitchPlayerAnimation();
        LastAttackTime -= Time.deltaTime;
        //Debug.Log(LastAttackTime);
    }

    // Player移动的动画转换
    void SwitchPlayerAnimation()
    {
        // 将Player的运动速度变成浮点数传递给Speed
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
        //Debug.Log(agent.velocity.sqrMagnitude);

        anim.SetBool("Dead", isDead);
    }

    // 移动到指定地方
    void MoveToTraget(Vector3 target)
    {
        //if (!isDead)
        //{
            agent.isStopped = false;
            characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
            StopAllCoroutines();
            agent.destination = target;
            //Player.SimpleMove(target);
        //}
    }

    // 攻击事件
    void EventAttack(GameObject target)
    {
        if(target != null)
        {
            AttackTarget = target;
            StartCoroutine(MoveToAttackTarget());
        }
    }

    // 用协程来实现Player走向Enemy，每一帧均执行
    IEnumerator MoveToAttackTarget()
    {

        agent.isStopped = false;                // 设置可以移动
        // 向要将其面向敌人
        transform.LookAt(AttackTarget.transform);
        // 循坏，如果距离大于攻击最大范围，则移动至Enemy面前
        while(Vector3.Distance(AttackTarget.transform.position,transform.position) > characterStats.attackData.attackRange)
        {
            agent.destination = AttackTarget.transform.position;
            yield return null;// 可以直接进行下一次的循坏，没有时间间隔
        }

        // 移动到之后要进行速度变零处理
        agent.isStopped = true;

        // Attack
        if(LastAttackTime < 0)  
        {
            // 攻击冷却时间结束
            anim.SetTrigger("Attack");
            LastAttackTime = characterStats.attackData.coolDown;
        }
    }

    //Player攻击动画时产生攻击效果
    void hit()
    {
        if (AttackTarget.CompareTag("Attackable"))
        {
            // 如果目标上面挂载了Rock脚本的话
            if (AttackTarget.GetComponent<Rock>())
            {
                AttackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                AttackTarget.GetComponent<Rock>().rockStates = RockStates.HitGloem;
                AttackTarget.GetComponent<Rigidbody>().AddForce(transform.forward *20, ForceMode.Impulse);

            }
        }
        else
        {
            var targetStats = AttackTarget.GetComponent<CharacterStats>();
            characterStats.TakeDamage(characterStats, targetStats);

            AttackTarget.GetComponent<Animator>().SetTrigger("Hit");
        }
    }
}
