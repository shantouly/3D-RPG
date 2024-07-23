using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class CharacterStats : MonoBehaviour
{
    // 更新血条UI的操作
    public event Action<int, int> UpdateHealthBarOnAttack;

    [Header("敌人Data模版")]
    public CharacterStats_SO templateData;

    [Header("读取物体基本的属性")]
    public CharacterStats_SO characterData;

    [Header("获取物体攻击属性")]
    public Attack_SO attackData;

    [HideInInspector]
    public bool isCritical;

    private void Awake()
    {
        if (templateData != null)
            characterData = Instantiate(templateData);
    }
    #region Read Value From CharacterData
    // 最大血量设置
    public int MaxHealth
    {
        get
        {
            if (characterData != null)
                return characterData.MaxHealth;
            else
                return 0;
        }
        set
        {
            characterData.MaxHealth = value;
        }
    }

    // 当前血量
    public int CurrentHealth
    {
        get
        {
            if (characterData != null)
                return characterData.CurrentHealth;
            else
                return 0;
        }

        set
        {
            characterData.CurrentHealth = value;
        }
    }

    // 基础防御
    public int BaseDefence
    {
        get
        {
            if(characterData != null)
                return characterData.BaseDefence;
            else 
                return 0;
        }
    }

    // 当前防御值
    public int CurrentDefence
    {
        get
        {
            if (characterData != null)
                return characterData.CurrentDefence;
            else
                return 0;
        }
    }

    #endregion

    #region Character Combat

    // Player和其他Enemy发生攻击时
    public void TakeDamage(CharacterStats attacker,CharacterStats defener)
    {
        int damage = Math.Max(attacker.CurrentDamage() - defener.CurrentDefence,0);        

        defener.CurrentHealth = Math.Max(defener.CurrentHealth - damage,0);

        //TODO:updateUI
        //UpdateHealthBarOnAttack?.Invoke(defener.CurrentHealth,defener.MaxHealth);
        defener.UpdateHealthBarOnAttack?.Invoke(defener.CurrentHealth, defener.MaxHealth);
        //TODO:经验update（当攻击者是Player时）
        if (defener.CurrentHealth <= 0)
            attacker.characterData.UpdateExp(defener.characterData.GetExp);
    }

    // 石头攻击Player时
    public void TakeDamage(int damage,CharacterStats defener)
    {
        int currentDamage = Math.Max(damage - defener.CurrentDefence, 0);
        CurrentHealth = Math.Max(defener.CurrentHealth - currentDamage, 0);

        UpdateHealthBarOnAttack?.Invoke(defener.CurrentHealth,defener.MaxHealth);
    }

    public void GetRockExp(CharacterStats player)
    {
        player.characterData.UpdateExp(player.characterData.GetExp);
    }

    // 攻击大小
    private int CurrentDamage()
    {   
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage,attackData.maxDamage);

        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
        }

        return (int)coreDamage;
    }
    #endregion

    public void ShowBarUI(CharacterStats characterStats)
    {
        UpdateHealthBarOnAttack?.Invoke(characterStats.CurrentHealth, characterStats.MaxHealth);
    }
}
