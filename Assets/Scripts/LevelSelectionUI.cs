using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelSelectionUI : MonoBehaviour
{
    public Image fade;
    public Toggle[] levelButtons;
    public static int maxLevel = 10;
    void Start()
    {
        StartCoroutine(Fade(Color.black, Color.clear));
        for (int i = 0; i < levelButtons.Length; i++)
            if (i < maxLevel)
                levelButtons[i].interactable = true;
            else
                levelButtons[i].interactable = false;
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
}
