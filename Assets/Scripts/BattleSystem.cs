using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    private bool attackReady = false;
    private bool healReady = false;

    private float damage;

    public BattleState state;
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    private void Update()
    {
        playerHUD.SetHUD(playerUnit);
        playerHUD.SetHUD(playerUnit);

        enemyHUD.SetHUD(enemyUnit);
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        //Debug.Log(enemyUnit.unitName);
        dialogueText.text = "Here comes a " + enemyUnit.unitName + "!";
        //Debug.Log(enemyUnit.unitName);

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator Shaolin_sStrike()
    {
        damage = 15f + 0.75f * playerUnit.strength;

        bool isDead = enemyUnit.TakeDamage(damage);

        enemyHUD.SetHP(enemyUnit.currentHP);

        if (playerUnit.mana < playerUnit.maxMana)
        {
            playerUnit.mana += 5f;
        }

        playerHUD.SetMana(playerUnit.mana);

        dialogueText.text = "The enemy takes " + damage + " damage !";

        yield return new WaitForSeconds(2f);
        
        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator HealingMeditation()
    {
        bool canHeal = false;

        if (playerUnit.mana > 10)
        {
            playerUnit.mana -= 10;
            canHeal = playerUnit.Heal(playerUnit.mana);
            healReady = false;
        }

        if (canHeal)
        {
            playerHUD.SetMana(playerUnit.mana);
            playerHUD.SetHP(playerUnit.currentHP);

            dialogueText.text = "You feel better!";

            yield return new WaitForSeconds(2f);

            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
        else
        {
            dialogueText.text = "You don't have enough spirit.";

            yield return new WaitForSeconds(2f);

            dialogueText.text = "Choose an action : ";
        }
    }

    
    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.ennemyDamage);

        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if(isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if(state == BattleState.WON)
        {
            dialogueText.text = "You have defeated the enemy!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action : ";
        attackReady = true;
        healReady = true;
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        if (attackReady == true)
        {
            attackReady = false;
            healReady = false;
            StartCoroutine(Shaolin_sStrike());
        }
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        if (healReady == true)
        {
            healReady = false;
            attackReady = false;
            StartCoroutine(HealingMeditation());
        }
    }
}
