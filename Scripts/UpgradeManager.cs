using UnityEngine;

/// <summary>
/// Manages upgrades offered during the run.
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    [Header("Player")]
    public CombatStats player;

    /// <summary>
    /// Performs upgrade rock for this component.
    /// </summary>
    public void UpgradeRock()
    {
        player.rockDamage += 2;

        Debug.Log("Rock upgraded! Damage: " + player.rockDamage);
    }

    /// <summary>
    /// Performs upgrade scissors for this component.
    /// </summary>
    public void UpgradeScissors()
    {
        player.scissorsDamage += 1;

        Debug.Log("Scissors upgraded! Damage: " + player.scissorsDamage);
    }

    /// <summary>
    /// Performs upgrade bleed for this component.
    /// </summary>
    public void UpgradeBleed()
    {
        player.scissorsBleed += 1;

        Debug.Log("Bleed upgraded! Damage: " + player.scissorsBleed);
    }

    /// <summary>
    /// Performs upgrade paper for this component.
    /// </summary>
    public void UpgradePaper()
    {
        player.paperShield += 2;

        Debug.Log("Paper upgraded! Shield: " + player.paperShield);
    }


    [ContextMenu("Test Rock Upgrade")]
    /// <summary>
    /// Performs test rock upgrade for this component.
    /// </summary>
    public void TestRockUpgrade()
    {
        UpgradeRock();
    }

    [ContextMenu("Test Scissors Upgrade")]
    /// <summary>
    /// Performs test scissors upgrade for this component.
    /// </summary>
    public void TestScissorsUpgrade()
    {
        UpgradeScissors();
    }

    [ContextMenu("Test Bleed Upgrade")]
    /// <summary>
    /// Performs test bleed upgrade for this component.
    /// </summary>
    public void TestBleedUpgrade()
    {
        UpgradeBleed();
    }

    [ContextMenu("Test Paper Upgrade")]
    /// <summary>
    /// Performs test paper upgrade for this component.
    /// </summary>
    public void TestPaperUpgrade()
    {
        UpgradePaper();
    }


    [Header("Upgrade UI")]
    public GameObject upgradePanel;

    [Header("Combat Reference")]
    public CombatManager combatManager;
    /// <summary>
    /// Handles show upgrades for this component.
    /// </summary>
    public void ShowUpgrades()
    {
        upgradePanel.SetActive(true);

        combatManager.combatButtons.SetActive(false);
        combatManager.defenseButtons.SetActive(false);

        Debug.Log("Choose an upgrade!");
    }
    /// <summary>
    /// Performs select rock upgrade for this component.
    /// </summary>
    public void SelectRockUpgrade()
    {
        UpgradeRock();
        FinishUpgrade();
    }
    /// <summary>
    /// Performs select scissors upgrade for this component.
    /// </summary>
    public void SelectScissorsUpgrade()
    {
        UpgradeScissors();
        FinishUpgrade();
    }
    /// <summary>
    /// Performs select bleed upgrade for this component.
    /// </summary>
    public void SelectBleedUpgrade()
    {
        UpgradeBleed();
        FinishUpgrade();
    }
    /// <summary>
    /// Performs select paper upgrade for this component.
    /// </summary>
    public void SelectPaperUpgrade()
    {
        UpgradePaper();
        FinishUpgrade();
    }
    /// <summary>
    /// Handles finish upgrade for this component.
    /// </summary>
    private void FinishUpgrade()
    {
        upgradePanel.SetActive(false);
        combatManager.BeginNextEncounter();
    }
}
