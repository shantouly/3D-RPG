using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ͨ��������������Player���ƶ������ô˽ű��е�OnMouseClick
/// </summary>
/// 
// ����EventVector3�࣬ʹ��MouseManager�����¼���������ΪVector3 --- Serializable�����ࡢö�١��ṹ��Ч
//[System.Serializable]
//public class EventVector3 : UnityEvent<Vector3>{ }
public class MouseManager : Singleton<MouseManager>
{
    //public static MouseManager instance;        // ����ģʽ
    public RaycastHit hitInfo;                  // ��ײ����Ϣ
    // ActionӦ����һ��ί��
    public event Action<Vector3> OnMouseClick;  // ����unity���Դ����¼������еķ����еĲ������ͱ�����Vector3����
    public event Action<GameObject> OnEnemyClick;   // ����˵����ʱ��

    public Texture2D Ground,Attack,Rock,Portal;
    //private void Awake()
    //{
    //    if(instance != null)
    //        Destroy(gameObject);                // �����ǰ�еĻ�s���������٣�����ģʽֻ����һ����
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

    // ��������ڲ�ͬObject�µ�ͼ��
    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);        // ���������������λ��
        if(Physics.Raycast(ray,out hitInfo))
        {
            // ��������������Ϣ�ģ�Ҳ�����з���ֵ�Ļ�s
            // �л�����ͼ��
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

    // ������¼��Ĵ����������ĳTag�򴥷���Ӧ���¼�
    void MouseControl()
    {
        // �����������������������ײ���������Ϣ�еĻ�
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.transform.CompareTag("Ground"))
            {
                // ������ǵذ�Ļ���Player�ƶ���������ĵط�
                // OnMouseClick��Ϊ�յĻ�
                OnMouseClick?.Invoke(hitInfo.point);
            }
            if (hitInfo.collider.gameObject.transform.CompareTag("Enemy"))
            {
                // ������ǵ��˵Ļ���Player�ƶ���������ĵط�
                // OnEnemyClick��Ϊ�յĻ�
                OnEnemyClick?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.transform.CompareTag("Attackable"))
            {
                OnEnemyClick?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.transform.CompareTag("Portal"))
            {
                // ������ǵذ�Ļ���Player�ƶ���������ĵط�
                // OnMouseClick��Ϊ�յĻ�
                OnMouseClick?.Invoke(hitInfo.point);
            }
        }
    }
   
}
