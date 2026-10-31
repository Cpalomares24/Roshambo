
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Updates combat interface elements to reflect game state.
/// </summary>
public class CombatUI : MonoBehaviour
{
    [Header("Combat Participants")]
    public CombatStats player;
    public CombatStats enemy;

    [Header("Player UI")]
    public TMP_Text playerHealthText;
    public TMP_Text playerShieldText;
    public Slider playerHealthBar;

    [Header("Enemy UI")]
    public TMP_Text enemyHealthText;
    public TMP_Text enemyShieldText;
    public Slider enemyHealthBar;

    void Update()
    {
        // Update player health text.
        playerHealthText.text =
            "HP: " + player.currentHealth + " / " + player.maxHealth;

        // Update player shield text.
        playerShieldText.text = "Shield: " + player.shield;

        // Update enemy health text.
        enemyHealthText.text =
            "HP: " + enemy.currentHealth + " / " + enemy.maxHealth;

        // Update enemy shield text.
        enemyShieldText.text = "Shield: " + enemy.shield;

        // Update player health bar.
        if (playerHealthBar != null)
        {
            playerHealthBar.minValue = 0;
            playerHealthBar.maxValue = Mathf.Max(1, player.maxHealth);
            playerHealthBar.value =
                Mathf.Clamp(player.currentHealth, 0, player.maxHealth);
        }

        // Update enemy health bar.
        if (enemyHealthBar != null)
        {
            enemyHealthBar.minValue = 0;
            enemyHealthBar.maxValue = Mathf.Max(1, enemy.maxHealth);
            enemyHealthBar.value =
                Mathf.Clamp(enemy.currentHealth, 0, enemy.maxHealth);
        }
    }
}

