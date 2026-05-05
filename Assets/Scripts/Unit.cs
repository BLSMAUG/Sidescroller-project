using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

        //if (mana > 10)
        //{
        //    mana -= 10;

        //}
        //return false;
    }
    

}


