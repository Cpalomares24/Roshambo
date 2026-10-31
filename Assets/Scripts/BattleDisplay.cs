
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// Displays combat messages and other battle information.
/// </summary>
public class BattleDisplay : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text battleMessageText;
    public Button nextMessageButton;
    public TMP_Text continueIndicator;

    [Header("Combat Reference")]
    public CombatManager combatManager;

    [Header("Continue Animation")]
    public float pulseSpeed = 3f;
    public float minimumAlpha = 0.4f;

    private bool waitingForContinue = false;

    public bool IsDialogueActive => waitingForContinue;

    void Start()
    {
        // Hide the Continue indicator at the beginning.
        if (continueIndicator != null)
        {
            continueIndicator.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Only animate while waiting for the player.
        if (!waitingForContinue || continueIndicator == null)
            return;

        // Smoothly fade the indicator in and out.
        float pulse = (Mathf.Sin(Time.unscaledTime * pulseSpeed) + 1f) / 2f;

        float alpha = Mathf.Lerp(
            minimumAlpha,
            1f,
            pulse
        );

        Color color = continueIndicator.color;
        color.a = alpha;
        continueIndicator.color = color;
    }

    /// <summary>
    /// Handles show instant message for this component.
    /// </summary>
    public void ShowInstantMessage(string message)
    {
        battleMessageText.text = message;

        waitingForContinue = false;

        nextMessageButton.gameObject.SetActive(false);

        if (continueIndicator != null)
        {
            continueIndicator.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Handles show message and wait for this component.
    /// </summary>
    public IEnumerator ShowMessageAndWait(string message)
    {
        battleMessageText.text = message;

        waitingForContinue = true;

        nextMessageButton.gameObject.SetActive(true);

        if (continueIndicator != null)
        {
            continueIndicator.gameObject.SetActive(true);
        }

        // Wait until the player clicks Continue.
        while (waitingForContinue)
        {
            yield return null;
        }
    }

    /// <summary>
    /// Performs next message for this component.
    /// </summary>
    public void NextMessage()
    {
        if (!waitingForContinue)
            return;

        waitingForContinue = false;

        nextMessageButton.gameObject.SetActive(false);

        if (continueIndicator != null)
        {
            continueIndicator.gameObject.SetActive(false);
        }
    }

    // Keeps existing StartTurn() calls working.
    /// <summary>
    /// Handles show message for this component.
    /// </summary>
    public void ShowMessage(string message)
    {
        ShowInstantMessage(message);
    }
}

