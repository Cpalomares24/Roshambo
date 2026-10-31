
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Selects the enemy sprite, animation controller, display name, size, and screen position.
/// </summary>
public class EnemyVisualManager : MonoBehaviour
{
    [Header("Enemy Display")]
    public Image enemyImage;
    public Animator enemyAnimator;
    public TMP_Text enemyNameText;

    [Header("Enemy Animator Controllers")]
    public RuntimeAnimatorController sporelingController;
    public RuntimeAnimatorController duskwingController;
    public RuntimeAnimatorController golemController;
    public RuntimeAnimatorController necromancerController;

    [Header("Enemy Starting Sprites")]
    public Sprite sporelingSprite;
    public Sprite duskwingSprite;
    public Sprite golemSprite;
    public Sprite necromancerSprite;

    [Header("Enemy Sizes")]
    public Vector3 sporelingScale = new Vector3(6f, 6f, 1f);
    public Vector3 duskwingScale = new Vector3(6f, 6f, 1f);
    public Vector3 golemScale = new Vector3(6f, 6f, 1f);
    public Vector3 necromancerScale = new Vector3(1f, 1f, 1f);

    [Header("Enemy Position")]
    public Vector2 necromancerOffset = new Vector2(0f, -50f);
    private Vector2 originalEnemyPosition;
    private bool positionSaved = false;

    /// <summary>
    /// Handles set enemy visual for this component.
    /// </summary>
    public void SetEnemyVisual(int encounterNumber)
    {
        RuntimeAnimatorController selectedController = null;
        Sprite selectedSprite = null;
        string selectedName = "";
        Vector3 selectedScale = Vector3.one;

        switch (encounterNumber)
        {
            case 1:
                selectedController = sporelingController;
                selectedSprite = sporelingSprite;
                selectedName = "Sporeling";
                selectedScale = sporelingScale;
                break;

            case 2:
                selectedController = duskwingController;
                selectedSprite = duskwingSprite;
                selectedName = "Duskwing";
                selectedScale = duskwingScale;
                break;

            case 3:
                selectedController = golemController;
                selectedSprite = golemSprite;
                selectedName = "Frostbound Golem";
                selectedScale = golemScale;
                break;

            case 4:
                selectedController = necromancerController;
                selectedSprite = necromancerSprite;
                selectedName = "Malachar the Necromancer";
                selectedScale = necromancerScale;
                break;

            default:
                Debug.LogWarning("Unknown encounter: " + encounterNumber);
                return;
        }

        if (enemyAnimator != null)
        {
            enemyAnimator.runtimeAnimatorController = selectedController;

            if (selectedController != null)
            {
                enemyAnimator.Rebind();
                enemyAnimator.Update(0f);
            }
        }

        if (enemyImage != null && selectedSprite != null)
        {
            RectTransform rect = enemyImage.rectTransform;

            if (!positionSaved)
            {
                originalEnemyPosition = rect.anchoredPosition;
                positionSaved = true;
            }

            enemyImage.sprite = selectedSprite;
            rect.localScale = selectedScale;

            // Keep the first three enemies at their original position.
            if (encounterNumber == 4)
                rect.anchoredPosition = originalEnemyPosition + necromancerOffset;
            else
                rect.anchoredPosition = originalEnemyPosition;
        }

        if (enemyNameText != null)
        {
            enemyNameText.text = selectedName;
        }

        Debug.Log("Enemy visual changed to: " + selectedName);
        
    }
}
