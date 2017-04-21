using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuUI : MonoBehaviour
{
    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;
    public GameObject quitMenuHolder;
    public Image fadePlane;
    public Image fadePlane2;

    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;
    public Dropdown resolutionDropdown;
    public Toggle fulscreenToggle;

    void Start()
    {
        StartCoroutine(StartIE());
        fulscreenToggle.isOn = Screen.fullScreen;
        int currentResolutionIndex = 0;
        List<string> resolutionsToAdd = new List<string>();
        foreach (Resolution resolution in Screen.resolutions)
        {
            resolutionsToAdd.Add(resolution.width + "x" + resolution.height);
            if (resolution.height == Screen.height)
            {
                currentResolutionIndex = System.Array.IndexOf(Screen.resolutions, resolution);
            }
        }
        resolutionDropdown.AddOptions(resolutionsToAdd);
        resolutionDropdown.value = currentResolutionIndex;

        masterVolumeSlider.value = PlayerPrefs.GetFloat("master vol", .5f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfx vol", .5f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("music vol", .5f);
        PlayerPrefs.SetFloat("master vol", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("sfx vol", sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("music vol", musicVolumeSlider.value);
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
        Screen.SetResolution(Screen.resolutions[i].width, Screen.resolutions[i].height, Screen.fullScreen);
    }
    public void SetFullScreen(bool isFullscren)
    {
        Screen.fullScreen = isFullscren;
    }
    public void SetMasterVolume(float value)
    {
        //AudioMenager.instance.SetVolume(value, AudioMenager.AudioChannel.Master);
    }
    public void SetSFXVolume(float value)
    {
        //AudioMenager.instance.SetVolume(value, AudioMenager.AudioChannel.SFX);
    }
    public void SetMusicVolume(float value)
    {
        //AudioMenager.instance.SetVolume(value, AudioMenager.AudioChannel.Music);
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
