using UnityEngine;
using System.Collections;

/// <summary>
/// Implements attack and defense button handlers and their animated combat sequences. This is part of the CombatManager partial class, not a separate Unity component.
/// </summary>
public partial class CombatManager
{

    // --------------------------------
    // PLAYER ROCK ATTACK
    // --------------------------------

    /// <summary>
    /// Handles player rock attack for this component.
    /// </summary>
    public void PlayerRockAttack()
    {
        if (actionInProgress || battleDisplay.IsDialogueActive)
            return;

        if (playerCards.rockCards <= 0)
            return;

        // Play immediately when the card is selected.
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayRock();

        StartCoroutine(PlayerRockSequence());
    }

    /// <summary>
    /// Handles player rock sequence for this component.
    /// </summary>
    private IEnumerator PlayerRockSequence()
    {
        actionInProgress = true;
        HideCombatButtons();

        playerCards.rockCards--;

        battleDisplay.ShowInstantMessage("You used Rock!");

        yield return StartCoroutine(
            battleAnimator.PlayAttackSymbol(true, 0)
        );

        yield return new WaitForSeconds(0.5f);

        int enemyDefense = enemyAI.ChooseDefense(0);

        yield return StartCoroutine(
            PlayEnemyDefense(enemyDefense)
        );

        if (enemyDefense == 2)
            enemy.AddShield(enemy.paperShield);

        bool blocked = enemyDefense == 0;

        int previousHealth = enemy.currentHealth;

        attackSystem.RockAttack(player, enemy, blocked);

        string resultMessage;

        if (blocked)
        {
            resultMessage = "Your Rock attack was blocked!";
        }
        else
        {
            int damageDealt =
                previousHealth - enemy.currentHealth;

            resultMessage = damageDealt > 0
                ? "Enemy took " + damageDealt + " damage!"
                : "Enemy's Shield absorbed the attack!";
        }

        yield return StartCoroutine(
            battleDisplay.ShowMessageAndWait(resultMessage)
        );

        actionInProgress = false;
        FinishAttack();
    }

    // --------------------------------
    // PLAYER SCISSORS ATTACK
    // --------------------------------

    /// <summary>
    /// Handles player scissors attack for this component.
    /// </summary>
    public void PlayerScissorsAttack()
    {
        if (actionInProgress || battleDisplay.IsDialogueActive)
            return;

        if (playerCards.scissorsCards <= 0)
            return;

        // Play immediately when the card is selected.
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayScissors();

        StartCoroutine(PlayerScissorsSequence());
    }

    /// <summary>
    /// Handles player scissors sequence for this component.
    /// </summary>
    private IEnumerator PlayerScissorsSequence()
    {
        actionInProgress = true;
        HideCombatButtons();

        playerCards.scissorsCards--;

        battleDisplay.ShowInstantMessage("You used Scissors!");

        yield return StartCoroutine(
            battleAnimator.PlayAttackSymbol(true, 1)
        );

        yield return new WaitForSeconds(0.5f);

        int enemyDefense = enemyAI.ChooseDefense(1);

        yield return StartCoroutine(
            PlayEnemyDefense(enemyDefense)
        );

        if (enemyDefense == 2)
            enemy.AddShield(enemy.paperShield);

        bool countered = enemyDefense == 1;

        int previousEnemyHealth = enemy.currentHealth;
        int previousPlayerHealth = player.currentHealth;

        attackSystem.ScissorsAttack(player, enemy, countered);

        string resultMessage;

        if (countered)
        {
            int damageTaken =
                previousPlayerHealth - player.currentHealth;

            resultMessage = damageTaken > 0
                ? "Your Scissors was countered! You took " +
                  damageTaken + " damage!"
                : "Your Scissors was countered! Shield absorbed it!";
        }
        else
        {
            int damageDealt =
                previousEnemyHealth - enemy.currentHealth;

            resultMessage = damageDealt > 0
                ? "Enemy took " + damageDealt +
                  " damage and is bleeding!"
                : "Enemy's Shield absorbed your Scissors!";
        }

        yield return StartCoroutine(
            battleDisplay.ShowMessageAndWait(resultMessage)
        );

        actionInProgress = false;
        FinishAttack();
    }

    // --------------------------------
    // PLAYER PAPER ATTACK
    // --------------------------------

    /// <summary>
    /// Handles player paper attack for this component.
    /// </summary>
    public void PlayerPaperAttack()
    {
        if (actionInProgress || battleDisplay.IsDialogueActive)
            return;

        if (playerCards.paperCards <= 0)
            return;

        // Play immediately when the card is selected.
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayPaper();

        StartCoroutine(PlayerPaperSequence());
    }

    /// <summary>
    /// Handles player paper sequence for this component.
    /// </summary>
    private IEnumerator PlayerPaperSequence()
    {
        actionInProgress = true;
        HideCombatButtons();

        playerCards.paperCards--;

        battleDisplay.ShowInstantMessage("You used Paper!");

        yield return StartCoroutine(
            battleAnimator.PlayPaperAnimation(true)
        );

        player.AddShield(player.paperShield);

        yield return new WaitForSeconds(0.5f);

        int enemyDefense = enemyAI.ChooseDefense(2);

        yield return StartCoroutine(
            PlayEnemyDefense(enemyDefense, true)
        );

        if (enemyDefense == 2)
            enemy.AddShield(enemy.paperShield);

        yield return StartCoroutine(
            battleDisplay.ShowMessageAndWait(
                "You gained " + player.paperShield + " Shield!"
            )
        );

        actionInProgress = false;
        FinishAttack();
    }

    // --------------------------------
    // ENEMY DEFENSE ANIMATION
    // --------------------------------

    /// <summary>
    /// Handles play enemy defense for this component.
    /// </summary>
    private IEnumerator PlayEnemyDefense(int enemyDefense, bool paperAttack = false)
    {
        string defenseName = enemyDefense == 0
            ? "Block"
            : enemyDefense == 1
                ? "Counter"
                : "Paper";

        battleDisplay.ShowInstantMessage(
            "Enemy used " + defenseName + "!"
        );

        // Play the defense sound when the defense animation begins.
        // Rock/Scissors Block and Counter sounds are handled by AttackSystem,
        // so only play those here when Paper was the player's attack.
        if (AudioManager.Instance != null)
        {
            if (enemyDefense == 2)
                AudioManager.Instance.PlayPaper();
            else if (paperAttack && enemyDefense == 0)
                AudioManager.Instance.PlayBlock();
            else if (paperAttack && enemyDefense == 1)
                AudioManager.Instance.PlayCounter();
        }

        if (enemyDefense == 2)
        {
            yield return StartCoroutine(
                battleAnimator.PlayPaperAnimation(false)
            );
        }
        else
        {
            yield return StartCoroutine(
                battleAnimator.PlayDefenseStance(false, enemyDefense)
            );
        }

        yield return new WaitForSeconds(0.5f);
    }

    // --------------------------------
    // PLAYER BLOCK DEFENSE
    // --------------------------------

    /// <summary>
    /// Handles player block for this component.
    /// </summary>
    public void PlayerBlock()
    {
        if (actionInProgress || battleDisplay.IsDialogueActive)
            return;

        StartCoroutine(PlayerBlockSequence());
    }

    /// <summary>
    /// Handles player block sequence for this component.
    /// </summary>
    private IEnumerator PlayerBlockSequence()
    {
        actionInProgress = true;
        HideCombatButtons();

        int attackChoice = enemyAI.ChooseAttack();

        // Enemy attack sound plays when its attack is revealed.
        if (AudioManager.Instance != null)
        {
            if (attackChoice == 0)
                AudioManager.Instance.PlayRock();
            else
                AudioManager.Instance.PlayScissors();
        }

        battleDisplay.ShowInstantMessage(
            attackChoice == 0
                ? "Enemy used Rock!"
                : "Enemy used Scissors!"
        );

        yield return StartCoroutine(
            battleAnimator.PlayAttackSymbol(false, attackChoice)
        );

        yield return new WaitForSeconds(0.5f);

        battleDisplay.ShowInstantMessage("You used Block!");

        yield return StartCoroutine(
            battleAnimator.PlayDefenseStance(true, 0)
        );

        yield return new WaitForSeconds(0.5f);

        int previousHealth = player.currentHealth;

        if (attackChoice == 0)
            attackSystem.RockAttack(enemy, player, true);
        else
            attackSystem.ScissorsAttack(enemy, player, false);

        string resultMessage;

        if (attackChoice == 0)
        {
            resultMessage = "You blocked the enemy's Rock!";
        }
        else
        {
            int damageTaken =
                previousHealth - player.currentHealth;

            resultMessage = damageTaken > 0
                ? "Block failed! You took " +
                  damageTaken + " damage!"
                : "Block failed, but Shield absorbed the attack!";
        }

        yield return StartCoroutine(
            battleDisplay.ShowMessageAndWait(resultMessage)
        );

        actionInProgress = false;
        FinishDefense();
    }

    // --------------------------------
    // PLAYER COUNTER DEFENSE
    // --------------------------------

    /// <summary>
    /// Handles player counter for this component.
    /// </summary>
    public void PlayerCounter()
    {
        if (actionInProgress || battleDisplay.IsDialogueActive)
            return;

        StartCoroutine(PlayerCounterSequence());
    }

    /// <summary>
    /// Handles player counter sequence for this component.
    /// </summary>
    private IEnumerator PlayerCounterSequence()
    {
        actionInProgress = true;
        HideCombatButtons();

        int attackChoice = enemyAI.ChooseAttack();

        // Enemy attack sound plays when its attack is revealed.
        if (AudioManager.Instance != null)
        {
            if (attackChoice == 0)
                AudioManager.Instance.PlayRock();
            else
                AudioManager.Instance.PlayScissors();
        }

        battleDisplay.ShowInstantMessage(
            attackChoice == 0
                ? "Enemy used Rock!"
                : "Enemy used Scissors!"
        );

        yield return StartCoroutine(
            battleAnimator.PlayAttackSymbol(false, attackChoice)
        );

        yield return new WaitForSeconds(0.5f);

        battleDisplay.ShowInstantMessage("You used Counter!");

        yield return StartCoroutine(
            battleAnimator.PlayDefenseStance(true, 1)
        );

        yield return new WaitForSeconds(0.5f);

        int previousPlayerHealth = player.currentHealth;
        int previousEnemyHealth = enemy.currentHealth;

        if (attackChoice == 0)
            attackSystem.RockAttack(enemy, player, false);
        else
            attackSystem.ScissorsAttack(enemy, player, true);

        string resultMessage;

        if (attackChoice == 0)
        {
            int damageTaken =
                previousPlayerHealth - player.currentHealth;

            resultMessage =
                "Counter failed against Rock! You took " +
                damageTaken + " damage!";
        }
        else
        {
            int damageDealt =
                previousEnemyHealth - enemy.currentHealth;

            resultMessage =
                "Counter succeeded! Enemy took " +
                damageDealt + " damage!";
        }

        yield return StartCoroutine(
            battleDisplay.ShowMessageAndWait(resultMessage)
        );

        actionInProgress = false;
        FinishDefense();
    }

    // --------------------------------
    // PLAYER PAPER DEFENSE
    // --------------------------------

    /// <summary>
    /// Handles player defense paper for this component.
    /// </summary>
    public void PlayerDefensePaper()
    {
        if (actionInProgress || battleDisplay.IsDialogueActive)
            return;

        if (playerCards.paperCards <= 0)
            return;

        StartCoroutine(PlayerDefensePaperSequence());
    }

    /// <summary>
    /// Handles player defense paper sequence for this component.
    /// </summary>
    private IEnumerator PlayerDefensePaperSequence()
    {
        actionInProgress = true;
        HideCombatButtons();

        playerCards.paperCards--;

        int attackChoice = enemyAI.ChooseAttack();

        // Enemy attack sound plays when its attack is revealed.
        if (AudioManager.Instance != null)
        {
            if (attackChoice == 0)
                AudioManager.Instance.PlayRock();
            else
                AudioManager.Instance.PlayScissors();
        }

        battleDisplay.ShowInstantMessage(
            attackChoice == 0
                ? "Enemy used Rock!"
                : "Enemy used Scissors!"
        );

        yield return StartCoroutine(
            battleAnimator.PlayAttackSymbol(false, attackChoice)
        );

        yield return new WaitForSeconds(0.5f);

        battleDisplay.ShowInstantMessage("You used Paper!");

        // Play Paper when its defense animation starts, not at button click.
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayPaper();

        yield return StartCoroutine(
            battleAnimator.PlayPaperAnimation(true)
        );

        player.AddShield(player.paperShield);

        yield return new WaitForSeconds(0.5f);

        int previousHealth = player.currentHealth;

        if (attackChoice == 0)
            attackSystem.RockAttack(enemy, player, false);
        else
            attackSystem.ScissorsAttack(enemy, player, false);

        int damageTaken =
            previousHealth - player.currentHealth;

        string resultMessage = damageTaken > 0
            ? "You gained Shield, but took " +
              damageTaken + " damage!"
            : "Your Shield protected you!";

        yield return StartCoroutine(
            battleDisplay.ShowMessageAndWait(resultMessage)
        );

        actionInProgress = false;
        FinishDefense();
    }
}
