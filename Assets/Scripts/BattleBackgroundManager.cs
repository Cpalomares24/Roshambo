
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates the battle background presentation.
/// </summary>
public class BattleBackgroundManager : MonoBehaviour
{
    [Header("References")]
    public Image battleBackground;
    public EncounterManager encounterManager;

    [Header("Encounter Backgrounds")]
    public Sprite hallBackground;
    public Sprite chamberBackground;
    public Sprite deepBackground;
    public Sprite altarBackground;

    private int lastEncounter = -1;

    void Start()
    {
        UpdateBackground();
    }

    void Update()
    {
        if (encounterManager == null || battleBackground == null)
            return;

        if (encounterManager.currentEncounter != lastEncounter)
        {
            UpdateBackground();
        }
    }

    void UpdateBackground()
    {
        if (encounterManager == null || battleBackground == null)
            return;

        int encounter = encounterManager.currentEncounter;

        Sprite selectedBackground;

        switch (encounter)
        {
            case 1:
                selectedBackground = hallBackground;
                break;

            case 2:
                selectedBackground = chamberBackground;
                break;

            case 3:
                selectedBackground = deepBackground;
                break;

            case 4:
                selectedBackground = altarBackground;
                break;

            default:
                selectedBackground = hallBackground;
                break;
        }

        if (selectedBackground != null)
        {
            battleBackground.sprite = selectedBackground;
            battleBackground.color = Color.white;
        }

        lastEncounter = encounter;
    }
}
