using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This Script Handle's The Spawning Of The Enemies And Waves
[System.Serializable]
public class Wave
{
    public GameObject quickBasicPrefab;
    public GameObject quickAdvancedPrefab;
    public GameObject strongBasicPrefab;
    public GameObject strongAdvancedPrefab;

    public int basicadvancedratio = 20;
    public int strongquickratio = 20;

    public float spawnInterval = 2;
    public int maxEnemies = 20;
}

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] waypoints;
    public GameObject testEnemyPrefab;
    public Wave[] waves;
    public int timeBetweenWaves = 5;
    private GameManagerBehaviour gameManager;
    private float lastSpawnTime;
    private int enemiesSpawned = 0;
    void Start()
    {
        lastSpawnTime = Time.time;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
    }

    void Update()
    {
        int currentWave = gameManager.Wave;
        if (currentWave < waves.Length)
        {
            float timeInterval = Time.time - lastSpawnTime;
            float spawnInterval = waves[currentWave].spawnInterval;

            if (((enemiesSpawned == 0 && timeInterval > timeBetweenWaves) || (enemiesSpawned != 0 && timeInterval > spawnInterval)) &&
            (enemiesSpawned < waves[currentWave].maxEnemies))
            {
                lastSpawnTime = Time.time;
                SpawnEnemies(currentWave);
                enemiesSpawned++;
            }

            if (enemiesSpawned == waves[currentWave].maxEnemies && GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                // Wave/Round Over
                gameManager.Wave++;
                gameManager.Gold = Mathf.RoundToInt(gameManager.Gold * 1.1f);
                enemiesSpawned = 0;
                lastSpawnTime = Time.time;
            }
        }
        else
        {
            // Won Game
            gameManager.gameOver = true;
            GameObject gameOverText = GameObject.FindGameObjectWithTag("GameWon");
            gameOverText.GetComponent<Animator>().SetBool("gameOver", true);
        }
    }
    
    public void SpawnEnemies(int currentWave)
    {
        // Two Enemy Types 
        // Quick Which Are Fast And Have Low Health
        // Strong Which Are Slow And Have High Health

        // Random Roll For Either Quick Or Strong
        int EnemyTypeSeed = Random.Range(0, 10);
        // Random Roll For Either Advanced Or Basic
        int EnemyQuailtySeed = Random.Range(0, 10);
        bool Advanced = false;
        bool Strong = false;

        GameObject newEnemy;

        if (EnemyTypeSeed <= waves[currentWave].strongquickratio)
        {
            // Strong
            Strong = true;
        }

        if(EnemyQuailtySeed <= waves[currentWave].basicadvancedratio)
        {
            // Advanced
            Advanced = true;
        }

        if(Strong == true)
        {
            if(Advanced == true)
            {
                // Strong Advanced
                newEnemy = (GameObject)Instantiate(waves[currentWave].strongAdvancedPrefab);
            }

            else
            {
                // Strong Basic
                newEnemy = (GameObject)Instantiate(waves[currentWave].strongBasicPrefab);
            }
        }

        else
        {
            if(Advanced == true)
            {
                // Quick Advanced
                newEnemy = (GameObject)Instantiate(waves[currentWave].quickAdvancedPrefab);
            }

            else
            {
                // Quick Basic
                newEnemy = (GameObject)Instantiate(waves[currentWave].quickBasicPrefab);
            }
        }

        newEnemy.GetComponent<MoveEnemy>().waypoints = waypoints;
    }
}
