
using UnityEngine;

/// <summary>
/// Tracks the player’s available attack and defense cards.
/// </summary>
public class CardInventory : MonoBehaviour
{
    [Header("Starting Cards")]
    public int rockCards = 5;
    public int paperCards = 5;
    public int scissorsCards = 5;


    /// <summary>
    /// Performs has attack cards for this component.
    /// </summary>
    public bool HasAttackCards()
    {
        return rockCards > 0 ||
           paperCards > 0 ||
           scissorsCards > 0;
    }
}
