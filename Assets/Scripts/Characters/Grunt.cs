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
            // 先要转向Player
            transform.LookAt(attackTarget.transform.position);

            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();// 只需要获取方向就行了

            // 先将其进行停止
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
        }
    }
}
