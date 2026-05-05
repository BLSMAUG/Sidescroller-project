using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject bossPrefab;

    public GameObject enemyBattleStation1GO;
    public GameObject enemyBattleStation2GO;
    public GameObject bossBattleStationGO;

    public GameObject enemySelector;

    public Transform playerBattleStation;
    public Transform enemyBattleStation1;
    public Transform enemyBattleStation2;
    public Transform bossBattleStation;

    Unit playerUnit;
    Unit enemyUnit1;
    Unit enemyUnit2;
    Unit bossUnit;
    Unit target;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemy1HUD;
    public BattleHUD enemy2HUD;

    private bool attackReady = false;
    private bool healReady = false;
    private bool selectionDone = true;
    private bool isB_Wrath = false;

    private float damage;
    private int hitCount;
    private int enemyCount;
    

    public BattleState state;
    void Start()
    {
        bossBattleStationGO.SetActive(false);
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    private void Update()
    {
        playerHUD.SetHUD(playerUnit);
        enemy1HUD.SetHUD(enemyUnit1);
        enemy2HUD.SetHUD(enemyUnit2);

        if (selectionDone == true)
        {
            enemySelector.SetActive(false);
        }
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemy1GO = Instantiate(enemyPrefab, enemyBattleStation1);
        enemyUnit1 = enemy1GO.GetComponent<Unit>();
        GameObject enemy2GO = Instantiate(enemyPrefab, enemyBattleStation2);
        enemyUnit2 = enemy2GO.GetComponent<Unit>();

        hitCount = 0;
        enemyCount = 2;

        //Debug.Log(enemyUnit.unitName);
        dialogueText.text = "Here come " + enemyCount + " " +  enemyUnit1.unitName + "s!";
        //Debug.Log(enemyUnit.unitName);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    #region PLAYER_ABILITIES
    IEnumerator ShaolinStrike(Unit unit)
    {
        damage = 15f + 0.75f * playerUnit.strength;

        bool isDead = unit.TakeDamage(damage);

        //enemy1HUD.SetHP(unit.currentHP);

        if (playerUnit.mana < playerUnit.maxMana)
        {
            playerUnit.mana += 5f;
        }

        playerHUD.SetMana(playerUnit.mana);

        dialogueText.text = "The enemy takes " + damage + " damage !";

        yield return new WaitForSeconds(2f);
        
        if (isDead)
        {
            enemyCount -= 1;
            if (enemyCount == 0)
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
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator Buddha_sWrath(Unit unit)
    {
        isB_Wrath = false;
        bool isDead = false;

        playerUnit.mana -= 30;

        damage = 3.5f * playerUnit.strength * (1f + 0.5f * hitCount);

        isDead = unit.TakeDamage(damage);
        hitCount += 1;

        dialogueText.text = "The enemy takes " + damage + " damage !";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            enemyCount -= 1;
            if (enemyCount == 0)
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
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator HealingMeditation()
    {
        bool canHeal = false;

        if (playerUnit.mana >= 10)
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
            dialogueText.text = "You don't have enough mana.";

            yield return new WaitForSeconds(2f);

            dialogueText.text = "Choose an action : ";
        }
    }

    #endregion

    IEnumerator EnemyTurn()
    {
        if (enemyCount >= 2)
        {
            dialogueText.text = "The enemies attack you for " + enemyUnit1.ennemyDamage + " x " + enemyCount + " damage!";
        }
        else
        {
            dialogueText.text = "The enemy attacks you for " + enemyUnit1.ennemyDamage + " damage!";
        }

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit1.ennemyDamage * enemyCount);

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

    #region COMBAT_BUTTONS
    public void OnShaolinStrikeButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        if (attackReady == true)
        {
            attackReady = false;
            healReady = false;
            selectionDone = false;

            enemySelector.SetActive(true);
            dialogueText.text = "Choose a target :";
        }
    }

    public void OnBuddha_sWrathButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        if (attackReady == true && playerUnit.mana >= 30)
        {
            attackReady = false;
            healReady = false;
            selectionDone = false;
            isB_Wrath = true;

            enemySelector.SetActive(true);
            dialogueText.text = "Choose a target :";
        }
        
    }

    public void OnSelectEnemy1Button()
    {
        if (isB_Wrath == true)
        {
            StartCoroutine(Buddha_sWrath(enemyUnit1));
        }
        else
        {
            StartCoroutine(ShaolinStrike(enemyUnit1));
        }
        selectionDone = true;
    }

    public void OnSelectEnemy2Button()
    {
        if (isB_Wrath == true)
        {
            StartCoroutine(Buddha_sWrath(enemyUnit2));
        }
        else
        {
            StartCoroutine(ShaolinStrike(enemyUnit2));
        }
        selectionDone = true;
    }

    public void OnHealingMeditationButton()
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

    #endregion
}
