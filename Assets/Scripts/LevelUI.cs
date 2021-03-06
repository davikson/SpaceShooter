﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class LevelUI : MonoBehaviour
{
    public Text livesText;
    public Text scoreText;
    public Image fadePlane;
    public GameObject gameOverHandler;
    public GameObject levelCompletedHandler;
    public Text levelCompletedText;
    public Text gameOverScore;
    public Text levelCompletedScore;
    int score;
    Player player;
    bool gameOver = false;

    public AudioSource ambiendSound;
    public AudioSource looseSound;
    public AudioSource winSound;

    private void Start()
    {
        AudioSource interactionAudioSource = GetComponent<AudioSource>();
        foreach (Button button in GetComponentsInChildren<Button>(includeInactive: true))
        {
            button.onClick.AddListener(delegate () { interactionAudioSource.Play(); });
        }
    }
    void Awake()
    {
        StartCoroutine(StartFade());
        Enemy.resetOnDying();
        Enemy.OnDying += UpdateScore;
        Spawner.levelCompleted += LevelCompleted;
    }
    void OnDestroy()
    {
        Spawner.levelCompleted -= LevelCompleted;
    }
    void LateUpdate()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.OnHit += UpdateHealth;
                livesText.text = "Lives: " + player.health;
                scoreText.text = "Score: " + score.ToString("D4");
                player.OnDie += GameOver;
            }
        }
    }
    void GameOver()
    {
        StartCoroutine(GameOverIE());
        gameOverScore.text = "Score: " + score.ToString("D4");
        gameOver = true;
        ambiendSound.Stop();
        looseSound.Play();
    }
    void LevelCompleted()
    {
        if (gameOver) return;
        if (Spawner.currentLevel >= LevelSelectionUI.maxLevel - 1)
            LevelSelectionUI.maxLevel++;
        levelCompletedText.text = "Level " + (Spawner.currentLevel + 1) + " Completed";
        levelCompletedScore.text = "Score: " + score.ToString("D4");
        StartCoroutine(LevelCompletedIE());
        LevelSelectionUI.SaveMaxLevel();
        ambiendSound.Stop();
        winSound.Play();
    }
    public void RestartMission()
    {
        SceneManager.LoadScene(2);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void NextLevel()
    {
        Spawner.currentLevel++;
        SceneManager.LoadScene(2);
    }
    void UpdateScore()
    {
        score += 10;
        scoreText.text = "Score: " + score.ToString("D4");
    }
    void UpdateHealth()
    {
        livesText.text = "Health: " + player.health;
    }
    IEnumerator StartFade()
    {
        Time.timeScale = 0;
        fadePlane.gameObject.SetActive(true);
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.unscaledDeltaTime / 2;
            fadePlane.color = Color.Lerp(Color.black, Color.clear, percent);
            yield return null;
        }
        fadePlane.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    IEnumerator GameOverIE()
    {
        fadePlane.gameObject.SetActive(true);
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime;
            fadePlane.color = Color.Lerp(Color.clear, Color.black, percent);
            yield return null;
        }
        gameOverHandler.SetActive(true);
    }
    IEnumerator LevelCompletedIE()
    {
        fadePlane.gameObject.SetActive(true);
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime;
            fadePlane.color = Color.Lerp(Color.clear, Color.black, percent);
            yield return null;
        }
        levelCompletedHandler.SetActive(true);
    }
}
