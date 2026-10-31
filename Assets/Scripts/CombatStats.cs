

using UnityEngine;

/// <summary>
/// Stores a combatant’s health and status information and processes end-of-turn effects.
/// </summary>
public class CombatStats : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 20;
    public int currentHealth = 20;

    [Header("Shield")]
    public int shield = 0;

    [Header("Bleed")]
    public int bleedDamage = 0;
    public int bleedTurnsRemaining = 0;

    [Header("Card Strength")]
    public int rockDamage = 5;
    public int scissorsDamage = 3;
    public int scissorsBleed = 1;
    public int paperShield = 3;

    // Resets health and status effects for a new battle.
    /// <summary>
    /// Performs initialize for this component.
    /// </summary>
    public void Initialize()
    {
        currentHealth = maxHealth;
        shield = 0;
        bleedDamage = 0;
        bleedTurnsRemaining = 0;
    }

    // Shield absorbs damage before health is reduced.
    /// <summary>
    /// Performs take damage for this component.
    /// </summary>
    public int TakeDamage(int damage)
    {
        int absorbed = Mathf.Min(shield, damage);
        shield -= absorbed;

        int remainingDamage = damage - absorbed;
        currentHealth = Mathf.Max(0, currentHealth - remainingDamage);

        return remainingDamage;
    }

    // Adds shield when Paper is played.
    /// <summary>
    /// Handles add shield for this component.
    /// </summary>
    public void AddShield(int amount)
    {
        shield += amount;
    }

    // Applies bleed after Scissors penetrates the shield.
    /// <summary>
    /// Handles apply bleed for this component.
    /// </summary>
    public void ApplyBleed(int amount, int duration)
    {
        bleedDamage = amount;
        bleedTurnsRemaining = duration;
    }

    // Called after both characters finish their attack and defense.
    /// <summary>
    /// Handles process end of turn for this component.
    /// </summary>
    public void ProcessEndOfTurn()
    {
        if (bleedTurnsRemaining > 0)
        {
            currentHealth = Mathf.Max(0, currentHealth - bleedDamage);
            bleedTurnsRemaining--;
            
            if (bleedTurnsRemaining == 0)
            {
                bleedDamage = 0;
            }
        }
    }

    /// <summary>
    /// Performs is defeated for this component.
    /// </summary>
    public bool IsDefeated()
    {
        return currentHealth <= 0;
    }
}

