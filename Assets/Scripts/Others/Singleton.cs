using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    // protected --- ֻ��������ܵ�Ӷ��vritual --- ������ԶԴ˷���������д
    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;
    }

    // �ж��Ƿ��ǵ���ģʽ
    public bool Isinitialized
    {
        get
        {
            return (instance != null);
        }
    }

    // Player����֮��OnDestory ---- Ҫ����ʱ��instance��Ϊnull
    protected virtual void OnDestory()
    {
        if(instance == this)
            instance = null;

    }
}
