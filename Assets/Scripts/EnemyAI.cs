
using UnityEngine;

/// <summary>
/// Chooses enemy actions based on the current combat state.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    [Header("Encounter Reference")]
    public EncounterManager encounterManager;

    // 0 = Rock, 1 = Scissors
    /// <summary>
    /// Handles choose attack for this component.
    /// </summary>
    public int ChooseAttack()
    {
        int encounter = encounterManager.currentEncounter;
        int choice;

        if (encounter == 2)
        {
            // Aggressive enemy: 75% Rock.
            choice = Random.value < 0.75f ? 0 : 1;
        }
        else if (encounter == 3)
        {
            // Defensive enemy: 75% Scissors.
            choice = Random.value < 0.75f ? 1 : 0;
        }
        else
        {
            // Enemy 1 and boss: balanced attacks.
            choice = Random.Range(0, 2);
        }

        Debug.Log(choice == 0
            ? "Enemy chose Rock!"
            : "Enemy chose Scissors!");

        return choice;
    }

    // 0 = Block, 1 = Counter, 2 = Paper
    // playerAttack: 0 = Rock, 1 = Scissors, 2 = Paper
    /// <summary>
    /// Handles choose defense for this component.
    /// </summary>
    public int ChooseDefense(int playerAttack)
    {
        int encounter = encounterManager.currentEncounter;
        int choice;

        if (encounter == 2)
        {
            // Aggressive enemy prefers Counter.
            choice = Random.value < 0.65f
                ? 1
                : Random.Range(0, 3);
        }
        else if (encounter == 3)
        {
            // Defensive enemy prefers Paper.
            choice = Random.value < 0.65f
                ? 2
                : Random.Range(0, 3);
        }
        else if (encounter == 4)
        {
            // Boss has a 60% chance to choose
            // the correct defense against Rock or Scissors.
            if (playerAttack == 0 && Random.value < 0.60f)
                choice = 0;
            else if (playerAttack == 1 && Random.value < 0.60f)
                choice = 1;
            else
                choice = Random.Range(0, 3);
        }
        else
        {
            choice = Random.Range(0, 3);
        }

        string[] defenses = { "Block", "Counter", "Paper" };

        Debug.Log("Enemy chose " + defenses[choice] + "!");

        return choice;
    }
}
