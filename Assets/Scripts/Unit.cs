using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Unit : MonoBehaviour
{

    public string unitName;
    public int unitLevel;

    public int strength;
    public int spirit;
    public int constitution;

    public float maxMana;
    public float mana;

    public float maxHP;
    public float currentHP;

    public float ennemyDamage;

    public static Unit instance;

    private void Awake()
    {
        instance = this;
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        unitName = data.unitName;
        unitLevel = data.unitLevel;

        strength = data.strength;
        spirit = data.spirit;
        constitution = data.constitution;

        maxMana = data.maxMana;
        mana = data.mana;

        maxHP = data.maxHP;
        currentHP = data.currentHP;

        Vector2 position;
        position.x = data.position[0];
        position.y = data.position[1];
        transform.position = position;
    }
    public void SaveBasePlayer()
    {
        Debug.Log("a");
        SaveBaseData.SavePlayerBaseData(this);
    }

    public void LoadBasePlayer()
    {
        PlayerBaseData data = SaveBaseData.LoadPlayer();

        unitName = data.unitName;
        unitLevel = data.unitLevel;

        strength = data.strength;
        spirit = data.spirit;
        constitution = data.constitution;

        maxMana = data.maxMana;
        mana = data.mana;

        maxHP = data.maxHP;
        currentHP = data.currentHP;

        Vector2 position;
        position.x = data.position[0];
        position.y = data.position[1];
        transform.position = position;
    }

    public bool TakeDamage(float dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0 )
        {
            currentHP = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Heal(float amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        return true;
    }
    

}


