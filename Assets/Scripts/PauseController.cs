using System.Collections;
using Michsky.LSS;
using Michsky.UI.Reach;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public GameObject pauseMenuUI;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject quitGameMenu;
    public GameObject musicMenu;
    public GameObject changeMusicButton;
    public LSS_Manager lSS_Manager;
    private int currentTrack = 0;
    public AudioSource audioSource;
    public AudioClip[] musicTracks;
    public bool enabledMusicChange;
    public float fadeDuration = 0.2f;
    public SliderManager masterVolumeSlider;
    private bool isPaused = false;
    private byte currentPanel = 0;

    private const byte PANEL_MAIN = 0;
    private const byte PANEL_MUSIC = 1;
    private const byte PANEL_OPTIONS = 2;
    private const byte PANEL_QUIT_GAME = 3;

    private AudioSource[] allAudioSources;

    /// <summary>
    ///
    /// </summary>
    void Start()
    {
        allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        float volume = PlayerPrefs.GetFloat(SparkEnum.GAME_OPT_MASTER_VOLUME, 1f);
        masterVolumeSlider.mainSlider.value = volume;
        ApplyVolume(volume);
        masterVolumeSlider.onValueChanged.AddListener(ApplyVolume);

        currentPanel = PANEL_MAIN;
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (enabledMusicChange)
            changeMusicButton.SetActive(true);
        else
            changeMusicButton.SetActive(false);
    }

    /// <summary>
    ///
    /// /// </summary>
    /// <param name="volume"></param>
    public void ApplyVolume(float volume)
    {
        foreach (var source in allAudioSources)
        {
            if (source != null)
                source.volume = volume;
        }

        PlayerPrefs.SetFloat(SparkEnum.GAME_OPT_MASTER_VOLUME, volume);
    }

    /// <summary>
    ///
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                if (currentPanel == PANEL_MAIN)
                    Resume();
                else
                {
                    if (currentPanel == PANEL_MUSIC)
                        closeMusicMenu();
                    else if (currentPanel == PANEL_OPTIONS)
                        CloseOptionsModal();
                    else if (currentPanel == PANEL_QUIT_GAME)
                        CloseQuitGameModal();
                }
            }
            else
                Pause();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public void FadeIn()
    {
        // gameObject.SetActive(true);
        StartCoroutine(FadeCanvas(0f, 1f));
    }

    /// <summary>
    ///
    /// </summary>
    public void FadeOut()
    {
        StartCoroutine(FadeCanvas(1f, 0f, disableAfter: true));
    }

    /// <summary>
    ///
    /// </summary>
    public void NextTrack()
    {
        if (musicTracks.Length == 0)
            return;

        currentTrack = (currentTrack + 1) % musicTracks.Length; // Cycle vers le début
        PlayCurrentTrack();
    }

    /// <summary>
    ///
    /// </summary>
    public void PreviousTrack()
    {
        if (musicTracks.Length == 0)
            return;

        currentTrack--;
        if (currentTrack < 0)
            currentTrack = musicTracks.Length - 1;

        PlayCurrentTrack();
    }

    /// <summary>
    ///
    /// </summary>
    private void PlayCurrentTrack()
    {
        audioSource.clip = musicTracks[currentTrack];
        audioSource.Play();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="index"></param>
    public void PlayTrack(int index)
    {
        if (index < 0 || index >= musicTracks.Length)
            return;

        currentTrack = index;
        PlayCurrentTrack();
    }

    /// <summary>
    ///
    /// </summary>
    public void Resume()
    {
        FadeOut();
    }

    /// <summary>
    ///
    /// /// </summary>
    void Pause()
    {
        currentPanel = PANEL_MAIN;
        FadeIn();
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    /// <summary>
    ///
    /// /// </summary>
    public void BackToMenu()
    {
        lSS_Manager.LoadScene("MenuScene");
    }

    /// <summary>
    ///
    /// /// </summary>
    public void OpenMusicMenu()
    {
        if (!enabledMusicChange)
            return;
        musicMenu.SetActive(true);
        currentPanel = PANEL_MUSIC;
    }

    /// <summary>
    ///
    /// </summary>
    public void closeMusicMenu()
    {
        musicMenu.SetActive(false);
        currentPanel = PANEL_MAIN;
    }

    /// <summary>
    ///
    /// /// </summary>
    public void OpenOptionsModal()
    {
        optionsMenu.SetActive(true);
        currentPanel = PANEL_OPTIONS;
    }

    /// <summary>
    ///
    /// </summary>
    public void CloseOptionsModal()
    {
        optionsMenu.SetActive(false);
        currentPanel = PANEL_MAIN;
    }

    /// <summary>
    ///
    /// /// </summary>
    public void OpenQuitGameModal()
    {
        quitGameMenu.SetActive(true);
        currentPanel = PANEL_QUIT_GAME;
    }

    /// <summary>
    ///
    /// /// </summary>
    public void CloseQuitGameModal()
    {
        quitGameMenu.SetActive(false);
        currentPanel = PANEL_MAIN;
    }

    /// <summary>
    ///
    /// /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    ///
    /// /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="disableAfter"></param>
    /// <returns></returns>
    private IEnumerator FadeCanvas(float start, float end, bool disableAfter = false)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime; // Time.unscaledDeltaTime pour ignorer Time.timeScale = 0
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = end;

        if (disableAfter)
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }
}
