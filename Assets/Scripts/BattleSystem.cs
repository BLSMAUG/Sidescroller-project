using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, BOSSTURN, WAVEDONE, WON, LOST }

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject bossPrefab;

    public GameObject enemyBattleStation1GO;
    public GameObject enemyBattleStation2GO;
    public GameObject bossBattleStationGO;

    public GameObject playerHudGO;
    public GameObject enemy1HudGO;
    public GameObject enemy2HudGO;
    public GameObject bossHudGO;

    public GameObject enemySelector;

    public Transform playerBattleStation;
    public Transform enemyBattleStation1;
    public Transform enemyBattleStation2;
    public Transform bossBattleStation;

    Unit playerUnit;
    Unit enemyUnit1;
    Unit enemyUnit2;
    Unit bossUnit;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemy1HUD;
    public BattleHUD enemy2HUD;
    public BattleHUD bossHUD;

    private bool attackReady = false;
    private bool healReady = false;
    private bool healDone = false;
    private bool selectionDone = true;
    private bool isB_Wrath = false;
    private bool enemy1Dead = false;
    private bool enemy2Dead = false;
    private bool waveDone = false;
    //private bool bossDead = false;
    private bool wave = false;
    private bool boss = false;

    private float damage;
    private int hitCount;
    private int enemyCount;
    private int bossAttackCount;
    private float playerDefenseDebuff;
    private float playerAttackDebuff;
    

    public BattleState state;
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    private void Update()
    {
        playerHUD.SetHUD(playerUnit);

        if (wave)
        {
            enemy1HUD.SetHUD(enemyUnit1);
            enemy2HUD.SetHUD(enemyUnit2);
        }
        else if (boss)
        {
            bossHUD.SetHUD(bossUnit);
        }

        if (selectionDone == true)
        {
            enemySelector.SetActive(false);
        }
        if (enemyCount == 0 && boss == false)
        {
            state = BattleState.WAVEDONE;
            StartCoroutine(EndBattle());
        }
    }

    IEnumerator SetupBattle()
    {
        bossBattleStationGO.SetActive(false);
        bossHudGO.SetActive(false);

        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemy1GO = Instantiate(enemyPrefab, enemyBattleStation1);
        enemyUnit1 = enemy1GO.GetComponent<Unit>();
        GameObject enemy2GO = Instantiate(enemyPrefab, enemyBattleStation2);
        enemyUnit2 = enemy2GO.GetComponent<Unit>();

        hitCount = 0;
        enemyCount = 2;
        wave = true;

        playerAttackDebuff = 1f;
        playerDefenseDebuff = 1f;

        dialogueText.text = "Here come " + enemyCount + " " +  enemyUnit1.unitName + "s!";

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        StartCoroutine(PlayerTurn());
    }

    #region PLAYER_ABILITIES
    IEnumerator ShaolinStrike(Unit unit)
    {
        damage = 15f + 0.75f * playerUnit.strength;

        bool isDead = unit.TakeDamage(damage * playerAttackDebuff);

        if (playerUnit.mana < playerUnit.maxMana)
        {
            playerUnit.mana += 5f;
        }

        dialogueText.text = "The enemy takes " + damage * playerAttackDebuff + " damage !";

        yield return new WaitForSeconds(1.5f);
        
        if (isDead)
        {
            if (wave && enemyCount == 0)
            {
                state = BattleState.WAVEDONE;
                StartCoroutine(EndBattle());
            }
            else if (boss)
            {
                state = BattleState.WON;
                StartCoroutine(EndBattle());
            }
            else
            {
                if (wave)
                {
                    state = BattleState.ENEMYTURN;
                    StartCoroutine(EnemyTurn());
                }
            }
        }
        else
        {
            if (wave)
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
            else if (boss)
            {
                state = BattleState.BOSSTURN;
                StartCoroutine(BossTurn());
            }
        }
    }

    IEnumerator Buddha_sWrath(Unit unit)
    {
        isB_Wrath = false;
        bool isDead = false;

        playerUnit.mana -= 30;
        damage = 3.5f * playerUnit.strength * (1f + 0.5f * hitCount);

        isDead = unit.TakeDamage(damage * playerAttackDebuff);
        hitCount += 1;

        dialogueText.text = "The enemy takes " + damage * playerAttackDebuff + " damage !";

        if (isDead)
        {
            if (playerUnit.mana < playerUnit.maxMana)
            {
                playerUnit.mana += 15;
            }
            if (wave && enemyCount == 0)
            {
                yield return new WaitForSeconds(1.5f);
                state = BattleState.WAVEDONE;
                StartCoroutine(EndBattle());
            }
            else if (boss)
            {
                state = BattleState.WON;
                StartCoroutine(EndBattle());
            }
            else
            {
                if (wave)
                {
                    yield return new WaitForSeconds(1.5f);
                    state = BattleState.ENEMYTURN;
                    StartCoroutine(EnemyTurn());
                }
            }
        }
        else
        {
            if (wave)
            {
                yield return new WaitForSeconds(1.5f);
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
            else if (boss)
            {
                yield return new WaitForSeconds(1.5f);
                state = BattleState.BOSSTURN;
                StartCoroutine(BossTurn());
            }
        }
    }

    IEnumerator SpiritualWave ()
    {
        bool isDead = false;

        damage = 25 + 2.5f * playerUnit.spirit;

        if (wave)
        {
            isDead = enemyUnit1.TakeDamage(damage * playerAttackDebuff);
            isDead = enemyUnit2.TakeDamage(damage * playerAttackDebuff);

            dialogueText.text = "The enemies take " + damage * playerAttackDebuff + " damage !";
        }

        else if (boss)
        {
            isDead = bossUnit.TakeDamage(damage * playerAttackDebuff);

            dialogueText.text = "The Boss takes " + damage * playerAttackDebuff + " damage !";
        }

        yield return null;
        playerUnit.mana -= 20;
        yield return new WaitForSeconds(1.5f);

        if (isDead)
        {
            if (wave && enemyCount == 0)
            {
                state = BattleState.WAVEDONE;
                StartCoroutine(EndBattle());
            }
            else if (boss)
            {
                state = BattleState.WON;
                StartCoroutine(EndBattle());
            }
            else
            {
                if (wave)
                {
                    state = BattleState.ENEMYTURN;
                    StartCoroutine(EnemyTurn());
                }
            }
        }
        else
        {
            if (wave)
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
            else if (boss)
            {
                state = BattleState.BOSSTURN;
                StartCoroutine(BossTurn());
            }
        }
    }

    IEnumerator HealingMeditation()
    {
        bool canHeal = false;

        if (playerUnit.mana >= 10)
        {
            canHeal = playerUnit.Heal(20 + playerUnit.spirit * 1.5f);
            healReady = false;
            healDone = true;
        }

        if (canHeal)
        {
            dialogueText.text = "You feel better!";

            yield return null;
            playerUnit.mana -= 10;
            yield return new WaitForSeconds(1.5f);

            playerAttackDebuff = 1f;
            playerDefenseDebuff = 1f;
            
            if (wave)
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
            if (boss)
            {
                state = BattleState.BOSSTURN;
                StartCoroutine(BossTurn());
            }
        }
    }

    #endregion

    IEnumerator EnemyTurn()
    {
        EnemyStatusChecker();

        if (enemyCount >= 2)
        {
            dialogueText.text = "The enemies attack you for " + enemyUnit1.ennemyDamage * playerDefenseDebuff + " x " + enemyCount + " damage!";
        }
        else
        {
            dialogueText.text = "The enemy attacks you for " + enemyUnit1.ennemyDamage * playerDefenseDebuff + " damage!";
        }

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit1.ennemyDamage * enemyCount * playerDefenseDebuff);

        yield return new WaitForSeconds(1.5f);

        if(isDead)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            StartCoroutine(PlayerTurn());
        }
    }

    IEnumerator BossTurn()
    {
        bool isDead = false;
        if (bossAttackCount == 0)
        {
            dialogueText.text = "The Boss attacks you for " + bossUnit.ennemyDamage * playerDefenseDebuff + " damage!";
            isDead = playerUnit.TakeDamage(bossUnit.ennemyDamage * playerDefenseDebuff);
            yield return new WaitForSeconds(1.5f);

            if (isDead)
            {
                state = BattleState.LOST;
                StartCoroutine(EndBattle());
            }
            else
            {
                bossAttackCount += 1;
                state = BattleState.PLAYERTURN;
                StartCoroutine(PlayerTurn());
            }
        }

        else if (bossAttackCount == 1)
        {
            dialogueText.text = "The Boss attacks you for " + bossUnit.ennemyDamage * playerDefenseDebuff+ " damage!";
            isDead = playerUnit.TakeDamage(bossUnit.ennemyDamage * playerDefenseDebuff);
            
            yield return new WaitForSeconds(1.5f);
            dialogueText.text = "You have a bad feeling ...";
            yield return new WaitForSeconds(1.5f);

            if (isDead)
            {
                state = BattleState.LOST;
                StartCoroutine(EndBattle());
            }
            else
            {
                bossAttackCount += 1;
                state = BattleState.PLAYERTURN;
                StartCoroutine(PlayerTurn());
            }
        }

        else if (bossAttackCount == 2)
        {
            dialogueText.text = "The Boss starts making up lies to destabilize you!";
            yield return new WaitForSeconds(1.5f);
            playerDefenseDebuff = 1.5f;
            playerAttackDebuff = 0.5f;
            dialogueText.text = "You feel weak.";
            isDead = playerUnit.TakeDamage(0);
            yield return new WaitForSeconds(1.5f);
            

            if (isDead)
            {
                state = BattleState.LOST;
                StartCoroutine(EndBattle());
            }
            else
            {
                bossAttackCount = 0;
                state = BattleState.PLAYERTURN;
                StartCoroutine(PlayerTurn());
            }
        }
    }

    IEnumerator EndBattle()
    {
        if (state == BattleState.WAVEDONE && waveDone == false)
        {
            waveDone = true;
            wave = false;

            dialogueText.text = "You have defeated the enemies!";

            yield return new WaitForSeconds(1.5f);

            enemyBattleStation1GO.SetActive(false);
            enemyBattleStation2GO.SetActive(false);
            enemy1HudGO.SetActive(false);
            enemy2HudGO.SetActive(false);

            yield return new WaitForSeconds(1.5f);

            bossBattleStationGO.SetActive(true);
            bossHudGO.SetActive(true);
            
            dialogueText.text = "Here comes the Boss !";

            GameObject bossGO = Instantiate(bossPrefab, bossBattleStation);
            bossUnit = bossGO.GetComponent<Unit>();

            boss = true;

            yield return new WaitForSeconds(1.5f);

            state = BattleState.PLAYERTURN;
            bossAttackCount = 0;
            StartCoroutine(PlayerTurn());
        }

        else if (state == BattleState.WON)
        {
            dialogueText.text = "You have defeated the Boss!";
            yield return new WaitForSeconds(1.5f);
            bossBattleStationGO.SetActive(false);
            bossHudGO.SetActive(false);
            dialogueText.text = "Congratulations, you have cleared this area of it's evil influence.";
            yield return new WaitForSeconds(1.5f);
            //changement de scène ou panel d'UI -> level up + nouvelle compétence de déplacement
        }

        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
        }
    }

    IEnumerator PlayerTurn()
    {
        EnemyStatusChecker();
        yield return null;
        dialogueText.text = "Choose an action : ";
        attackReady = true;
        healReady = true;
    }

    IEnumerator NoManaText(int mana)
    {
        if (playerUnit.mana < mana)
        {
            dialogueText.text = "You don't have enough mana.";

            yield return new WaitForSeconds(1.5f);

            dialogueText.text = "Choose an action : ";
        }
    }

    void EnemyStatusChecker()
    {
        if (enemyUnit1.currentHP == 0 && enemy1Dead == false)
        {
            enemyCount -= 1;
            enemy1Dead = true;
        }
        if (enemyUnit2.currentHP == 0 && enemy2Dead == false)
        {
            enemyCount -= 1;
            enemy2Dead = true;
        }
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

            if (wave)
            {
                enemySelector.SetActive(true);
                dialogueText.text = "Choose a target :";
            }
            if (boss)
            {
                StartCoroutine(ShaolinStrike(bossUnit));
            }
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

            if (wave)
            {
                enemySelector.SetActive(true);
                dialogueText.text = "Choose a target :";
            }
            if (boss)
            {
                StartCoroutine(Buddha_sWrath(bossUnit));
            }
        }        
    }

    public void OnSpiritualWaveButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        if (attackReady == true && playerUnit.mana >= 20)
        {
            StartCoroutine(SpiritualWave());
            attackReady = false;
            healReady = false;
        }
    }

    public void OnSpiritualWaveNoMana()
    {
        StartCoroutine(NoManaText(20));
    }

    public void OnB_WrathButtonNoMana()
    {
        StartCoroutine(NoManaText(30));
    }

    public void OnSelectEnemy1Button()
    {
        if (enemy1Dead  == false)
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
    }

    public void OnSelectEnemy2Button()
    {
        if (enemy2Dead  == false)
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

    public void OnHealingMeditationButtonNoMana()
    {
        if (healDone)
        {
            StartCoroutine(NoManaText(10));
        }        
    }

    #endregion
}
