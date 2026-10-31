
using UnityEngine;

/// <summary>
/// Loops the menu background or decorative scrolling element.
/// </summary>
public class InfiniteMenuScroll : MonoBehaviour
{
    public RectTransform background1;
    public RectTransform background2;

    [Header("Scroll Settings")]
    public float scrollSpeed = 30f;

    private float imageWidth;

    void Start()
    {
        imageWidth = background1.rect.width;

        background1.anchoredPosition = Vector2.zero;
        background2.anchoredPosition =
            new Vector2(imageWidth, 0f);
    }

    void Update()
    {
        float movement = scrollSpeed * Time.unscaledDeltaTime;

        background1.anchoredPosition += Vector2.left * movement;
        background2.anchoredPosition += Vector2.left * movement;

        if (background1.anchoredPosition.x <= -imageWidth)
        {
            background1.anchoredPosition =
                background2.anchoredPosition +
                new Vector2(imageWidth, 0f);
        }

        if (background2.anchoredPosition.x <= -imageWidth)
        {
            background2.anchoredPosition =
                background1.anchoredPosition +
                new Vector2(imageWidth, 0f);
        }
    }
}
