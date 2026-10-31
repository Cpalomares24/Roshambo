
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles visual feedback when the player hovers over cards.
/// </summary>
public class CardHover : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [Header("Hover Settings")]
    public float hoverHeight = 40f;
    public float hoverScale = 1.1f;
    public float animationSpeed = 12f;

    private RectTransform rectTransform;

    private Vector2 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    private bool isHovered = false;
    private bool initialized = false;

    private int originalSiblingIndex;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        isHovered = false;
        initialized = false;
    }

    void Start()
    {
        Initialize();
    }

    /// <summary>
    /// Performs initialize for this component.
    /// </summary>
    private void Initialize()
    {
        if (initialized)
            return;

        originalPosition = rectTransform.anchoredPosition;
        originalRotation = rectTransform.localRotation;
        originalScale = rectTransform.localScale;
        originalSiblingIndex = rectTransform.GetSiblingIndex();

        initialized = true;
    }

    void Update()
    {
        if (!initialized)
            Initialize();

        Vector2 targetPosition = originalPosition;
        Quaternion targetRotation = originalRotation;
        Vector3 targetScale = originalScale;

        if (isHovered)
        {
            targetPosition += Vector2.up * hoverHeight;
            targetRotation = Quaternion.identity;
            targetScale = originalScale * hoverScale;
        }

        float t = 1f - Mathf.Exp(
            -animationSpeed * Time.deltaTime
        );

        rectTransform.anchoredPosition = Vector2.Lerp(
            rectTransform.anchoredPosition,
            targetPosition,
            t
        );

        rectTransform.localRotation = Quaternion.Slerp(
            rectTransform.localRotation,
            targetRotation,
            t
        );

        rectTransform.localScale = Vector3.Lerp(
            rectTransform.localScale,
            targetScale,
            t
        );
    }

    /// <summary>
    /// Handles on pointer enter for this component.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Initialize();

        isHovered = true;

        // Bring the hovered card in front.
        rectTransform.SetAsLastSibling();
    }

    /// <summary>
    /// Handles on pointer exit for this component.
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    void OnDisable()
    {
        if (rectTransform == null || !initialized)
            return;

        rectTransform.anchoredPosition = originalPosition;
        rectTransform.localRotation = originalRotation;
        rectTransform.localScale = originalScale;

        isHovered = false;
    }
}
