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
    private Vector3 wayPoint;               // ����ƶ���
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
            // һ��ʼ��ѡ����Guard����������״̬
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

    // ��Destoryִ����֮��Ž��е���
    private void OnDisable()
    {
            GameManager.Instance.RemoveObverser(this);
    }

    // ͨ��ö�ٵ�������ʵ��Enemy�Ķ����л�
    void SwitchStates()
    {
        if (IsDead)
            enemyStates = EnemyStates.DEAD;

        // �����radius��Χ�ڷ���Player�Ļ���enemy״̬����ΪCHASE
        else if(FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
            //Debug.Log("�ҵ�����");
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

                // Ѳ��
                IsChase = false;
                agent.speed = Speed * 0.5f;

                // ����WayPoint,������һ������������
                if(Vector3.Distance(transform.position,wayPoint) <= agent.stoppingDistance)
                {
                    IsWalk = false;

                    if(remainLookAtTime > 0)
                        remainLookAtTime -= Time.deltaTime;
                    else
                        // ��������Ŀ���
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
                    //���ѻص���һ��״̬
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
                    // ׷��
                    IsFollow = true;
                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position;
                    ShowBarUI();
                }

                // �ڹ�����Χ���򹥻�
                if(TargetInAttackRange() || TargetInSkillRange())
                {
                    IsFollow = false;
                    agent.isStopped = true;

                    if(LastAttackTime < 0)
                    {
                        // ��ȴʱ�����
                        LastAttackTime = characterStats.attackData.coolDown;

                        // �����ж�
                        characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
                        // ִ�й���
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

    // ���˹�������
    void Attack()
    {
        transform.LookAt(attackTarget.transform.position);
        if (TargetInAttackRange())
        {
            // ����������
            anim.SetTrigger("Attack");
        }
        if(TargetInSkillRange())
        {
            // ���ܹ�������
            anim.SetTrigger("SKILL");
        }
    }

    // Enemy״̬�ı仯
    void SwitchAnimations()
    {
        anim.SetBool("Walk", IsWalk);
        anim.SetBool("Chase", IsChase);
        anim.SetBool("Follow", IsFollow);
        anim.SetBool("Critical",characterStats.isCritical);
        anim.SetBool("Dead", IsDead);
    }
    
    // �ж�Player�Ƿ���Attackrange��Χ��
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

    // �ж�Plater�Ƿ���SkillRange��Χ��
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

    // ����Player�ķ���
    bool FoundPlayer()
    {
        // ��var������OverLapSphere�µ�Collider
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

        // ���Playerû�г����ڵ���Chase�ķ�Χ�ڵĻ�������Ϊnull
        attackTarget = null;
        IsFound = false;
        return false;
    }
    // ��ȡ�����
    void GetNewWayPoint()
    {

        remainLookAtTime = LookAtTime;
        float randomX =UnityEngine.Random.Range(-patrolRadius, patrolRadius);
        float randomZ = UnityEngine.Random.Range(-patrolRadius, patrolRadius);

        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRadius, 1) ? hit.position : transform.position;
    }

    // ��Scene�����չʾsightRaidus�ķ�Χ�����ڵ������С
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }

    // ���˹�������ʱ��������Ч��
    void hit()
    {
        // ��Player��ǰ����ĳ����Χ�ڵĻ����Ż�����˺�
        if(attackTarget != null && transform.IsfacingTarget(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            characterStats.TakeDamage(characterStats, targetStats);


            attackTarget.GetComponent<Animator>().SetTrigger("Hit");
        }
    }

    public void EndNotify()
    {
        // ���˻�ʤ�Ķ���
        // ֹͣ���е��ƶ�
        // ֹͣAgent
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
