using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class MenuUI : MonoBehaviour
{
    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;
    public GameObject quitMenuHolder;
    public Image fadePlane;
    public Image fadePlane2;

    public float maxVolumeLevel = 0f;
    public float minVolumeLevel = -40f;
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;
    public Dropdown resolutionDropdown;
    public Toggle fulscreenToggle;

    public AudioMixer audioMixer;

    void Start()
    {
        mainMenuHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
        StartCoroutine(StartIE());
        InitializeResolution();
        InitializeAudio();
    }
    void InitializeResolution()
    {
        int currentWidth = PlayerPrefs.GetInt("ResolutionWidth", Screen.currentResolution.width);
        int currentheight = PlayerPrefs.GetInt("ResolutionHeight", Screen.currentResolution.height);
        int currentRefreshRate = PlayerPrefs.GetInt("ResolutionRefreshRate", Screen.currentResolution.refreshRate);
        bool currentFullScreen = PlayerPrefs.GetInt("FullScreen", Screen.fullScreen ? 1 : 0) == 1;
        Resolution currentResolution = new Resolution()
            { 
                width = currentWidth, 
                height = currentheight, 
                refreshRate = currentRefreshRate
            };

        int currentResolutionIndex = 0;
        List<string> resolutionsToAdd = new List<string>();
        foreach (Resolution resolution in Screen.resolutions)
        {
            resolutionsToAdd.Add(resolution.ToString());
            if (resolution.Equals(currentResolution))
            {
                currentResolutionIndex = System.Array.IndexOf(Screen.resolutions, resolution);
            }
        }

        resolutionDropdown.AddOptions(resolutionsToAdd);
        resolutionDropdown.value = currentResolutionIndex;
        fulscreenToggle.isOn = currentFullScreen;
        SetFullScreen(currentFullScreen);
        SetScreenResolution(currentResolutionIndex);
    }
    void InitializeAudio()
    {
        AudioSource interactionAudioSource = GetComponent<AudioSource>();
        foreach (Button button in GetComponentsInChildren<Button>(includeInactive:true))
        {
            button.onClick.AddListener(delegate () { interactionAudioSource.Play(); });
        }

        masterVolumeSlider.maxValue = maxVolumeLevel;
        masterVolumeSlider.minValue = minVolumeLevel;
        sfxVolumeSlider.maxValue = maxVolumeLevel;
        sfxVolumeSlider.minValue = minVolumeLevel;
        musicVolumeSlider.maxValue = maxVolumeLevel;
        musicVolumeSlider.minValue = minVolumeLevel;

        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", maxVolumeLevel);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SfxVolume", maxVolumeLevel);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", maxVolumeLevel);

        SetMasterVolume();
        SetSFXVolume();
        SetMusicVolume();
    }
    public void Play()
    {
        StartCoroutine(PlayGame());
    }
    public void Quit()
    {
        StartCoroutine(QuitMenu(Color.clear,new Color(0,0,0,.75f)));
    }
    public void QuitYes()
    {
        StartCoroutine(QuitMenuYes(new Color(0, 0, 0, .75f), Color.black));
    }
    public void QuitNo()
    {
        StartCoroutine(QuitMenuNo(new Color(0, 0, 0, .75f),Color.clear));
    }
    public void OptionMenu()
    {
        StartCoroutine(SwitchMenu(0));
    }
    public void MainMenu()
    {
        StartCoroutine(SwitchMenu(1));
    }
    public void SetScreenResolution(int i)
    {
        int width = Screen.resolutions[i].width;
        int height = Screen.resolutions[i].height;
        int refreshRate = Screen.resolutions[i].refreshRate;
        Screen.SetResolution(width, height, Screen.fullScreen, refreshRate);
        PlayerPrefs.SetInt("ResolutionWidth", width);
        PlayerPrefs.SetInt("ResolutionHeight", height);
        PlayerPrefs.SetInt("ResolutionRefreshRate", refreshRate);
    }
    public void SetFullScreen(bool isFullscren)
    {
        Screen.fullScreen = isFullscren;
        PlayerPrefs.SetInt("FullScreen", isFullscren ? 1 : 0);
    }
    public void SetMasterVolume()
    {
        SetChannelVolume("MasterVolume", masterVolumeSlider.value);
    }
    public void SetSFXVolume()
    {
        SetChannelVolume("SfxVolume", sfxVolumeSlider.value);
    }
    public void SetMusicVolume()
    {
        SetChannelVolume("MusicVolume", musicVolumeSlider.value);
    }
    void SetChannelVolume(string channelName, float value)
    {
        audioMixer.SetFloat(channelName, value);
        PlayerPrefs.SetFloat(channelName, value);
    }
    IEnumerator StartIE()
    {
        fadePlane.gameObject.SetActive(true);
        fadePlane2.gameObject.SetActive(true);
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / 2;
            fadePlane.color = Color.Lerp(Color.black, Color.clear, percent);
            fadePlane2.color = Color.Lerp(Color.black, Color.clear, percent);
            yield return null;
        }
        fadePlane.gameObject.SetActive(false);
        fadePlane2.gameObject.SetActive(false);
    }
    IEnumerator PlayGame()
    {
        fadePlane.gameObject.SetActive(true);
        fadePlane2.gameObject.SetActive(true);
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / 2;
            fadePlane.color = Color.Lerp(Color.clear, Color.black, percent);
            fadePlane2.color = Color.Lerp(Color.clear, Color.black, percent);
            yield return null;
        }
        SceneManager.LoadScene(1);
    }
    IEnumerator SwitchMenu(int i)
    {
        fadePlane.gameObject.SetActive(true);
        float speed = 2;
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(Color.clear, Color.black, percent);
            yield return null;
        }
        if (i == 0)
        {
            mainMenuHolder.SetActive(false);
            optionsMenuHolder.SetActive(true);
        }
        else
        {
            mainMenuHolder.SetActive(true);
            optionsMenuHolder.SetActive(false);
            PlayerPrefs.Save();
        }
        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(Color.black, Color.clear, percent);
            yield return null;
        }
        fadePlane.gameObject.SetActive(false);
    }
    IEnumerator QuitMenu(Color startColor, Color endColor)
    {
        fadePlane.gameObject.SetActive(true);
        fadePlane2.gameObject.SetActive(true);
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime;
            fadePlane.color = Color.Lerp(startColor, endColor, percent);
            fadePlane2.color = Color.Lerp(startColor, endColor, percent);
            yield return null;
        }
        quitMenuHolder.SetActive(true);
    }
    IEnumerator QuitMenuNo(Color startColor, Color endColor)
    {
        quitMenuHolder.SetActive(false);
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime;
            fadePlane.color = Color.Lerp(startColor, endColor, percent);
            fadePlane2.color = Color.Lerp(startColor, endColor, percent);
            yield return null;
        }
        fadePlane.gameObject.SetActive(false);
        fadePlane2.gameObject.SetActive(false);
    }
    IEnumerator QuitMenuYes(Color startColor, Color endColor)
    {
        fadePlane.gameObject.SetActive(true);
        fadePlane2.gameObject.SetActive(true);
        quitMenuHolder.SetActive(false);
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / 2;
            fadePlane.color = Color.Lerp(startColor, endColor, percent);
            fadePlane2.color = Color.Lerp(startColor, endColor, percent);
            yield return null;
        }
        Application.Quit();
    }
}
