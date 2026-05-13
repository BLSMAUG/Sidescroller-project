using UnityEngine;


[System.Serializable]
public class PlayerBaseData
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

    public float[] position;


    public PlayerBaseData(Unit player)
    {
        unitName = player.unitName;
        unitLevel = player.unitLevel;

        strength = player.strength;
        spirit = player.spirit;
        constitution = player.constitution;

        maxMana = player.maxMana;
        mana = player.mana;

        maxHP = player.maxHP;
        currentHP = player.currentHP;

        position = new float[2];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
    }
}
