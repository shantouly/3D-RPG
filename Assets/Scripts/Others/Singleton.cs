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

    // protected --- 只有子类才能调佣，vritual --- 子类可以对此方法进行重写
    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;
    }

    // 判断是否是单例模式
    public bool Isinitialized
    {
        get
        {
            return (instance != null);
        }
    }

    // Player死亡之后OnDestory ---- 要将此时的instance变为null
    protected virtual void OnDestory()
    {
        if(instance == this)
            instance = null;

    }
}
