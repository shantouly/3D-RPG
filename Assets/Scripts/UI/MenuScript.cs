using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
public class MenuScript : MonoBehaviour
{
    Button NewGameBtn;
    Button ContinueBtn;
    Button QuitBtn;
    PlayableDirector director;


    private void Awake()
    {
        NewGameBtn = transform.GetChild(1).GetComponent<Button>();
        ContinueBtn = transform.GetChild(2).GetComponent<Button>();
        QuitBtn = transform.GetChild(3).GetComponent<Button>();

        NewGameBtn.onClick.AddListener(PlayTimeLine);
        ContinueBtn.onClick.AddListener(Continue);
        QuitBtn.onClick.AddListener(Quit);

        director = FindObjectOfType<PlayableDirector>();
        director.stopped += NewGame;
    }

    void PlayTimeLine()
    {
        director.Play();
    }

    // 点击NewGame按钮之后执行的方法
    void NewGame(PlayableDirector obj)
    {
        // 新游戏，先清空原来的数据
        PlayerPrefs.DeleteAll();

        SceneController.Instance.LoadFristLevel();
    }

    // 点击Continue按钮之后执行的方法
    void Continue()
    {
        SceneController.Instance.ContinueToLevel();

    }

    // 点击退出游戏按钮之后执行的方法
    void Quit()
    {
        Application.Quit();
        Debug.Log("退出游戏");
    }


}
