using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEni : MonoBehaviour
{
    public List<GameObject> enemiesInRange;
    void Start()
    {
        enemiesInRange = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
            enemiesInRange.Add(other.gameObject);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
            enemiesInRange.Remove(other.gameObject);
    }
}
