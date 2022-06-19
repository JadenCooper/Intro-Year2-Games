using System.Collections.Generic;
using UnityEngine;

public class MazeConstructor : MonoBehaviour
{
    public bool showDebug;

    private MazeMeshGenerator meshGenerator;
    [SerializeField] private Material mazeMat1;
    [SerializeField] private Material mazeMat2;
    [SerializeField] private Material startMat;
    [SerializeField] private Material treasureMat;

    public Node[,] graph;

    public float hallWidth { get; private set; }
    public int goalRow { get; private set; }
    public int goalCol { get; private set; }

    public float placementThreshold = 0.1f;   // chance of empty space

    private List<GameObject> SphereList;
    public int[,] data
    {
        get; private set;
    }

    void Awake()
    {
        meshGenerator = new MazeMeshGenerator();
        hallWidth = meshGenerator.width;

        SphereList = new List<GameObject>();
        // default to walls surrounding a single empty cell
        data = new int[,]
        {
            {1, 1, 1},
            {1, 0, 1},
            {1, 1, 1}
        };
    }

    public void GenerateNewMaze(int sizeRows, int sizeCols, TriggerEventHandler treasureCallback)
    {
        DisposeOldMaze();

        if (sizeRows % 2 == 0 && sizeCols % 2 == 0)
            Debug.LogError("Odd numbers work better for dungeon size.");

        data = FromDimensions(sizeRows, sizeCols);

        goalRow = data.GetUpperBound(0) - 1;
        goalCol = data.GetUpperBound(1) - 1;

        graph = new Node[sizeRows, sizeCols];

        for (int i = 0; i < sizeRows; i++)
            for (int j = 0; j < sizeCols; j++)
                graph[i, j] = data[i, j] == 0 ? new Node(i, j, true) : new Node(i, j, false);
        DisplayMaze();
        PlaceGoal(treasureCallback);
    }

    public int[,] FromDimensions(int sizeRows, int sizeCols)
    {
        int[,] maze = new int[sizeRows, sizeCols];
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        for (int i = 0; i <= rMax; i++)
            for (int j = 0; j <= cMax; j++)
                if (i == 0 || j == 0 || i == rMax || j == cMax)
                    maze[i, j] = 1;
                else if (i % 2 == 0 && j % 2 == 0 && Random.value > placementThreshold)
                {
                    maze[i, j] = 1;

                    int a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                    int b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                    maze[i + a, j + b] = 1;
                }
        return maze;
    }

    private void PlaceGoal(TriggerEventHandler treasureCallback)
    {
        GameObject treasure = GameObject.CreatePrimitive(PrimitiveType.Cube);
        treasure.transform.position = new Vector3(goalCol * hallWidth, .5f, goalRow * hallWidth);
        treasure.name = "Treasure";
        treasure.tag = "Generated";
        treasure.GetComponent<BoxCollider>().isTrigger = true;
        treasure.GetComponent<MeshRenderer>().sharedMaterial = treasureMat;

        TriggerEventRouter tc = treasure.AddComponent<TriggerEventRouter>();
        tc.callback = treasureCallback;
    }

    public void Pathway(List<Node> SpherePath)
    {
        //foreach (Node node in SpherePath)
        //{
        //    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    sphere.transform.position = new Vector3(node.x * hallWidth, .5f, node.y * hallWidth);
        //    sphere.name = "Sphere";
        //    sphere.tag = "Generated";
        //    sphere.GetComponent<SphereCollider>().isTrigger = true;
        //    sphere.GetComponent<MeshRenderer>().sharedMaterial = treasureMat;
        //}

        if(SphereList != null)
        {
            foreach(GameObject sphere in SphereList)
            {
                Destroy(sphere);
            }
            SphereList = null;
        }

        SphereList = new List<GameObject>();

        for (int i = 1; i <= (SpherePath.Count - 2); i++)
        {
            Node nextNode = SpherePath[i];
            float nextX = nextNode.y * hallWidth;
            float nextZ = nextNode.x * hallWidth;
            Vector3 endPosition = new Vector3(nextX, .7f, nextZ);
            SphereList.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere));
            SphereList[i - 1].transform.position = endPosition;
            SphereList[i - 1].name = "Sphere";
            SphereList[i - 1].tag = "Generated";
            SphereList[i - 1].GetComponent<SphereCollider>().isTrigger = true;
            SphereList[i - 1].GetComponent<MeshRenderer>().sharedMaterial = treasureMat;
        }
    }

    private void DisplayMaze()
    {
        GameObject go = new GameObject();
        go.transform.position = Vector3.zero;
        go.name = "Procedural Maze";
        go.tag = "Generated";

        MeshFilter mf = go.AddComponent<MeshFilter>();
        mf.mesh = meshGenerator.FromData(data);

        MeshCollider mc = go.AddComponent<MeshCollider>();
        mc.sharedMesh = mf.mesh;

        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        mr.materials = new Material[2] { mazeMat1, mazeMat2 };
    }

    public void DisposeOldMaze()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Generated");
        foreach (GameObject go in objects)
        {
            Destroy(go);
        }
    }

    void OnGUI()
    {
        if (!showDebug)
            return;

        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        string msg = "";

        for (int i = rMax; i >= 0; i--)
        {
            for (int j = 0; j <= cMax; j++)
                msg += maze[i, j] == 0 ? "...." : "==";
            msg += "\n";
        }

        GUI.Label(new Rect(20, 20, 500, 500), msg);
    }
}
