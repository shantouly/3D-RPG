using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
public class SceneController : Singleton<SceneController>,IEndGameObverser
{
    //[Header("Load UI")]
    //public GameObject LoadBg;
    //public Slider slider;
    //public Text text;

    public GameObject playerPrefab;
    GameObject Player;
    NavMeshAgent Agent;
    public SceneFader sceneFaderPrefab;
    bool FadeFinish;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        GameManager.Instance.AddObverser(this);
        FadeFinish = true;
    }

    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch(transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:              
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationType));
                //LoadManager.Instance.LoadNextLevel();
                break;
            case TransitionPoint.TransitionType.DifferentScene:
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationType));
                break;
        }
    }

    #region ��Ϸ�в�ͬ������ת��
    IEnumerator Transition(string sceneName, TransitionDestination.DestinationType destinationType)
    {
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            //LoadBg.SetActive(true);
            //AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            //if (!operation.isDone)
            //{
            //    operation.allowSceneActivation = false;
            //    while (operation.progress < 0.9f)
            //    {
            //        slider.value = operation.progress;
            //        text.text = operation.progress * 100 + "%";

            //        operation.allowSceneActivation = true;
            //        
            //    }
            //}         
            SaveManger.Instance.SavePlayerData();
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return Instantiate(playerPrefab, GetDestination(destinationType).position, Quaternion.identity);
            SaveManger.Instance.LoadPlayerData();

            yield return null;

        }
        else
        {

            Player = GameManager.Instance.characterStats.gameObject;
            Agent = Player.GetComponent<NavMeshAgent>();
            Agent.enabled = false;
            // ͬ�������صķ���
            Player.transform.SetPositionAndRotation(GetDestination(destinationType).position, GetDestination(destinationType).rotation);
            Agent.enabled = true;
            yield return null;
        }
    }
#endregion

    // ���ص���һ������ķ���
    public void LoadFristLevel()
    {
        StartCoroutine(LoadLevel("Game"));
    }

    // ����������ķ���
    public void LoadMainLevel()
    {
        StartCoroutine(LoadMain());
    }

    // ������Ϸʱ���ص����Ǹ������ķ���
    public void ContinueToLevel()
    {
        StartCoroutine(LoadLevel(SaveManger.Instance.SceneName));
    }

    public Transform GetDestination(TransitionDestination.DestinationType destinationType)
    {
        var entries = FindObjectsOfType<TransitionDestination>();
        foreach (var entry in entries)
        {
            if (entry.destinationType == destinationType)
                return entry.transform;
        }
        return null;
    }

    IEnumerator LoadLevel(string scene)
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        if (scene != "")
        {

            yield return StartCoroutine(fade.FadeOut(2.5f));
            yield return SceneManager.LoadSceneAsync(scene);
            yield return Instantiate(playerPrefab, (Instance.GetDestination(TransitionDestination.DestinationType.ENTRY)).position, Quaternion.identity);


            // �洢����
            SaveManger.Instance.SavePlayerData();
            yield return StartCoroutine(fade.FadeIn(2.5f));

            yield break;
        }
    }

    // ����������
    IEnumerator LoadMain()
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        yield return StartCoroutine(fade.FadeOut(2.5f));
        yield return SceneManager.LoadSceneAsync("Main");
        yield return StartCoroutine(fade.FadeIn(2.5f));
        yield break;
    }


    // ������֮����еĲ���
    public void EndNotify()
    {
        if (FadeFinish)
        {
            FadeFinish = false;
            StartCoroutine(LoadMain());
        }
    }
}
