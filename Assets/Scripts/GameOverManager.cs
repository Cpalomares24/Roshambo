
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Displays victory and defeat outcomes.
/// </summary>
public class GameOverManager : MonoBehaviour
{
    [Header("Game Over Panels")]
    public GameObject victoryPanel;
    public GameObject defeatPanel;

    void Start()
    {
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
    }

    /// <summary>
    /// Handles show victory for this component.
    /// </summary>
    public void ShowVictory()
    {
        if (victoryPanel.activeSelf)
            return;

        victoryPanel.SetActive(true);

        // Play victory sound when the player wins.
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayVictory();
    }

    /// <summary>
    /// Handles show defeat for this component.
    /// </summary>
    public void ShowDefeat()
    {
        if (defeatPanel.activeSelf)
            return;

        defeatPanel.SetActive(true);

        // Play defeat sound when the player loses.
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayGameOver();
    }

    /// <summary>
    /// Performs restart game for this component.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    /// <summary>
    /// Performs return to main menu for this component.
    /// </summary>
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
