
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Animates the coin flip that determines the turn order.
/// </summary>
public class CoinFlipAnimator : MonoBehaviour
{
    [Header("UI References")]
    public GameObject coinFlipPanel;
    public RectTransform coinImage;
    public TMP_Text coinResultText;

    [Header("Coin Sprites")]
    public Sprite playerCoinSprite;
    public Sprite enemyCoinSprite;

    private Image coinImageComponent;

    void Awake()
    {
        coinImageComponent = coinImage.GetComponent<Image>();
    }

    /// <summary>
    /// Handles play coin flip for this component.
    /// </summary>
    public IEnumerator PlayCoinFlip(bool playerWon)
    {
        coinFlipPanel.SetActive(true);
        coinResultText.text = "FLIPPING...";

        // Play spinning sound when the coin starts.
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayCoinSpin();

        float duration = 1.5f;
        float elapsed = 0f;
        int previousSide = -1;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // Rotate the coin around its vertical axis.
            float angle = elapsed * 1080f;
            coinImage.localRotation =
                Quaternion.Euler(0f, angle, 0f);

            // Alternate between the player and enemy sprites.
            int currentSide = Mathf.FloorToInt(angle / 180f) % 2;

            if (currentSide != previousSide)
            {
                coinImageComponent.sprite = currentSide == 0
                    ? playerCoinSprite
                    : enemyCoinSprite;

                previousSide = currentSide;
            }

            yield return null;
        }

        // Stop the coin and show the winning side.
        coinImage.localRotation = Quaternion.identity;

        coinImageComponent.sprite = playerWon
            ? playerCoinSprite
            : enemyCoinSprite;

        coinResultText.text = playerWon
            ? "PLAYER GOES FIRST!"
            : "ENEMY GOES FIRST!";

        // Play landing sound when the result is revealed.
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayCoinLand();

        yield return new WaitForSeconds(1f);

        coinFlipPanel.SetActive(false);
    }
}
