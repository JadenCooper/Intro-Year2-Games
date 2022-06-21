using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    // This script handle's the bullets

    public float speed = 10;
    public int damage;
    public GameObject target;
    public Vector3 startPosition;
    public Vector3 targetPosition;
    private Vector3 normalizeDirection;
    private GameManagerBehaviour gameManager;

    // Start is called before the first frame update
    void Start()
    {
        normalizeDirection = (targetPosition - startPosition).normalized;
        GameObject gm = GameObject.Find("GameManager");
        gameManager = gm.GetComponent<GameManagerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += normalizeDirection * speed * Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        target = other.gameObject;
        if (target.tag.Equals("Enemy"))
        {
            Transform healthBarTransform = target.transform.Find("HealthBar");
            Healthbar healthBar = healthBarTransform.gameObject.GetComponent<Healthbar>();
            healthBar.currentHealth -= damage;

            if (healthBar.currentHealth <= 0)
            {
                // Killed Target
                Destroy(target);
                // Play sound
                AudioSource audioSource = target.GetComponent<AudioSource>();
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
                gameManager.Gold += 50;
            }
            Destroy(gameObject);
        }
    }
}
