using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
/// 
[CreateAssetMenu(fileName = "New Attack", menuName = "CharacterStats/Attack")]
public class Attack_SO : ScriptableObject
{
    [Header("AttackStats")]
    public float attackRange;                   // ������Χ
    public float skillRange;                    // ���ܹ�����Χ
    public float coolDown;                      // ��ȴʱ��
    public int minDamage;                      // ��С�Ĺ����˺�
    public int maxDamage;                      // ���Ĺ����˺�
    public float criticalMultiplier;            // ����֮��ԭ���Ĺ����˺����˵İٷֱ�
    public float criticalChance;                // ������
}
