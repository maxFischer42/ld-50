using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int globalEnemyCount;

    private int difficulties = 9;
    public int currentDifficulty = 1;

    public GameObject[] enemyPrefabs;
    public List<GameObject> easyEnemies = new List<GameObject>();
    public List<GameObject> mediumEnemies = new List<GameObject>();
    public List<GameObject> hardEnemies = new List<GameObject>();

    public int MAX_ENEMIES = 10;

    public Transform[] spawnPoints;

    private void Start()
    {
        SpawnSwarmOfEnemies();
    }

    private void Update()
    {
        if(globalEnemyCount < MAX_ENEMIES)
        {
            SpawnSwarmOfEnemies();
        }
    }

    public void INCREASE_DIFFICULTY()
    {
        currentDifficulty++;
        MAX_ENEMIES += 2;
        if (currentDifficulty > difficulties) currentDifficulty = difficulties;
    }

    public void InstantiateObject(GameObject p, Vector2 pos)
    {
        globalEnemyCount++;
        Instantiate(p, pos, Quaternion.identity);
    }

    public Transform GetSpawnClosestToPlayer()
    {
        int r = Random.Range(0, spawnPoints.Length);
        return spawnPoints[r];
    }

    public void SpawnSwarmOfEnemies()
    {
        List<GameObject> enemiesToSpawn = CreateEnemyGroupFromDifficulty();
        Transform spawnPos = GetSpawnClosestToPlayer();
        float offset = 0;
        foreach(GameObject e in enemiesToSpawn)
        {
            InstantiateObject(e, (Vector2)spawnPos.position + (Vector2.right * offset));
            offset += 0.5f;
        }
    }

    private int[] difficultySwarmMin = { 1, 2, 3, 3, 2, 4, 3, 3, 5 };
    private int[] difficultySwarmMax = { 3, 5, 7, 9, 11, 14, 9, 12, 10 };

    private Vector3[] chanceBasedOnDifficulty
        = { new Vector3(1, -1, -1),
            new Vector3(0.8f, 0.2f, -1),
            new Vector3(0.6f, 0.4f, -1),
            new Vector3(0.2f, 0.8f, -1),
            new Vector3(-1, 1, -1),
            new Vector3(-1, 0.8f, 0.2f),
            new Vector3(-1, 0.6f, 0.4f),
            new Vector3(-1, 0.2f, 0.8f),
            new Vector3(-1, -1, 1)};

    public List<GameObject> CreateEnemyGroupFromDifficulty()
    {
        List<GameObject> list = new List<GameObject>();
        int low = difficultySwarmMin[currentDifficulty - 1];
        int high = difficultySwarmMax[currentDifficulty - 1];
        int numToSpawn = Random.Range(low, high);

        for(int i = 0; i < numToSpawn; i++)
        {
            float chance = Random.Range(0.1f, 1.0f);
            int diff = 1;
            Vector3 check = chanceBasedOnDifficulty[currentDifficulty - 1];
            if (currentDifficulty == 1)
            {
                diff = 0;
            } else if (check.x > check.y && currentDifficulty < 5)
            {
                if (chance >= check.x) diff = 0;
                else diff = 1;
            } else if (check.x < check.y && currentDifficulty < 5)
            {
                if (chance >= check.y) diff = 1;
                else diff = 0;
            } else if(check.x == -1 && check.z == -1)
            {
                diff = 1;
            } else if (check.y > check.z && currentDifficulty > 5)
            {
                if (chance >= check.y) diff = 1;
                else diff = 2;
            } else if (check.y < check.z && currentDifficulty > 5)
            {
                if (chance >= check.z) diff = 2;
                else diff = 1;
            } else if (check.x == -1 && check.y == -1)
            {
                diff = 2;
            }
            Debug.Log(chance + " " + diff);

            int type = Random.Range(0, 4);

            GameObject enemy = new GameObject();

            switch(diff)
            {
                case 0:
                    enemy = easyEnemies[type];
                    break;
                case 1:
                    enemy = mediumEnemies[type];
                    break;
                case 2:
                    enemy = hardEnemies[type];
                    break;
            }

            list.Add(enemy);
        }
        return list;
    }

}
