using UnityEngine.UI;
using UnityEngine;
using System.Collections;

/// <summary>
/// Runs the visual animations used during combat actions.
/// </summary>
public class BattleAnimator : MonoBehaviour
{
    [Header("Character Images")]
    public RectTransform playerImage;
    public RectTransform enemyImage;

    [Header("Attack Symbols")]
    public GameObject playerAttackSymbol;
    public GameObject enemyAttackSymbol;
    [Header("Attack Sprite Images")]
    public Sprite rockSprite;
    public Sprite paperSprite;
    public Sprite scissorsSprite;

    [Header("Animation Settings")]
    public float moveDistance = 70f;
    public float moveDuration = 0.2f;
    /// <summary>
    /// Handles player attack animation for this component.
    /// </summary>
    public IEnumerator PlayerAttackAnimation()
    {
        playerAttackSymbol.SetActive(true);

        yield return MoveForwardAndBack(
            playerImage,
            Vector2.right
        );

        playerAttackSymbol.SetActive(false);
    }
    /// <summary>
    /// Performs enemy attack animation for this component.
    /// </summary>
    public IEnumerator EnemyAttackAnimation()
    {
        enemyAttackSymbol.SetActive(true);

        yield return MoveForwardAndBack(
            enemyImage,
            Vector2.left
        );

        enemyAttackSymbol.SetActive(false);
    }

    /// <summary>
    /// Handles player defense animation for this component.
    /// </summary>
    public IEnumerator PlayerDefenseAnimation()
    {
        yield return MoveForwardAndBack(
            playerImage,
            Vector2.left
        );
    }

    /// <summary>
    /// Performs enemy defense animation for this component.
    /// </summary>
    public IEnumerator EnemyDefenseAnimation()
    {
        yield return MoveForwardAndBack(
            enemyImage,
            Vector2.right
        );
    }

    /// <summary>
    /// Performs move forward and back for this component.
    /// </summary>
    private IEnumerator MoveForwardAndBack(
        RectTransform character,
        Vector2 direction
    )
    {
        Vector2 startPosition = character.anchoredPosition;
        Vector2 targetPosition =
            startPosition + direction * moveDistance;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(
                elapsed / moveDuration
            );

            character.anchoredPosition =
                Vector2.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(
                elapsed / moveDuration
            );

            character.anchoredPosition =
                Vector2.Lerp(targetPosition, startPosition, t);

            yield return null;
        }

        character.anchoredPosition = startPosition;
    }

    /// <summary>
    /// Handles play attack symbol for this component.
    /// </summary>
    public IEnumerator PlayAttackSymbol(bool isPlayer, int attackType)
    {
        GameObject symbol = isPlayer
            ? playerAttackSymbol
            : enemyAttackSymbol;

        Image symbolImage = symbol.GetComponent<Image>();

        switch (attackType)
        {
            case 0:
                symbolImage.sprite = rockSprite;
                break;

            case 1:
                symbolImage.sprite = scissorsSprite;
                break;

            case 2:
                symbolImage.sprite = paperSprite;
                break;
        }

        symbolImage.preserveAspect = true;

        symbol.SetActive(true);

        yield return isPlayer
            ? PlayerAttackMovement()
            : EnemyAttackMovement();

        symbol.SetActive(false);
    }

    /// <summary>
    /// Handles player attack movement for this component.
    /// </summary>
    private IEnumerator PlayerAttackMovement()
    {
        yield return MoveForwardAndBack(playerImage, Vector2.right);
    }

    /// <summary>
    /// Performs enemy attack movement for this component.
    /// </summary>
    private IEnumerator EnemyAttackMovement()
    {
        yield return MoveForwardAndBack(enemyImage, Vector2.left);
    }

    [ContextMenu("Test Player Attack")]
    /// <summary>
    /// Performs test player attack for this component.
    /// </summary>
    public void TestPlayerAttack()
    {
        StartCoroutine(PlayerAttackAnimation());
    }

    [ContextMenu("Test Enemy Attack")]
    /// <summary>
    /// Performs test enemy attack for this component.
    /// </summary>
    public void TestEnemyAttack()
    {
        StartCoroutine(EnemyAttackAnimation());
    }

    [ContextMenu("Test Player Defense")]
    /// <summary>
    /// Performs test player defense for this component.
    /// </summary>
    public void TestPlayerDefense()
    {
        StartCoroutine(PlayerDefenseAnimation());
    }

    [ContextMenu("Test Enemy Defense")]
    /// <summary>
    /// Performs test enemy defense for this component.
    /// </summary>
    public void TestEnemyDefense()
    {
        StartCoroutine(EnemyDefenseAnimation());
    }


public IEnumerator PlayDefenseStance(bool isPlayer, int defenseType)
{
    RectTransform character = isPlayer
        ? playerImage
        : enemyImage;

    Vector2 originalPosition = character.anchoredPosition;
    Vector3 originalScale = character.localScale;
    Quaternion originalRotation = character.localRotation;

    float direction = isPlayer ? 1f : -1f;

    Vector2 targetPosition = originalPosition;
    Vector3 targetScale = originalScale;
    Quaternion targetRotation = originalRotation;

    switch (defenseType)
    {
        case 0: // Block: brace forward.
            targetPosition += new Vector2(direction * 20f, 0f);
            targetRotation = originalRotation *
                Quaternion.Euler(0f, 0f, -direction * 15f);
            break;

        case 1: // Counter: lean backward.
            targetPosition += new Vector2(-direction * 45f, 0f);
            targetRotation = originalRotation *
                Quaternion.Euler(0f, 0f, direction * 12f);
            break;

        case 2: // Paper: crouch and widen.
            targetPosition += new Vector2(0f, -15f);
            targetScale = new Vector3(
                originalScale.x * 1.25f,
                originalScale.y * 0.75f,
                originalScale.z
            );
            break;
    }

    // Move into the defensive stance.
    float elapsed = 0f;
    float duration = 0.2f;

    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);

        character.anchoredPosition =
            Vector2.Lerp(originalPosition, targetPosition, t);

        character.localScale =
            Vector3.Lerp(originalScale, targetScale, t);

        character.localRotation =
            Quaternion.Slerp(originalRotation, targetRotation, t);

        yield return null;
    }

    // Hold the stance briefly.
    yield return new WaitForSeconds(
        defenseType == 1 ? 0.15f : 0.4f
    );

    if (defenseType == 1)
    {
        // COUNTER: fast forward lunge.
        Vector2 lungePosition =
            originalPosition + new Vector2(direction * 110f, 0f);

        float lungeDuration = 0.08f;
        elapsed = 0f;

        // Launch forward from the leaned-back position.
        while (elapsed < lungeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / lungeDuration);

            character.anchoredPosition =
                Vector2.Lerp(targetPosition, lungePosition, t);

            character.localRotation =
                Quaternion.Slerp(
                    targetRotation,
                    originalRotation,
                    t
                );

            yield return null;
        }

        // Brief impact pause.
        yield return new WaitForSeconds(0.08f);

        // Return to original position.
        elapsed = 0f;

        while (elapsed < 0.15f)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / 0.15f);

            character.anchoredPosition =
                Vector2.Lerp(lungePosition, originalPosition, t);

            yield return null;
        }
    }
    else
    {
        // Normal return animation for Block and Paper.
        elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            character.anchoredPosition =
                Vector2.Lerp(targetPosition, originalPosition, t);

            character.localScale =
                Vector3.Lerp(targetScale, originalScale, t);

            character.localRotation =
                Quaternion.Slerp(targetRotation, originalRotation, t);

            yield return null;
        }
    }

    // Restore original transform.
    character.anchoredPosition = originalPosition;
    character.localScale = originalScale;
    character.localRotation = originalRotation;
}


    /// <summary>
    /// Handles play paper animation for this component.
    /// </summary>
    public IEnumerator PlayPaperAnimation(bool isPlayer)
    {
        RectTransform character = isPlayer
            ? playerImage
            :enemyImage;

        GameObject symbol = isPlayer
            ? playerAttackSymbol
            : enemyAttackSymbol;

        Image characterImage = character.GetComponent<Image>();
        Image symbolImage = symbol.GetComponent<Image>();

        Color originalColor = characterImage.color;

        // Display the Paper symbol.
        symbolImage.sprite = paperSprite;
        symbolImage.preserveAspect = true;
        symbol.SetActive(true);

        // Flash blue three times.
        for (int i = 0; i < 3; i++)
        {
            characterImage.color = new Color(0.3f, 0.7f, 1f);
            yield return new WaitForSeconds(0.12f);

            characterImage.color = originalColor;
            yield return new WaitForSeconds(0.12f);
        }

        // Restore the character and hide the symbol.
        characterImage.color = originalColor;
        symbol.SetActive(false);
    }

}