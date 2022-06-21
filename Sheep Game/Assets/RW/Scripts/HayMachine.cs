using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayMachine : MonoBehaviour
{
    // This Script Manage's Hay Machine

    public float movementSpeed;

    // The HorizontalBoundary is the ends of the Hay Machine Track
    public float horizontalBoundary = 22;
    public GameObject hayBalePrefab;
    public Transform haySpawnpoint;
    public float shootInterval;
    private float shootTimer;
    public Transform modelParent;
    public GameObject blueModelPrefab;
    public GameObject yellowModelPrefab;
    public GameObject redModelPrefab;
    void Start()
    {
        LoadModel();
    }
    private void LoadModel()
    {
        // This method handle's the color of the Hay Machine

        Destroy(modelParent.GetChild(0).gameObject);

        switch (GameSettings.hayMachineColor)
        {
            case HayMachineCol.Blue:
                Instantiate(blueModelPrefab, modelParent);
                break;

            case HayMachineCol.Yellow:
                Instantiate(yellowModelPrefab, modelParent);
                break;

            case HayMachineCol.Red:
                Instantiate(redModelPrefab, modelParent);
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        UpdateShooting();
    }
    private void UpdateMovement()
    {
        // This method controls the movement of the Haymachine 

        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput < 0 && transform.position.x > -horizontalBoundary)
        {
            transform.Translate(transform.right * -movementSpeed * Time.deltaTime);
        }
        else if (horizontalInput > 0 && transform.position.x < horizontalBoundary)
        {
            transform.Translate(transform.right * movementSpeed * Time.deltaTime);
        }
    }
    private void UpdateShooting()
    {
        // This method handle's shooting intervals so the player cant constantly shoot
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0 && Input.GetKey(KeyCode.Space))
        {
            shootTimer = shootInterval;
            ShootHay();
        }
    }
    private void ShootHay()
    {
        // This method spawns the hay and plays the soundclip
        SoundManager.Instance.PlayShootClip();
        Instantiate(hayBalePrefab, haySpawnpoint.position, Quaternion.identity);
    }
}
