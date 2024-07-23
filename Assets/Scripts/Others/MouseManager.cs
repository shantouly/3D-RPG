using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 通过空物体来管理Player的移动，调用此脚本中的OnMouseClick
/// </summary>
/// 
// 设置EventVector3类，使得MouseManager中有事件框，且类型为Vector3 --- Serializable仅对类、枚举、结构有效
//[System.Serializable]
//public class EventVector3 : UnityEvent<Vector3>{ }
public class MouseManager : Singleton<MouseManager>
{
    //public static MouseManager instance;        // 单例模式
    public RaycastHit hitInfo;                  // 碰撞的信息
    // Action应该是一个委托
    public event Action<Vector3> OnMouseClick;  // 创建unity中自带的事件，其中的方法中的参数类型必须是Vector3类型
    public event Action<GameObject> OnEnemyClick;   // 与敌人点击的时间

    public Texture2D Ground,Attack,Rock,Portal;
    //private void Awake()
    //{
    //    if(instance != null)
    //        Destroy(gameObject);                // 如果当前有的话s，进行销毁（单例模式只能有一个）
    //    instance = this;
    //}

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        SetCursorTexture();
        MouseControl(); 
        //ButtonControl();
    }

    // 设置鼠标在不同Object下的图标
    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);        // 返回鼠标点击的坐标位置
        if(Physics.Raycast(ray,out hitInfo))
        {
            // 如果点击的是有信息的，也就是有返回值的话s
            // 切换鼠标的图标
            switch (hitInfo.collider.gameObject.transform.tag)
            {
                case "Ground":
                    Cursor.SetCursor(Ground, new Vector3(16, 16), CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(Attack, new Vector3(16, 16), CursorMode.Auto);
                    break;
                case "Attackable":
                    Cursor.SetCursor(Rock,new Vector2(16,16),CursorMode.Auto);
                    break;
                case "Portal":
                    Cursor.SetCursor(Portal, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }

    // 鼠标点击事件的触发，点击了某Tag则触发相应的事件
    void MouseControl()
    {
        // 如果按下鼠标左键并且射线碰撞的物体的信息有的话
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.transform.CompareTag("Ground"))
            {
                // 点击的是地板的话，Player移动至鼠标点击的地方
                // OnMouseClick不为空的话
                OnMouseClick?.Invoke(hitInfo.point);
            }
            if (hitInfo.collider.gameObject.transform.CompareTag("Enemy"))
            {
                // 点击的是敌人的话，Player移动至鼠标点击的地方
                // OnEnemyClick不为空的话
                OnEnemyClick?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.transform.CompareTag("Attackable"))
            {
                OnEnemyClick?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.transform.CompareTag("Portal"))
            {
                // 点击的是地板的话，Player移动至鼠标点击的地方
                // OnMouseClick不为空的话
                OnMouseClick?.Invoke(hitInfo.point);
            }
        }
    }
   
}
