using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
public class LoadManager : Singleton<LoadManager>
{
    public GameObject LoadBg;
    public GameObject PlayerUI;
    public Slider slider;
    public Text text;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void LoadNextLevel(Transform destination)
    {
        StartCoroutine(Load(destination));
    }
    IEnumerator Load(Transform destination)
    {
        PlayerUI.SetActive(false);
        LoadBg.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex+1);

        if (!operation.isDone)
        {
            slider.value = operation.progress;

            text.text = operation.progress * 100 + "%";
            operation.allowSceneActivation = false;

            if (operation.progress >= 0.9f)
            {
                slider.value = 1;
                PlayerUI.SetActive(true);

                operation.allowSceneActivation = true;
                Instantiate(SceneController.Instance.playerPrefab, destination.position, Quaternion.identity);
            }             
            yield return null;
        }
    }

}
