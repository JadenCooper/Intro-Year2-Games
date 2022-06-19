using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MazeConstructor))]

public class GameController : MonoBehaviour
{
    private MazeConstructor constructor;
    public GameObject playerPrefab;
    public GameObject monsterPrefab;
    private AIController aIController;

    [SerializeField] private int rows;
    [SerializeField] private int cols;

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
            SetUpMaze();
        }
        if (Input.GetKeyDown("f"))
        {
            int playerCol = (int)Mathf.Round(player.transform.position.x / aIController.HallWidth);
            int playerRow = (int)Mathf.Round(player.transform.position.z / aIController.HallWidth);

            int GoalRow = aIController.Graph.GetUpperBound(0) - 1;
            int GoalCol = aIController.Graph.GetUpperBound(1) - 1;
            if(SpherePath != null)
            {
                SpherePath = null;
            }
            SpherePath = aIController.FindPath(playerRow, playerCol, GoalRow, GoalCol);
            constructor.Pathway(SpherePath);
        }
        if (Input.GetKeyDown("tab"))
        {
            constructor.showDebug =! constructor.showDebug;
        }
    }

    private GameObject CreatePlayer()
    {
        Vector3 playerStartPosition = new Vector3(constructor.hallWidth, 1, constructor.hallWidth);
        player = Instantiate(playerPrefab, playerStartPosition, Quaternion.identity);
        player.tag = "Generated";
        return player;
    }

    private GameObject CreateMonster()
    {
        Vector3 monsterPosition = new Vector3(constructor.goalCol * constructor.hallWidth, 0f, constructor.goalRow * constructor.hallWidth);
        GameObject monster = Instantiate(monsterPrefab, monsterPosition, Quaternion.identity);
        monster.tag = "Generated";

        TriggerEventRouter tc = monster.AddComponent<TriggerEventRouter>();
        tc.callback = OnMonsterTrigger;

        return monster;
    }

    private void OnTreasureTrigger(GameObject trigger, GameObject other)
    {
        Debug.Log("You Won!");
        aIController.StopAI();
    }

    private void OnMonsterTrigger(GameObject trigger, GameObject other)
    {
        Debug.Log("Gotcha!");
        aIController.StopAI();
        SetUpMaze();
    }

    private void SetUpMaze()
    {
        constructor.GenerateNewMaze(rows, cols, OnTreasureTrigger);
        aIController.Graph = constructor.graph;
        aIController.Player = CreatePlayer();
        aIController.Monster = CreateMonster();
        aIController.HallWidth = constructor.hallWidth;
        aIController.StartAI();
    }
}
