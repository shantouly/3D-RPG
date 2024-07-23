using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// 
/// </summary>
public class Grunt : Enemy
{
    [Header("Grunt Skill")]
    public float kickForce;

    public void KickOff()
    {
        if(attackTarget != null)
        {
            // ��Ҫת��Player
            transform.LookAt(attackTarget.transform.position);

            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();// ֻ��Ҫ��ȡ���������

            // �Ƚ������ֹͣ
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
        }
    }
}
