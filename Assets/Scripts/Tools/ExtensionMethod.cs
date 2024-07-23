using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public static class ExtensionMethod
{
    private const float angle = 0.5f;

    // �����˹�����ʱ����Ҫ�ж�Player�Ƿ���ǰ����ĳ����Χ��������ڵĻ�����������˺�
    // ��this�ؼ��ֵĻ��������Ѿ����ΪTransform��һ���������ˣ�����֮����е���
    public static bool IsfacingTarget(this Transform transform, Transform target)
    {
        Vector3 vectorToTarget = target.position -  transform.position;
        vectorToTarget.Normalize();                     // ��λ����ֻ�û�ȡ�䷽�򼴿�

        float dot = Vector3.Dot(transform.forward, vectorToTarget);


        // Vector3.Dot(Vector3 lhs,Vector3 rhs) ���ص�������������֮���cosֵ�����rhs��lhs��ǰ���Ļ�(�Ƕ�Ϊ0)������1
        // �����󷽵Ļ�����-1����ߺ��ұ߶���0

        return dot >= angle;
    }
}
