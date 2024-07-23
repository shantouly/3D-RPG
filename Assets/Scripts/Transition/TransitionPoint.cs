using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType
    {
        SameScene,DifferentScene
    }

    [Header("Transition Info")]
    public string sceneName;
    public TransitionType transitionType;
    public TransitionDestination.DestinationType destinationType;

    private bool canTrans;
   

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canTrans) {
            SceneController.Instance.TransitionToDestination(this);

            //LoadManager.Instance.LoadNextLevel(SceneController.Instance.GetDestination(destinationType));
        }      
    }

    private void OnTriggerEnter(Collider other)
    {
        canTrans = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canTrans = false;
        }
    }
}
