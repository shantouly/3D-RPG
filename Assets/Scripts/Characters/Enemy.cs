using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
/// 
public enum EnemyStates { GUARD,PATROL,CHASE,DEAD}
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class Enemy : MonoBehaviour, IEndGameObverser
{

    public bool IsFound;
    private EnemyStates enemyStates;
    private NavMeshAgent agent;
    private Collider coll;

    [Header("Basic Settings")]
    public float sightRadius;
    protected GameObject attackTarget;
    public bool IsGuard;
    public float Speed;

    [Header("Patrol States")]
    public float LookAtTime;
    private float remainLookAtTime;
    public float patrolRadius;
    private Vector3 wayPoint;               // 随机移动点
    private Vector3 guardPos;
    private quaternion guardRotation;

    private bool IsWalk;
    private bool IsChase;
    private bool IsFollow;
    private bool IsDead;
    private Animator anim;
    private float LastAttackTime = 0;
    protected CharacterStats characterStats;
    private void Awake()
    {
        IsWalk = false;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        coll = GetComponent<Collider>();
        Speed = agent.speed;
        guardPos = transform.position;
        guardRotation = transform.rotation;
        remainLookAtTime = LookAtTime;
    }

    private void Start()
    {
        if (IsGuard)
        {
            // 一开始勾选上了Guard，处于守卫状态
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            GetNewWayPoint();
        }

        GameManager.Instance.AddObverser(this);
    }

    private void Update()
    {
        if (characterStats.CurrentHealth == 0)
            IsDead = true;
        SwitchStates();
        SwitchAnimations();
        LastAttackTime -= Time.deltaTime;
    }

    //private void OnEnable()
    //{
    //    GameManager.Instance.AddObverser(this);
    //}

    // 在Destory执行完之后才进行调用
    private void OnDisable()
    {
            GameManager.Instance.RemoveObverser(this);
    }

    // 通过枚举的类型来实现Enemy的动画切换
    void SwitchStates()
    {
        if (IsDead)
            enemyStates = EnemyStates.DEAD;

        // 如果在radius范围内发现Player的话，enemy状态设置为CHASE
        else if(FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
            //Debug.Log("找到物体");
        }

        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                IsChase = false;

                if (transform.position != guardPos)
                {
                    IsWalk = true;
                    agent.isStopped = false;
                    agent.destination = guardPos;

                    if (Vector3.SqrMagnitude(guardPos - transform.position) <= agent.stoppingDistance)
                    {
                        IsWalk = false;
                    }
                }
                    break;
            case EnemyStates.PATROL:

                // 巡逻
                IsChase = false;
                agent.speed = Speed * 0.5f;

                // 靠近WayPoint,进行下一次随机点的生成
                if(Vector3.Distance(transform.position,wayPoint) <= agent.stoppingDistance)
                {
                    IsWalk = false;

                    if(remainLookAtTime > 0)
                        remainLookAtTime -= Time.deltaTime;
                    else
                        // 重新生成目标点
                        GetNewWayPoint();
                }
                else
                {
                    IsWalk = true;
                    agent.destination = wayPoint;
                }
                    break;
            case EnemyStates.CHASE:
                IsWalk = false;
                IsChase = true;
                agent.speed = Speed;
                if (!FoundPlayer())
                {
                    //拉脱回到上一个状态
                    IsFollow = false;
                    if (remainLookAtTime > 0)
                    {
                        agent.destination = transform.position;
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else if (IsGuard)
                        enemyStates = EnemyStates.GUARD;
                    else
                        enemyStates = EnemyStates.PATROL;

                    //Debug.Log(enemyStates);
                    }
                else
                {
                    // 追击
                    IsFollow = true;
                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position;
                    ShowBarUI();
                }

                // 在攻击范围内则攻击
                if(TargetInAttackRange() || TargetInSkillRange())
                {
                    IsFollow = false;
                    agent.isStopped = true;

                    if(LastAttackTime < 0)
                    {
                        // 冷却时间结束
                        LastAttackTime = characterStats.attackData.coolDown;

                        // 暴击判断
                        characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
                        // 执行攻击
                        Attack();
                    }
                }
                break;
            case EnemyStates.DEAD:
                coll.enabled = false;
                agent.radius = 0;
                Destroy(gameObject,2f);
                break;

        }
    }

    // 敌人攻击方法
    void Attack()
    {
        transform.LookAt(attackTarget.transform.position);
        if (TargetInAttackRange())
        {
            // 近身攻击动画
            anim.SetTrigger("Attack");
        }
        if(TargetInSkillRange())
        {
            // 技能攻击动画
            anim.SetTrigger("SKILL");
        }
    }

    // Enemy状态的变化
    void SwitchAnimations()
    {
        anim.SetBool("Walk", IsWalk);
        anim.SetBool("Chase", IsChase);
        anim.SetBool("Follow", IsFollow);
        anim.SetBool("Critical",characterStats.isCritical);
        anim.SetBool("Dead", IsDead);
    }
    
    // 判断Player是否在Attackrange范围内
    bool TargetInAttackRange()
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.attackRange;
        }
        else
        {
            return false;
        }
    }

    // 判断Plater是否在SkillRange范围内
    bool TargetInSkillRange()
    {
        if(attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
            //return TargetInAttackRange()?true:false;
        }
        else
        {
            return false;
        }
    }

    // 发现Player的方法
    bool FoundPlayer()
    {
        // 用var来接收OverLapSphere下的Collider
        var colliders =  Physics.OverlapSphere(transform.position, sightRadius);

        foreach (var target in colliders)
        {
            if (target.gameObject.transform.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                IsFound = true;
                return true;
            }
        }

        // 如果Player没有出现在敌人Chase的范围内的话，设置为null
        attackTarget = null;
        IsFound = false;
        return false;
    }
    // 获取随机点
    void GetNewWayPoint()
    {

        remainLookAtTime = LookAtTime;
        float randomX =UnityEngine.Random.Range(-patrolRadius, patrolRadius);
        float randomZ = UnityEngine.Random.Range(-patrolRadius, patrolRadius);

        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRadius, 1) ? hit.position : transform.position;
    }

    // 在Scene面板中展示sightRaidus的范围，便于调节其大小
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }

    // 敌人攻击动画时产生攻击效果
    void hit()
    {
        // 当Player在前方的某个范围内的话，才会造成伤害
        if(attackTarget != null && transform.IsfacingTarget(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            characterStats.TakeDamage(characterStats, targetStats);


            attackTarget.GetComponent<Animator>().SetTrigger("Hit");
        }
    }

    public void EndNotify()
    {
        // 敌人获胜的动画
        // 停止所有的移动
        // 停止Agent
        IsChase = false;
        IsWalk = false;
        attackTarget = null;
        anim.SetBool("Win", true);
    }

    private void ShowBarUI()    
    {
        if (IsFound)
        { 
            characterStats.ShowBarUI(characterStats);
        }
            
    }
}
