using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MazeConstructor))]

public class GameController : MonoBehaviour
{
    // This Script Controls The General Gameplay
    private MazeConstructor constructor;
    public GameObject playerPrefab;
    public GameObject monsterPrefab;
    private AIController aIController;

    [SerializeField] private int maxRows;
    [SerializeField] private int maxCols;
    [SerializeField] private int minRows;
    [SerializeField] private int minCols;
    private int rows;
    private int cols;

    List<Node> SpherePath;

    public GameObject player;
    void Awake()
    {
        constructor = GetComponent<MazeConstructor>();
        aIController = GetComponent<AIController>();
    }

    void Start()
    {
        SetUpMaze();
    }

    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            // Restarts The Maze
            SetUpMaze();
        }
        if (Input.GetKeyDown("f"))
        {
            // This Generates The Sphere Path To The Treasure
            int playerCol = (int)Mathf.Round(player.transform.position.x / aIController.HallWidth);
            int playerRow = (int)Mathf.Round(player.transform.position.z / aIController.HallWidth);

            int GoalRow = aIController.Graph.GetUpperBound(0) - 1;
            int GoalCol = aIController.Graph.GetUpperBound(1) - 1;
            if(SpherePath != null)
            {
                // Makes Sure The Path Is Clear If Not Clear It
                SpherePath = null;
            }
            // Find Path From Player To The Treasure
            SpherePath = aIController.FindPath(playerRow, playerCol, GoalRow, GoalCol);
            // Generate The Spheres
            constructor.Pathway(SpherePath);
        }
        if (Input.GetKeyDown("tab"))
        {
            // Shows The Debug Map To The Player
            constructor.showDebug =! constructor.showDebug;
        }
    }

    private GameObject CreatePlayer()
    {
        // Create The Player
        Vector3 playerStartPosition = new Vector3(constructor.hallWidth, 1, constructor.hallWidth);
        player = Instantiate(playerPrefab, playerStartPosition, Quaternion.identity);
        player.tag = "Generated";
        return player;
    }

    private GameObject CreateMonster()
    {
        // Create The Monster
        Vector3 monsterPosition = new Vector3(constructor.goalCol * constructor.hallWidth, 0f, constructor.goalRow * constructor.hallWidth);
        GameObject monster = Instantiate(monsterPrefab, monsterPosition, Quaternion.identity);
        monster.tag = "Generated";

        TriggerEventRouter tc = monster.AddComponent<TriggerEventRouter>();
        tc.callback = OnMonsterTrigger;

        return monster;
    }

    private void OnTreasureTrigger(GameObject trigger, GameObject other)
    {
        // Won The Game, Stop The AI
        Debug.Log("You Won!");
        aIController.StopAI();
    }

    private void OnMonsterTrigger(GameObject trigger, GameObject other)
    {
        // Lost The Game So Restart
        Debug.Log("Gotcha!");
        aIController.StopAI();
        SetUpMaze();
    }

    private void SetUpMaze()
    {
        // Resets The Game

        // These Next While Loops Randomize The Maze Row And Cols
        do
        {
            rows = UnityEngine.Random.Range(1, maxRows);
        } while ((rows % 2 == 0) || rows < minRows);
        // They Have To Be Lower Than The Max, High Than The Min And Odd
        do
        {
            cols = UnityEngine.Random.Range(1, maxCols);
        } while ((cols % 2 == 0) || cols < minCols);

        constructor.GenerateNewMaze(rows, cols, OnTreasureTrigger);
        aIController.Graph = constructor.graph;
        aIController.Player = CreatePlayer();
        aIController.Monster = CreateMonster();
        aIController.HallWidth = constructor.hallWidth;
        aIController.StartAI();
    }
}
