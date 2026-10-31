
using UnityEngine;

/// <summary>
/// Responds to enemy damage and defeat with visual and audio feedback.
/// </summary>
public class EnemyDamageAnimation : MonoBehaviour
{
    [Header("Enemy References")]
    public CombatStats enemy;
    public Animator enemyAnimator;
    public EncounterManager encounterManager;

    [Header("Animation Triggers")]
    public string hurtTriggerName = "Hurt";
    public string deathTriggerName = "Die";

    private int previousHealth;
    private bool initialized;
    private bool deathPlayed;

    void Start()
    {
        ResetHealthTracking();
    }

    void Update()
    {
        if (enemy == null || enemyAnimator == null)
            return;

        if (!initialized)
        {
            ResetHealthTracking();
            return;
        }

        if (enemy.currentHealth <= 0)
        {
            if (!deathPlayed)
            {
                enemyAnimator.SetTrigger(deathTriggerName);

                // Play the correct enemy death sound.
                if (AudioManager.Instance != null &&
                    encounterManager != null)
                {
                    AudioManager.Instance.PlayEnemyDeath(
                        encounterManager.currentEncounter
                    );
                }

                deathPlayed = true;
            }
        }
        else if (enemy.currentHealth < previousHealth)
        {
            enemyAnimator.SetTrigger(hurtTriggerName);
        }

        previousHealth = enemy.currentHealth;
    }

    /// <summary>
    /// Handles reset health tracking for this component.
    /// </summary>
    public void ResetHealthTracking()
    {
        if (enemy == null)
            return;

        previousHealth = enemy.currentHealth;
        initialized = true;
        deathPlayed = false;
    }
}
