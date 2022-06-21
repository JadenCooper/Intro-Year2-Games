using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSpawner : MonoBehaviour
{
    // This script handle's spawning of the sheep
    public bool canSpawn = true;
    public GameObject sheepPrefab;
    public List<Transform> sheepSpawnPositions = new List<Transform>();
    public float timeBetweenSpawns;
    private List<GameObject> sheepList = new List<GameObject>();

    public float additiveSpeed = 0f;
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }
    private void SpawnSheep()
    {
        Vector3 randomPosition = sheepSpawnPositions[Random.Range(0, sheepSpawnPositions.Count)].position;
        GameObject sheep = Instantiate(sheepPrefab, randomPosition, sheepPrefab.transform.rotation);
        //Increaces the spawned sheep's run speed by the current additive speed
        sheep.GetComponent<Sheep>().runSpeed += additiveSpeed;
        sheepList.Add(sheep);
        sheep.GetComponent<Sheep>().SetSpawner(this);
    }
    private IEnumerator SpawnRoutine()
    {
        while (canSpawn)
        {
            SpawnSheep();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
    public void RemoveSheepFromList(GameObject sheep)
    {
        sheepList.Remove(sheep);
    }
    public void DestroyAllSheep()
    {
        // This method destroys all sheep at the end of the game
        foreach (GameObject sheep in sheepList)
        {
            Destroy(sheep);
        }

        sheepList.Clear();
    }
}
