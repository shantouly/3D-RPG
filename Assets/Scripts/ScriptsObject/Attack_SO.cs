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
    public float attackRange;                   // 攻击范围
    public float skillRange;                    // 技能攻击范围
    public float coolDown;                      // 冷却时间
    public int minDamage;                      // 最小的攻击伤害
    public int maxDamage;                      // 最大的攻击伤害
    public float criticalMultiplier;            // 暴击之后原来的攻击伤害所乘的百分比
    public float criticalChance;                // 暴击率
}
