using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class CharacterStats : MonoBehaviour
{
    // ����Ѫ��UI�Ĳ���
    public event Action<int, int> UpdateHealthBarOnAttack;

    [Header("����Dataģ��")]
    public CharacterStats_SO templateData;

    [Header("��ȡ�������������")]
    public CharacterStats_SO characterData;

    [Header("��ȡ���幥������")]
    public Attack_SO attackData;

    [HideInInspector]
    public bool isCritical;

    private void Awake()
    {
        if (templateData != null)
            characterData = Instantiate(templateData);
    }
    #region Read Value From CharacterData
    // ���Ѫ������
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

    // ��ǰѪ��
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

    // ��������
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

    // ��ǰ����ֵ
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

    // Player������Enemy��������ʱ
    public void TakeDamage(CharacterStats attacker,CharacterStats defener)
    {
        int damage = Math.Max(attacker.CurrentDamage() - defener.CurrentDefence,0);        

        defener.CurrentHealth = Math.Max(defener.CurrentHealth - damage,0);

        //TODO:updateUI
        //UpdateHealthBarOnAttack?.Invoke(defener.CurrentHealth,defener.MaxHealth);
        defener.UpdateHealthBarOnAttack?.Invoke(defener.CurrentHealth, defener.MaxHealth);
        //TODO:����update������������Playerʱ��
        if (defener.CurrentHealth <= 0)
            attacker.characterData.UpdateExp(defener.characterData.GetExp);
    }

    // ʯͷ����Playerʱ
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

    // ������С
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
