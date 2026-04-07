using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{

    public string unitName;
    public int unitLevel;

    public int damage;
    public int heal;

    public int maxSpirit;
    public int spirit;

    public int maxHP;
    public int currentHP;

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0 )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Heal(int amount)
    {
        if (spirit > 10)
        {
            spirit -= 10;
            currentHP += amount;
            if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }
            return true;
        }
        return false;
        
    }
    

}
