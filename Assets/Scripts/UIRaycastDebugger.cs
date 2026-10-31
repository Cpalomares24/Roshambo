
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class UIRaycastDebugger : MonoBehaviour
{
    private List<RaycastResult> results =
        new List<RaycastResult>();

    void Update()
    {
        if (EventSystem.current == null ||
            Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            PointerEventData pointerData =
                new PointerEventData(EventSystem.current);

            pointerData.position =
                Mouse.current.position.ReadValue();

            results.Clear();

            EventSystem.current.RaycastAll(
                pointerData,
                results
            );

            Debug.Log("=== UI OBJECTS UNDER MOUSE ===");

            if (results.Count == 0)
            {
                Debug.Log("No UI objects detected.");
                return;
            }

            foreach (RaycastResult result in results)
            {
                Debug.Log(result.gameObject.name);
            }
        }
    }
}
