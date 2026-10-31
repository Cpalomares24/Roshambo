
using UnityEngine;
using System.Collections;

/// <summary>
/// Coordinates combat turns, player and enemy actions, encounter progression, and battle state. Action sequences are implemented in the matching partial-class file.
/// </summary>
public partial class CombatManager : MonoBehaviour
{
    [Header("Combat Participants")]
    public CombatStats player;
    public CombatStats enemy;

    [Header("Card Inventory")]
    public CardInventory playerCards;

    [Header("Upgrade System")]
    public UpgradeManager upgradeManager;

    [Header("Combat Systems")]
    public AttackSystem attackSystem;
    public EnemyAI enemyAI;

    [Header("Pre-Boss System")]
    public PreBossManager preBossManager;

    [Header("Turn Information")]
    public int turnNumber = 1;
    public bool playerAttacksFirst;

    [Header("Combat Phase")]
    public bool isAttackPhase = true;
    public bool isPlayerTurn = true;

    [Header("Turn Progress")]
    public bool playerHasAttacked = false;
    public bool enemyHasAttacked = false;

    [Header("Combat Buttons")]
    public GameObject combatButtons;
    public GameObject defenseButtons;

    [Header("Battle Animations")]
    public BattleAnimator battleAnimator;

    [Header("Encounter System")]
    public EncounterManager encounterManager;

    [Header("Battle Messages")]
    public BattleDisplay battleDisplay;

    [Header("Game Over System")]
    public GameOverManager gameOverManager;

    [Header("Coin Flip Animation")]
    public CoinFlipAnimator coinFlipAnimator;

    private bool actionInProgress = false;

    void Start()
    {
        encounterManager.StartEncounter();
        StartTurn();
    }

    /// <summary>
    /// Handles flip coin for this component.
    /// </summary>
    public void FlipCoin()
    {
        playerAttacksFirst = Random.Range(0, 2) == 0;

        Debug.Log(
            playerAttacksFirst
                ? "Player attacks first!"
                : "Enemy attacks first!"
        );
    }

    /// <summary>
    /// Handles start turn for this component.
    /// </summary>
    public void StartTurn()
    {
        Debug.Log("Turn " + turnNumber + " started.");

        playerHasAttacked = false;
        enemyHasAttacked = false;

        FlipCoin();

        isPlayerTurn = playerAttacksFirst;
        isAttackPhase = playerAttacksFirst;

        // Wait for the coin animation before allowing card selection.
        StartCoroutine(ShowCoinFlipThenBeginTurn());
    }

    /// <summary>
    /// Handles show coin flip then begin turn for this component.
    /// </summary>
    private IEnumerator ShowCoinFlipThenBeginTurn()
    {
        actionInProgress = true;
        HideCombatButtons();

        if (coinFlipAnimator != null)
        {
            yield return StartCoroutine(
                coinFlipAnimator.PlayCoinFlip(playerAttacksFirst)
            );
        }
        else
        {
            Debug.LogWarning("CoinFlipAnimator is not assigned on CombatManager.");
        }

        battleDisplay.ShowInstantMessage(
            playerAttacksFirst
                ? "Your turn! Choose an attack."
                : "Enemy attacks first! Choose your defense."
        );

        actionInProgress = false;
        UpdateCombatButtons();
    }

    /// <summary>
    /// Handles finish defense for this component.
    /// </summary>
    public void FinishDefense()
    {
        enemyHasAttacked = true;

        if (CheckBattleOver())
            return;

        if (playerHasAttacked)
        {
            EndTurn();
        }
        else
        {
            isPlayerTurn = true;
            isAttackPhase = true;

            battleDisplay.ShowInstantMessage(
                "Choose your attack!"
            );

            UpdateCombatButtons();
        }
    }

    /// <summary>
    /// Handles end turn for this component.
    /// </summary>
    public void EndTurn()
    {
        if (CheckBattleOver())
            return;

        player.ProcessEndOfTurn();
        enemy.ProcessEndOfTurn();

        if (CheckBattleOver())
            return;

        Debug.Log("Turn " + turnNumber + " ended.");

        turnNumber++;
        StartTurn();
    }

    /// <summary>
    /// Handles check battle over for this component.
    /// </summary>
    public bool CheckBattleOver()
    {
        if (player.IsDefeated())
        {
            Debug.Log("DEFEAT! Player defeated!");
            gameOverManager.ShowDefeat();

            combatButtons.SetActive(false);
            defenseButtons.SetActive(false);

            return true;
        }

        if (enemy.IsDefeated())
        {
            Debug.Log("Enemy defeated!");
            HandleEnemyDefeat();
            return true;
        }

        return false;
    }

    // --------------------------------
    // COMBAT BUTTONS
    // --------------------------------

    /// <summary>
    /// Handles hide combat buttons for this component.
    /// </summary>
    private void HideCombatButtons()
    {
        combatButtons.SetActive(false);
        defenseButtons.SetActive(false);
    }

    /// <summary>
    /// Handles update combat buttons for this component.
    /// </summary>
    public void UpdateCombatButtons()
    {
        if (actionInProgress || battleDisplay.IsDialogueActive)
        {
            HideCombatButtons();
            return;
        }

        bool showAttack = isAttackPhase && isPlayerTurn;

        if (player.IsDefeated() || enemy.IsDefeated())
        {
            HideCombatButtons();
            return;
        }

        if (showAttack && CheckOutOfCards())
            return;

        combatButtons.SetActive(showAttack);
        defenseButtons.SetActive(!showAttack);
    }

    /// <summary>
    /// Handles switch to defense for this component.
    /// </summary>
    public void SwitchToDefense()
    {
        isAttackPhase = false;
        isPlayerTurn = false;

        battleDisplay.ShowInstantMessage(
            "Choose your defense!"
        );

        UpdateCombatButtons();

        Debug.Log("Player is now defending!");
    }

    /// <summary>
    /// Handles finish attack for this component.
    /// </summary>
    public void FinishAttack()
    {
        if (CheckBattleOver())
            return;

        playerHasAttacked = true;

        if (enemyHasAttacked)
            EndTurn();
        else
            SwitchToDefense();
    }

    // Retained for compatibility with existing scripts.
    /// <summary>
    /// Performs enemy attack for this component.
    /// </summary>
    public void EnemyAttack(bool playerBlocked)
    {
        int attackChoice = Random.Range(0, 2);

        if (attackChoice == 0)
            attackSystem.RockAttack(enemy, player, playerBlocked);
        else
            attackSystem.ScissorsAttack(enemy, player, false);
    }

    // --------------------------------
    // CARD INVENTORY
    // --------------------------------

    /// <summary>
    /// Handles check out of cards for this component.
    /// </summary>
    public bool CheckOutOfCards()
    {
        if (!playerCards.HasAttackCards())
        {
            Debug.Log("DEFEAT! You ran out of cards!");
            gameOverManager.ShowDefeat();
            HideCombatButtons();
            return true;
        }

        return false;
    }

    // --------------------------------
    // ENCOUNTER SYSTEM
    // --------------------------------

    /// <summary>
    /// Handles begin next encounter for this component.
    /// </summary>
    public void BeginNextEncounter()
    {
        if (encounterManager.AdvanceEncounter())
        {
            if (encounterManager.currentEncounter < 4)
            {
                playerCards.rockCards += 1;
                playerCards.scissorsCards += 1;

                Debug.Log(
                    "Reward: +1 Rock and +1 Scissors!"
                );
            }

            turnNumber = 1;
            StartTurn();

            Debug.Log("Next encounter ready!");
        }
    }

    /// <summary>
    /// Handles handle enemy defeat for this component.
    /// </summary>
    public void HandleEnemyDefeat()
    {
        if (!encounterManager.HasMoreEncounters())
        {
            Debug.Log("GAME COMPLETE! Boss defeated!");
            gameOverManager.ShowVictory();
            HideCombatButtons();
            return;
        }

        if (encounterManager.currentEncounter == 3)
            preBossManager.ShowPreBossChoice();
        else
            upgradeManager.ShowUpgrades();
    }
}
