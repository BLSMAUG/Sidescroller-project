using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    
    public Text HPText;
    public float hpValue;

    public Text spiritText;
    public float spiritValue;

    public Slider hpSlider;
    public Slider spiritSlider;

    public void SetHPPercent(Unit unit)
    {
        hpValue = unit.currentHP * 100 / unit.maxHP;
        HPText.text = hpValue + "%";
    }
    
    public void SetSpiritPercent(Unit unit)
    {
        spiritValue = unit.spirit * 100 / unit.maxSpirit;
        spiritText.text = spiritValue + "%";
    }

    public void SetHUD(Unit unit)
    {
        HPText.text = hpValue + "%";
        spiritText.text = spiritValue + "%";

        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;

        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;

        spiritSlider.maxValue = unit.maxSpirit;
        spiritSlider.value = unit.spirit;

    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

    public void SetSpirit(int spirit)
    {
        spiritSlider.value = spirit;
    }
}
