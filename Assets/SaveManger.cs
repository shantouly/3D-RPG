using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 
/// </summary>
public class SaveManger : Singleton<SaveManger>
{
    [HideInInspector]
    public string sceneName;
    public string SceneName
    {
        get
        {
            return PlayerPrefs.GetString(sceneName);
        }
    }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void SavePlayerData()
    {
        Save(GameManager.Instance.characterStats.characterData, GameManager.Instance.characterStats.characterData.name);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
        }
        else if(Input.GetKeyDown(KeyCode.P))
        {
            LoadPlayerData();
        }else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.Instance.LoadMainLevel();
        }
    }

    public void LoadPlayerData()
    {
        Load(GameManager.Instance.characterStats.characterData, GameManager.Instance.characterStats.characterData.name);
    }

    public void Save(object data,string key)
    {
        // 以string类型的方式存储
        var jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.SetString(sceneName,SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }

    public void Load(object data,string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
        else
            return;
    }
}
