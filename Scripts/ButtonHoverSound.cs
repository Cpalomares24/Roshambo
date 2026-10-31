using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Plays feedback when the pointer enters an interactive button.
/// </summary>
public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    /// <summary>
    /// Handles on pointer enter for this component.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayHover();
    }
}
