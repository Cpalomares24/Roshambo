using UnityEngine;

/// <summary>
/// Initializes and advances the enemy encounters in the run.
/// </summary>
public class EncounterManager : MonoBehaviour
{
    [Header("Encounter Progress")]
    public int currentEncounter = 1;
    public int totalEncounters = 4;

    [Header("Enemy Health")]
    public int[] enemyHealth = { 10, 15, 20, 30 };


    [Header("Enemy Damage Scaling")]
    public int[] enemyRockDamage = { 5, 6, 7, 9 };
    public int[] enemyScissorsDamage = { 3, 4, 5, 6 };


   [Header("References")]
    public CombatStats enemy;
    public EnemyVisualManager enemyVisualManager;
    public EnemyDamageAnimation enemyDamageAnimation;

    /// <summary>
    /// Performs is boss encounter for this component.
    /// </summary>
    public bool IsBossEncounter()
    {
        return currentEncounter == totalEncounters;
    }

    /// <summary>
    /// Performs has more encounters for this component.
    /// </summary>
    public bool HasMoreEncounters()
    {
        return currentEncounter < totalEncounters;
    }


    /// <summary>
    /// Handles start encounter for this component.
    /// </summary>
    public void StartEncounter()
    {
        int index = currentEncounter - 1;

        enemy.maxHealth = enemyHealth[index];
        enemy.Initialize();

        enemy.rockDamage = enemyRockDamage[index];
        enemy.scissorsDamage = enemyScissorsDamage[index];

        if (enemyVisualManager != null)
        {
            enemyVisualManager.SetEnemyVisual(currentEncounter);
        }

        if (enemyDamageAnimation != null)
        {
            enemyDamageAnimation.ResetHealthTracking();
        }

        Debug.Log("Encounter " + currentEncounter + " started!");
        
        Debug.Log("Enemy Rock Damage: " + enemy.rockDamage);
        Debug.Log("Enemy Scissors Damage: " + enemy.scissorsDamage);

        if (IsBossEncounter())
            Debug.Log("BOSS FIGHT!");
    }



    /// <summary>
    /// Performs advance encounter for this component.
    /// </summary>
    public bool AdvanceEncounter()
    {
        if (!HasMoreEncounters())
        {
            Debug.Log("GAME COMPLETE! Boss defeated!");
            return false;
        }

        currentEncounter++;
        StartEncounter();

        return true;
    }
}
