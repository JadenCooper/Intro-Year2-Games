using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    // This Script Handle's The Sheep
    public float runSpeed;
    public float gotHayDestroyDelay;
    private bool hitByHay;
    private bool dropped;
    public float dropDestroyDelay;
    private Collider myCollider;
    private Rigidbody myRigidbody;
    private SheepSpawner sheepSpawner;
    public float heartOffset;
    public GameObject heartPrefab;

    void Start()
    {
        myCollider = GetComponent<Collider>();
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
    }
    private void HitByHay()
    {
        // Spawn The Sheep Heart
        Instantiate(heartPrefab, transform.position + new Vector3(0, heartOffset, 0), Quaternion.identity);
        GameStateManager.Instance.SavedSheep();
        TweenScale tweenScale = gameObject.AddComponent<TweenScale>();
        // Set TweenScale Settings
        tweenScale.targetScale = 0;
        tweenScale.timeToReachTarget = gotHayDestroyDelay;
        sheepSpawner.RemoveSheepFromList(gameObject);
        hitByHay = true;
        runSpeed = 0;
        SoundManager.Instance.PlaySheepHitClip();
        Destroy(gameObject, gotHayDestroyDelay);
    }
    private void OnTriggerEnter(Collider other)
    {
        // This Method Activates Upon Hitting A Trigger
        if (other.CompareTag("Hay") && !hitByHay)
        {
            // Hit By Hay So This Sheep Is Saved
            Destroy(other.gameObject);
            HitByHay();
        }
        else if (other.CompareTag("DropSheep") && !dropped)
        {
            // Made It Past The HayMachine And Escaped So Lose Life
            Drop();
        }
    }
    private void Drop()
    {
        GameStateManager.Instance.DroppedSheep();
        sheepSpawner.RemoveSheepFromList(gameObject);
        dropped = true;
        myRigidbody.isKinematic = false;
        myCollider.isTrigger = false;
        SoundManager.Instance.PlaySheepDroppedClip();
        Destroy(gameObject, dropDestroyDelay);
    }
    public void SetSpawner(SheepSpawner spawner)
    {
        sheepSpawner = spawner;
    }
}
