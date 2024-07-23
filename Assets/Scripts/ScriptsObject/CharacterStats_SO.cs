using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = " New Data", menuName = "CharacterStats/Data")] 
public class CharacterStats_SO : ScriptableObject
{
    public int GetExp;

    [Header("Stats Info")]
    public int MaxHealth;
    public int CurrentHealth;
    public int BaseDefence;
    public int CurrentDefence;

    [Header("Level")]
    public int CurrentLevel;
    public int MaxLevel;
    public int BaseExp;
    public int CurrentExp;
    public float levelBuff;
    public float LevelMutiplier
    {
        get
        {
            return 1 + (CurrentLevel - 1) * levelBuff;
        }
    }
    public void UpdateExp(int Exp)
    {
        CurrentExp += Exp;

        if(CurrentExp >= BaseExp)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        CurrentLevel = Mathf.Clamp(CurrentLevel + 1, 0, MaxLevel);

        BaseExp += (int)(BaseExp * LevelMutiplier);
        MaxHealth = (int)(MaxHealth * LevelMutiplier);

        CurrentHealth = MaxHealth;
    }

}
