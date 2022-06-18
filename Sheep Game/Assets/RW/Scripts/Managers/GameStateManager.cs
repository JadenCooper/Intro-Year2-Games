using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public UIManager uimanager;

    [HideInInspector]
    public int sheepSaved;

    [HideInInspector]
    public int sheepDropped;

    public int sheepDroppedBeforeGameOver;
    public SheepSpawner sheepSpawner;

    private int sheepSavedCounter = 0;
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
        }
    }
    public void SavedSheep()
    {
        sheepSaved++;
        sheepSavedCounter++;
        if(sheepSavedCounter == 5 || sheepSavedCounter == 10)
        {
            sheepSpawner.GetComponent<SheepSpawner>().additiveSpeed += 0.5f;
            if (sheepSavedCounter == 10)
            {
                sheepSavedCounter = 0;
                if(sheepDropped != 0)
                {
                    sheepDropped--;
                    UIManager.Instance.UpdateSheepDropped();
                }
                if(sheepSpawner.GetComponent<SheepSpawner>().timeBetweenSpawns != 1F)
                {
                    sheepSpawner.GetComponent<SheepSpawner>().timeBetweenSpawns -= 0.2F;
                }
            }
        }
        UIManager.Instance.UpdateSheepSaved();
    }
    private void GameOver()
    {
        sheepSpawner.canSpawn = false;
        sheepSpawner.DestroyAllSheep();
        uimanager.SetHighScore();
        UIManager.Instance.ShowGameOverWindow();
    }
    public void DroppedSheep()
    {
        sheepDropped++;
        UIManager.Instance.UpdateSheepDropped();

        if (sheepDropped == sheepDroppedBeforeGameOver)
        {
            GameOver();
        }
    }
}
