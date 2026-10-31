
using UnityEngine;

/// <summary>
/// Applies attack rules, damage, shields, counters, and other combat effects.
/// </summary>
public class AttackSystem : MonoBehaviour
{
    // Handles a Rock attack against a defender.
    /// <summary>
    /// Performs rock attack for this component.
    /// </summary>
    public void RockAttack(CombatStats attacker, CombatStats defender, bool blocked)
    {

        if (blocked)
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayBlock();

            Debug.Log("Rock attack was blocked! No damage taken.");
            return;
        }

        int damage = attacker.rockDamage;
        int healthDamage = defender.TakeDamage(damage);

        if (healthDamage > 0 && AudioManager.Instance != null)
            AudioManager.Instance.PlayHurt();

        Debug.Log("Rock attack dealt " + healthDamage + " HP damage!");
        Debug.Log("Defender HP: " + defender.currentHealth);
        Debug.Log("Defender Shield: " + defender.shield);
    }

    // Handles a Scissors attack against a defender.
    /// <summary>
    /// Performs scissors attack for this component.
    /// </summary>
    public void ScissorsAttack(CombatStats attacker, CombatStats defender, bool countered)
    {

        if (countered)
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayCounter();

            // Counter stops Scissors and damages the attacker.
            int counterDamage = 3;
            int counterHealthDamage = attacker.TakeDamage(counterDamage);

            if (counterHealthDamage > 0 && AudioManager.Instance != null)
                AudioManager.Instance.PlayHurt();

            Debug.Log("Scissors attack was countered!");
            Debug.Log("Counter dealt " + counterHealthDamage + " HP damage!");
            Debug.Log("Attacker HP: " + attacker.currentHealth);

            return;
        }

        int damage = attacker.scissorsDamage;
        int healthDamage = defender.TakeDamage(damage);

        if (healthDamage > 0 && AudioManager.Instance != null)
            AudioManager.Instance.PlayHurt();

        Debug.Log("Scissors dealt " + healthDamage + " HP damage!");

        // Bleed only applies if damage penetrates Shield.
        if (healthDamage > 0)
        {
            defender.ApplyBleed(attacker.scissorsBleed, 2);
            Debug.Log("Bleed applied!");
        }

        Debug.Log("Defender HP: " + defender.currentHealth);
        Debug.Log("Defender Shield: " + defender.shield);
    }
}
