using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
public class BarUI : MonoBehaviour 
{ 
    public GameObject healthUIPrefab;
    public Transform barPoint;
    public bool alwaysVisble;
    public float visbleTime;
    public float time;

    Image healthSlider;
    Transform UIBar;
    Transform cam;

    CharacterStats currentStats;

    private void Awake()
    {
        currentStats = GetComponent<CharacterStats>();

        currentStats.UpdateHealthBarOnAttack += UpdateHealthBar;
    }

    private void OnEnable()
    {
        cam = Camera.main.transform;
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            // 如果渲染的模式是世界空间的话
            if(canvas.renderMode == RenderMode.WorldSpace)
            {
                UIBar = Instantiate(healthUIPrefab,canvas.transform).transform;
                healthSlider = UIBar.GetChild(0).GetComponent<Image>();
                UIBar.gameObject.SetActive(alwaysVisble);
            }
        }

    }

    private void LateUpdate()
    {
        if (UIBar != null)
        {
            UIBar.position = barPoint.position;
            UIBar.forward = -cam.forward;
        }
        if (time <= 0 && !alwaysVisble)
            UIBar.gameObject.SetActive(false);
        else
            time -= Time.deltaTime;
    }
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
            Destroy(UIBar.gameObject);
        UIBar.gameObject.SetActive(true);
        time = visbleTime;


        float silderPercent = (float)currentHealth / (float) maxHealth;
        healthSlider.fillAmount = silderPercent;

    }
}
