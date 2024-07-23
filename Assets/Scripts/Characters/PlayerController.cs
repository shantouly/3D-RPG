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
        // �ű��Ļ�ȡҲ���������һ�����л�ȡ��Ҳ�൱����һ������ˣ�
        characterStats = GetComponent<CharacterStats>();
    }

    private void OnEnable()
    {
        // �൱��һ��ί���ˣ���Actionע�ᵽ�����У�ʹ��OnMouseClick���õ�ʱ��ͻ�ȫ��ע���Action�ķ���
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

    // Player�ƶ��Ķ���ת��
    void SwitchPlayerAnimation()
    {
        // ��Player���˶��ٶȱ�ɸ��������ݸ�Speed
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
        //Debug.Log(agent.velocity.sqrMagnitude);

        anim.SetBool("Dead", isDead);
    }

    // �ƶ���ָ���ط�
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

    // �����¼�
    void EventAttack(GameObject target)
    {
        if(target != null)
        {
            AttackTarget = target;
            StartCoroutine(MoveToAttackTarget());
        }
    }

    // ��Э����ʵ��Player����Enemy��ÿһ֡��ִ��
    IEnumerator MoveToAttackTarget()
    {

        agent.isStopped = false;                // ���ÿ����ƶ�
        // ��Ҫ�����������
        transform.LookAt(AttackTarget.transform);
        // ѭ�������������ڹ������Χ�����ƶ���Enemy��ǰ
        while(Vector3.Distance(AttackTarget.transform.position,transform.position) > characterStats.attackData.attackRange)
        {
            agent.destination = AttackTarget.transform.position;
            yield return null;// ����ֱ�ӽ�����һ�ε�ѭ����û��ʱ����
        }

        // �ƶ���֮��Ҫ�����ٶȱ��㴦��
        agent.isStopped = true;

        // Attack
        if(LastAttackTime < 0)  
        {
            // ������ȴʱ�����
            anim.SetTrigger("Attack");
            LastAttackTime = characterStats.attackData.coolDown;
        }
    }

    //Player��������ʱ��������Ч��
    void hit()
    {
        if (AttackTarget.CompareTag("Attackable"))
        {
            // ���Ŀ�����������Rock�ű��Ļ�
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
