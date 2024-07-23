using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
public class PlayerUI : MonoBehaviour
{
    Text PlayerLevel;
    Image HealthSlider;
    Image ExpSlider;

    private void Awake()
    {
        PlayerLevel = transform.GetChild(2).GetComponent<Text>();
        HealthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        ExpSlider = transform.GetChild(1).GetChild(0).GetComponent <Image>();
    }

    private void Update()
    {
        PlayerLevel.text = "Level" + GameManager.Instance.characterStats.characterData.CurrentLevel.ToString("00");
        PlayerUIUpdate();
    }

    void PlayerUIUpdate()
    {
        HealthBarUpdate();
        ExpBarUpdate();
    }

    void HealthBarUpdate()
    {
        float healthSlider = (float)(GameManager.Instance.characterStats.CurrentHealth) / (float)(GameManager.Instance.characterStats.MaxHealth);
        HealthSlider.fillAmount = healthSlider;
    }

    void ExpBarUpdate()
    {
        float expSlider = (float)(GameManager.Instance.characterStats.characterData.CurrentExp) /(float)( GameManager.Instance.characterStats.characterData.BaseExp);
        ExpSlider.fillAmount = expSlider;
    }
}
