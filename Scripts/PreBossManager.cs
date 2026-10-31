using UnityEngine;

/// <summary>
/// Manages the choice presented before the final boss encounter.
/// </summary>
public class PreBossManager : MonoBehaviour
{
    [Header("References")]
    public CombatStats player;
    public CardInventory playerCards;
    public CombatManager combatManager;

    [Header("UI")]
    public GameObject preBossPanel;

    /// <summary>
    /// Handles show pre boss choice for this component.
    /// </summary>
    public void ShowPreBossChoice()
    {
        preBossPanel.SetActive(true);

        combatManager.combatButtons.SetActive(false);
        combatManager.defenseButtons.SetActive(false);

        Debug.Log("Choose your pre-boss reward!");
    }

    /// <summary>
    /// Handles choose heal for this component.
    /// </summary>
    public void ChooseHeal()
    {
        player.currentHealth = Mathf.Min(
            player.maxHealth,
            player.currentHealth + 8
        );

        Debug.Log("Player healed! HP: " + player.currentHealth);

        FinishChoice();
    }

    /// <summary>
    /// Handles choose replenish for this component.
    /// </summary>
    public void ChooseReplenish()
    {
        playerCards.rockCards += 2;
        playerCards.paperCards += 1;
        playerCards.scissorsCards += 1;

        Debug.Log("Replenished 4 cards!");

        FinishChoice();
    }

    /// <summary>
    /// Handles finish choice for this component.
    /// </summary>
    private void FinishChoice()
    {
        preBossPanel.SetActive(false);
        combatManager.BeginNextEncounter();
    }
}
