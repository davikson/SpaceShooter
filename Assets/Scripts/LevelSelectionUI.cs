using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;

[RequireComponent(typeof(AudioSource))]
public class LevelSelectionUI : MonoBehaviour
{
    public Image fade;
    public Button[] levelButtons;
    public static int maxLevel = 1;
    void Start()
    {
        LoadMaxLevel();
        for (int i = 0; i < levelButtons.Length; i++)
            if (i < maxLevel)
                levelButtons[i].interactable = true;
            else
                levelButtons[i].interactable = false;
        StartCoroutine(Fade(Color.black, Color.clear));

        AudioSource interactionAudioSource = GetComponent<AudioSource>();
        foreach (Button button in GetComponentsInChildren<Button>(includeInactive: true))
        {
            button.onClick.AddListener(delegate () { interactionAudioSource.Play(); });
        }
    }
    public void BackToMenu()
    {
        StartCoroutine(Menu());
    }
    public void StartLevel(int i)
    {
        Spawner.currentLevel = i;
        StartCoroutine(Level());
    }
    IEnumerator Fade(Color startColor, Color endColor)
    {
        fade.gameObject.SetActive(true);
        float percent = 0;
        while(percent < 1)
        {
            percent += Time.deltaTime;
            fade.color = Color.Lerp(Color.black, Color.clear, percent);
            yield return null;
        }
        fade.gameObject.SetActive(false);
    }
    IEnumerator Menu()
    {
        fade.gameObject.SetActive(true);
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime;
            fade.color = Color.Lerp(Color.clear, Color.black, percent);
            yield return null;
        }
        SceneManager.LoadScene(0);
    }
    IEnumerator Level()
    {
        fade.gameObject.SetActive(true);
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime;
            fade.color = Color.Lerp(Color.clear, Color.black, percent);
            yield return null;
        }
        SceneManager.LoadScene(2);
    }
    public static void SaveMaxLevel()
    {
        int tmp = maxLevel;
        XmlSerializer xmls = new XmlSerializer(typeof(int));
        FileStream file = File.Open(Application.dataPath + "/ml", FileMode.OpenOrCreate);
        xmls.Serialize(file, tmp);
        file.Close();
    }
    public static void LoadMaxLevel()
    {
        if (File.Exists(Application.dataPath + "/ml"))
        {
            int tmp;
            XmlSerializer xmls = new XmlSerializer(typeof(int));
            FileStream file = File.Open(Application.dataPath + "/ml", FileMode.OpenOrCreate);
            tmp = (int)xmls.Deserialize(file);
            file.Close();
            maxLevel = tmp;
        }
        else
        {
            SaveMaxLevel();
        }
    }
}
