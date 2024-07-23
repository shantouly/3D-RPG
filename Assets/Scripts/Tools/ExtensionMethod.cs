using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public static class ExtensionMethod
{
    private const float angle = 0.5f;

    // 当敌人攻击的时候需要判断Player是否在前方的某个范围，如果不在的话，不会造成伤害
    // 用this关键字的话，该类已经添加为Transform的一个静方法了，可以之间进行调用
    public static bool IsfacingTarget(this Transform transform, Transform target)
    {
        Vector3 vectorToTarget = target.position -  transform.position;
        vectorToTarget.Normalize();                     // 单位化，只用获取其方向即可

        float dot = Vector3.Dot(transform.forward, vectorToTarget);


        // Vector3.Dot(Vector3 lhs,Vector3 rhs) 返回的是两个向量角之间的cos值，如果rhs在lhs正前方的话(角度为0)，返回1
        // 在正后方的话返回-1，左边和右边都是0

        return dot >= angle;
    }
}
