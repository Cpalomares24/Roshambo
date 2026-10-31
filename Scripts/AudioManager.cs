
using UnityEngine;

/// <summary>
/// Centralizes playback of gameplay, menu, and result sound effects.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Source")]
    public AudioSource sfxSource;

    [Header("Combat Sounds")]
    public AudioClip rockSound;
    public AudioClip paperSound;
    public AudioClip scissorsSound;
    public AudioClip blockSound;
    public AudioClip counterSound;
    public AudioClip hurtSound;

    [Header("Coin Flip Sounds")]
    public AudioClip coinSpinSound;
    public AudioClip coinLandSound;

    [Header("Enemy Death Sounds")]
    public AudioClip sporelingDeathSound;
    public AudioClip batDeathSound;
    public AudioClip golemDeathSound;
    public AudioClip necromancerDeathSound;

    [Header("UI Sounds")]
    public AudioClip hoverSound;

    [Header("Game Ending Sounds")]
    public AudioClip victorySound;
    public AudioClip gameOverSound;

    /// <summary>
    /// Performs awake for this component.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (sfxSource == null)
            sfxSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Handles play sound for this component.
    /// </summary>
    public void PlaySound(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Handles play rock for this component.
    /// </summary>
    public void PlayRock() => PlaySound(rockSound);
    /// <summary>
    /// Handles play paper for this component.
    /// </summary>
    public void PlayPaper() => PlaySound(paperSound);
    /// <summary>
    /// Handles play scissors for this component.
    /// </summary>
    public void PlayScissors() => PlaySound(scissorsSound);
    /// <summary>
    /// Handles play block for this component.
    /// </summary>
    public void PlayBlock() => PlaySound(blockSound);
    /// <summary>
    /// Handles play counter for this component.
    /// </summary>
    public void PlayCounter() => PlaySound(counterSound);
    /// <summary>
    /// Handles play hurt for this component.
    /// </summary>
    public void PlayHurt() => PlaySound(hurtSound);

    /// <summary>
    /// Handles play coin spin for this component.
    /// </summary>
    public void PlayCoinSpin() => PlaySound(coinSpinSound);
    /// <summary>
    /// Handles play coin land for this component.
    /// </summary>
    public void PlayCoinLand() => PlaySound(coinLandSound);

    /// <summary>
    /// Handles play enemy death for this component.
    /// </summary>
    public void PlayEnemyDeath(int encounter)
    {
        switch (encounter)
        {
            case 1: PlaySound(sporelingDeathSound); break;
            case 2: PlaySound(batDeathSound); break;
            case 3: PlaySound(golemDeathSound); break;
            case 4: PlaySound(necromancerDeathSound); break;
        }
    }

    /// <summary>
    /// Handles play hover for this component.
    /// </summary>
    public void PlayHover() => PlaySound(hoverSound);
    /// <summary>
    /// Handles play victory for this component.
    /// </summary>
    public void PlayVictory() => PlaySound(victorySound);
    /// <summary>
    /// Handles play game over for this component.
    /// </summary>
    public void PlayGameOver() => PlaySound(gameOverSound);
}
