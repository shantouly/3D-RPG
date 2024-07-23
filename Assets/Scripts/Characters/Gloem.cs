using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Boss的攻击脚本
/// </summary>
public class Gloem :Enemy
{
    [Header("Skill")]
    public float kickForce = 20f;
    public GameObject rockPrefab;
    public Transform rightPos;
    public void KickOff()
    {
        if(attackTarget != null && transform.IsfacingTarget(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();

            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;

            // 眩晕效果
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
            // 造成伤害
            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    public void RockFlyToTarget()
    {
        if (attackTarget != null)
        {
            // 生成rock
            var rock =  Instantiate(rockPrefab, rightPos.position, Quaternion.identity);
            rock.GetComponent<Rock>().target = attackTarget;
        }
        else
        {
            var rock = Instantiate(rockPrefab,rightPos.position, Quaternion.identity);
            rock.GetComponent<Rock>().target = FindObjectOfType<PlayerController>().gameObject;
        }
    }
}
