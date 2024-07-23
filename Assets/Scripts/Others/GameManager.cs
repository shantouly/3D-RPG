using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class GameManager : Singleton<GameManager>
{
    public CharacterStats characterStats;
    private CinemachineFreeLook followCamera;

    List<IEndGameObverser> endGameObservers = new List<IEndGameObverser>();

    protected override void Awake()
    {
        base.Awake();
        //FindObjectOfType<PlayerController>().gameObject.SetActive(true);
        DontDestroyOnLoad(this);
    }
    public void RigisterPlayer(CharacterStats Player)
    {
        characterStats = Player;

        followCamera = FindObjectOfType<CinemachineFreeLook>();
        if(followCamera != null )
        {
            followCamera.Follow = characterStats.transform;
            followCamera.LookAt = characterStats.transform;
        }
    }
    // ���Observers
    public void AddObverser(IEndGameObverser Observer)
    {
        endGameObservers.Add(Observer);
    }
    // �Ƴ�Observers
    public void RemoveObverser(IEndGameObverser Observer)
    {
        endGameObservers.Remove(Observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }
}
