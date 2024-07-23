using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class CameraScripts : MonoBehaviour
{
    public float Smooth = 2;
    public GameObject Target;

    private Vector3 distance;

    private void Start()
    {
        distance = transform.position - Target.transform.position;
    }

    private void LateUpdate()
    {
        CameraMove();
    }
    void CameraMove()
    {
        transform.position = Vector3.Lerp(transform.position,Target.transform.position + distance ,Smooth);    
        transform.LookAt(Target.transform.position);
    }
}
