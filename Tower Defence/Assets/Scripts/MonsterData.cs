using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
// This Script Handle's The Tower Levels
public class MonsterLevel
{
    // Monster Level Stats
    public int cost;
    // Character Sprite
    public GameObject visualization;
    // Bullet
    public GameObject bullet;
    public float fireRate;
    // Fire Area
    public float fireradius;
}
public class MonsterData : MonoBehaviour
{
    public List<MonsterLevel> levels;
    private MonsterLevel currentLevel;
    void OnEnable()
    {
        CurrentLevel = levels[0];
    }
    public MonsterLevel CurrentLevel
    {

        get { return currentLevel; }

        set
        {
            currentLevel = value;
            int currentLevelIndex = levels.IndexOf(currentLevel);

            GameObject levelVisualization = levels[currentLevelIndex].visualization;

            for (int i = 0; i < levels.Count; i++)
            {
                if (levelVisualization != null)
                    if (i == currentLevelIndex)
                        levels[i].visualization.SetActive(true);
                    else
                        levels[i].visualization.SetActive(false);
            }
        }

    }
    public MonsterLevel GetNextLevel()
    {
        int currentLevelIndex = levels.IndexOf(currentLevel);
        int maxLevelIndex = levels.Count - 1;
        if (currentLevelIndex < maxLevelIndex)
        {
            return levels[currentLevelIndex + 1];
        }
        else
        {
            // Max Level
            return null;
        }
    }
    public void IncreaseLevel()
    {
        int currentLevelIndex = levels.IndexOf(currentLevel);
        if (currentLevelIndex < levels.Count - 1)
        {
            CurrentLevel = levels[currentLevelIndex + 1];

            // This Changes The Circle Collider For The Fire Radius By Level
            CircleCollider2D collider = this.transform.GetComponent<CircleCollider2D>();
            collider.radius = levels[currentLevelIndex + 1].fireradius;
            Debug.Log(levels[currentLevelIndex + 1].fireradius);
        }
    }

}
