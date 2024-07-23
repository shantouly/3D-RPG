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

    // ���NewGame��ť֮��ִ�еķ���
    void NewGame(PlayableDirector obj)
    {
        // ����Ϸ�������ԭ��������
        PlayerPrefs.DeleteAll();

        SceneController.Instance.LoadFristLevel();
    }

    // ���Continue��ť֮��ִ�еķ���
    void Continue()
    {
        SceneController.Instance.ContinueToLevel();

    }

    // ����˳���Ϸ��ť֮��ִ�еķ���
    void Quit()
    {
        Application.Quit();
        Debug.Log("�˳���Ϸ");
    }


}
