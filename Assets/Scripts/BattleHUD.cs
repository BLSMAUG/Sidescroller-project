using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    
    public Text HPText;
    public float hpValue;

    public Text manaText;
    public float manaValue;

    public Slider hpSlider;
    public Slider manaSlider;

    //public void SetHPPercent(Unit unit)
    //{
    //    hpValue = unit.currentHP * 100 / unit.maxHP;
    //    HPText.text = hpValue + "%";
    //}
    
    //public void SetManaPercent(Unit unit)
    //{
    //    manaValue = unit.mana * 100 / unit.maxMana;
    //    manaText.text = manaValue + "%";
    //}

    public void SetHUD(Unit unit)
    {
        hpValue = unit.currentHP;
        manaValue = unit.mana;

        HPText.text = hpValue + " / " + unit.maxHP;
        manaText.text = manaValue + " / " + unit.maxMana;

        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;

        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;

        manaSlider.maxValue = unit.maxMana;
        manaSlider.value = unit.mana;

    }

    public void SetHP(float hp)
    {
        hpSlider.value = hp;
    }

    public void SetMana(float mana)
    {
        manaSlider.value = mana;
    }
}
