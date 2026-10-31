
using UnityEngine;
using System.Collections;

/// <summary>
/// Responds to player damage and defeat with animation feedback.
/// </summary>
public class PlayerDamageAnimation : MonoBehaviour
{
    [Header("References")]
    public CombatStats player;
    public Animator soldierAnimator;

    [Header("Animation State Names")]
    public string idleState = "Soldier_Idle";
    public string hurtState = "Soldier_Hurt";
    public string deathState = "Soldier_Death";

    private int previousHealth;
    private bool isDead;
    private Coroutine hurtCoroutine;

    void Start()
    {
        previousHealth = player.currentHealth;
    }

    void LateUpdate()
    {
        if (player == null || soldierAnimator == null || isDead)
            return;

        int currentHealth = player.currentHealth;

        if (currentHealth <= 0)
        {
            isDead = true;

            if (hurtCoroutine != null)
                StopCoroutine(hurtCoroutine);

            soldierAnimator.Play(deathState, 0, 0f);
        }
        else if (currentHealth < previousHealth)
        {
            if (hurtCoroutine != null)
                StopCoroutine(hurtCoroutine);

            hurtCoroutine = StartCoroutine(PlayHurt());
        }

        previousHealth = currentHealth;
    }

    /// <summary>
    /// Handles play hurt for this component.
    /// </summary>
    private IEnumerator PlayHurt()
    {
        soldierAnimator.Play(hurtState, 0, 0f);

        yield return null;

        AnimatorStateInfo state =
            soldierAnimator.GetCurrentAnimatorStateInfo(0);

        float duration = state.length;

        if (duration <= 0f)
            duration = 0.5f;

        yield return new WaitForSeconds(duration);

        if (!isDead)
            soldierAnimator.Play(idleState, 0, 0f);

        hurtCoroutine = null;
    }
}
