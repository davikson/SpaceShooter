using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;

public class Spawner : MonoBehaviour
{
    public List<LevelParameters> levels;
    public static int currentLevel;
    int enemiesToSpawn;
    float nextSpwanTime;
    public Enemy enemy;
    public Player player;
    public static event System.Action levelCompleted;
    void Start()
    {
        LoadLevelStats();
        //currentLevel = Mathf.Clamp(-1, levels.Count - 1, currentLevel);
        if (currentLevel > levels.Count - 1) currentLevel = levels.Count - 1;
        enemiesToSpawn = levels[currentLevel].enemiesCount;
        SpawnPlayer();
        SaveLevelsStats();
        nextSpwanTime = Time.time;
    }
    void Update()
    {
        if (Time.time > nextSpwanTime && enemiesToSpawn > 0)
            SpawnEnemy();
        else if(enemiesToSpawn <= 0 && FindObjectOfType<Enemy>() == null)
        {
            if (levelCompleted != null)
                levelCompleted();
            levelCompleted = null;
            Enemy.resetOnDying();
        }
    }
    void SpawnEnemy()
    {
        Enemy enemySpawned = Instantiate(enemy, new Vector3(Random.Range(19f, 23f), Random.Range(-8f, 8f)), Quaternion.Euler(0, -90, 0)) as Enemy;
        enemySpawned.startHealth = levels[currentLevel].eStartHealth;
        enemySpawned.fireRate = levels[currentLevel].eFireRate;
        enemySpawned.horizontalAcceleration = levels[currentLevel].eHorizontalAcceleration;
        enemySpawned.verticalAcceleration = levels[currentLevel].eVerticalAcceleration;
        enemiesToSpawn--;
        nextSpwanTime += Random.Range(levels[currentLevel].minSpwanDelay, levels[currentLevel].maxSpwanDelay);
    }
    void SpawnPlayer()
    {
        Player playerSpawned = Instantiate(player, new Vector3(-12, 0), Quaternion.Euler(0, 90, 0)) as Player;
        playerSpawned.startHealth = levels[currentLevel].pStartHealth;
        playerSpawned.fireRate = levels[currentLevel].pFireRate;
        playerSpawned.horizontalAcceleration = levels[currentLevel].pHorizontalAcceleration;
        playerSpawned.verticalAcceleration = levels[currentLevel].pVerticalAcceleration;
    }
    void SaveLevelsStats()
    {
        XmlSerializer xmls = new XmlSerializer(typeof(List<LevelParameters>));
        //BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.dataPath + "/level",FileMode.OpenOrCreate);
        //bf.Serialize(file, levels);
        xmls.Serialize(file, levels);
        file.Close();
    }
    void LoadLevelStats()
    {
        if (File.Exists(Application.dataPath + "/level"))
        {
            //BinaryFormatter bf = new BinaryFormatter();
            XmlSerializer xmls = new XmlSerializer(typeof(List<LevelParameters>));
            FileStream file = File.Open(Application.dataPath + "/level", FileMode.OpenOrCreate);
            //levels = (List<LevelParameters>)bf.Deserialize(file);
            levels = (List<LevelParameters>)xmls.Deserialize(file);
            file.Close();
        }
    }
}
[System.Serializable]
public class LevelParameters
{
    [Header("Level parameters")]
    public int enemiesCount = 10;
    public float minSpwanDelay = 5;
    public float maxSpwanDelay = 5;
    [Header("Enemies statistics")]
    public float eFireRate = 2;
    public float eStartHealth = 1;
    public float eHorizontalAcceleration = 4;
    public float eVerticalAcceleration = 3;
    [Header("Player statistics")]
    public float pFireRate = 2;
    public float pStartHealth = 5;
    public float pHorizontalAcceleration = 4;
    public float pVerticalAcceleration = 3;
}