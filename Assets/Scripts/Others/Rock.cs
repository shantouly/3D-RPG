using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// 可以由石头人扔向Player，也可以由Player反弹到石头人上
/// </summary>
/// 
public enum RockStates { HitPlayer,HitGloem,HitNothing}
public class Rock : MonoBehaviour
{
    
    private Rigidbody rgb;
    public RockStates rockStates;
    private Vector3 direction;

    [Header("Basic Settings")]
    public float force;
    public GameObject target;
    public int damage;
    public GameObject breakEffect;

    private void Start()
    {  
        rgb = GetComponent<Rigidbody>();
        rgb.velocity = Vector3.zero;
        FlyToTarget();
    }

    private void FixedUpdate()
    {
        if(rgb.velocity.sqrMagnitude < 1){
            rockStates = RockStates.HitNothing;
        }
    }

    public void FlyToTarget()
    {

        if(target == null)
            target = FindObjectOfType<PlayerController>().gameObject;
        direction = (target.transform.position - transform.position + Vector3.up).normalized;
        // 使用rgb的Addforce来添加一个冲击力 --- impulse(Force的类型是冲击力(Impulse)
        rgb.AddForce(direction * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (rockStates)
        {
            case RockStates.HitPlayer:
                if (collision.gameObject.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    collision.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;

                    collision.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    collision.gameObject.GetComponent<CharacterStats>().TakeDamage(damage, collision.gameObject.GetComponent<CharacterStats>());

                    rockStates = RockStates.HitNothing;
                }
                break;
            case RockStates.HitGloem:
                if (collision.gameObject.CompareTag("Enemy"))
                {
                    var GloemStats = collision.gameObject.GetComponent<CharacterStats>();
                    GloemStats.TakeDamage(damage, GloemStats);

                    Debug.Log(GloemStats.CurrentHealth);
                    Instantiate(breakEffect, transform.position, Quaternion.identity);
                    GameManager.Instance.characterStats.GetRockExp(GameManager.Instance.characterStats);  
                    Destroy(gameObject);
                }
                break;
        }
    }
}



