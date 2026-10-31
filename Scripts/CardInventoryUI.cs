using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Shows the card inventory in the interface.
/// </summary>
public class CardInventoryUI : MonoBehaviour
{
    [Header("Inventory")]
    public CardInventory inventory;

    [Header("Card Buttons")]
    public Button rockButton;
    public Button attackPaperButton;
    public Button scissorsButton;
    public Button defensePaperButton;

    [Header("Button Text")]
    public TMP_Text rockText;
    public TMP_Text attackPaperText;
    public TMP_Text scissorsText;
    public TMP_Text defensePaperText;

    void Update()
    {
        if (inventory == null)
            return;

        // Display remaining cards.
        rockText.text = "Rock\n (" + inventory.rockCards + ")";
        attackPaperText.text = "Paper\n (" + inventory.paperCards + ")";
        scissorsText.text = "Scissors\n (" + inventory.scissorsCards + ")";
        defensePaperText.text = "Paper\n (" + inventory.paperCards + ")";

        // Disable buttons when no cards remain.
        rockButton.interactable = inventory.rockCards > 0;
        attackPaperButton.interactable = inventory.paperCards > 0;
        scissorsButton.interactable = inventory.scissorsCards > 0;
        defensePaperButton.interactable = inventory.paperCards > 0;
    }
}

