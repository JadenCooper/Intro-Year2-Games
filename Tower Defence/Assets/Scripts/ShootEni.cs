using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEni : MonoBehaviour
{
    // This Script Handle's The Tower's Targeting

    public List<GameObject> enemiesInRange;
    private float lastShotTime;
    private MonsterData monsterData;
    void Start()
    {
        lastShotTime = Time.time;
        monsterData = gameObject.GetComponentInChildren<MonsterData>();
        enemiesInRange = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject target = null;
        float minimalEnemyDistance = float.MaxValue;
        foreach (GameObject enemy in enemiesInRange)
        {
            // This Foreach Goes Through All Targets In Range Then Sets Target To The Closest One To The End
            float distanceToGoal = enemy.GetComponent<MoveEnemy>().DistanceToGoal();
            if (distanceToGoal < minimalEnemyDistance)
            {
                target = enemy;
                minimalEnemyDistance = distanceToGoal;
            }
        }

        if (target != null)
        {
            // Found Target So Fire
            if (Time.time - lastShotTime > monsterData.CurrentLevel.fireRate)
            {
                Shoot(target.GetComponent<Collider2D>());
                lastShotTime = Time.time;
            }
            Vector3 direction = gameObject.transform.position - target.transform.position;
            gameObject.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI, new Vector3(0, 0, 1));
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // Target In Range
        if (other.gameObject.tag.Equals("Enemy"))
            enemiesInRange.Add(other.gameObject);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        // Target Left Range
        if (other.gameObject.tag.Equals("Enemy"))
            enemiesInRange.Remove(other.gameObject);
    }
    void Shoot(Collider2D target)
    {
        // Assign Bullet Type Based On Tower's Level
        GameObject bulletPrefab = monsterData.CurrentLevel.bullet;
        Vector3 startPosition = gameObject.transform.position;
        Vector3 targetPosition = target.transform.position;
        startPosition.z = bulletPrefab.transform.position.z;
        targetPosition.z = bulletPrefab.transform.position.z;

        GameObject newBullet = (GameObject)Instantiate(bulletPrefab);
        newBullet.transform.position = startPosition;
        BulletBehaviour bulletComp = newBullet.GetComponent<BulletBehaviour>();
        bulletComp.target = target.gameObject;
        bulletComp.startPosition = startPosition;
        bulletComp.targetPosition = targetPosition;

        Animator animator = monsterData.CurrentLevel.visualization.GetComponent<Animator>();
        animator.SetTrigger("fireShot");
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);
    }
}
