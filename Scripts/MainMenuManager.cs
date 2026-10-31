
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the main menu actions and navigation.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject howToPlayPanel;

    void Start()
    {
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Handles start game for this component.
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("BattleScene");
    }

    /// <summary>
    /// Handles show how to play for this component.
    /// </summary>
    public void ShowHowToPlay()
    {
        howToPlayPanel.SetActive(true);
    }

    /// <summary>
    /// Handles hide how to play for this component.
    /// </summary>
    public void HideHowToPlay()
    {
        howToPlayPanel.SetActive(false);
    }

    /// <summary>
    /// Performs quit game for this component.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting Roshambo...");
        Application.Quit();
    }
}

