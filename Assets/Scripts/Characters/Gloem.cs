using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Boss�Ĺ����ű�
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

            // ѣ��Ч��
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
            // ����˺�
            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    public void RockFlyToTarget()
    {
        if (attackTarget != null)
        {
            // ����rock
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
