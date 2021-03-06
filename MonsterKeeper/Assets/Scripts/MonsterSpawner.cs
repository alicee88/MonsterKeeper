using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> monsterPrefabs;
    [SerializeField] int numberToSpawn = 9;

    int numCols;
    int numRows;
    Vector2 START_GRID_POS = new Vector2(1, 3);
    Vector2 SPAWN_POS = new Vector2(0, 11);
    List<GameObject> viableMonsters;
    bool isSpawning = false;
    List<float> spawnXPos = new List<float>();

    private void Start()
    {
        InitializeBoard();
    }

    

    private void InitializeBoard()
    {
        numRows = Mathf.RoundToInt(Mathf.Sqrt(numberToSpawn));
        numCols = Mathf.RoundToInt(Mathf.Sqrt(numberToSpawn));
        Vector2 spawnPos = new Vector2(START_GRID_POS.x, START_GRID_POS.y);

        for(int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                spawnPos = new Vector2(START_GRID_POS.x + i, START_GRID_POS.y + j);
                GameObject spawnPrefab = CalcMonsterToSpawn(spawnPos);
                Instantiate(spawnPrefab, spawnPos, transform.rotation);
            }
        }

    }

    private void RemoveDuplicateMonster(Vector2 gridPos, Vector2 dir)
    {
        //Now fire rays to see what monsters to cull from the list
        RaycastHit2D[] hits = Physics2D.RaycastAll(gridPos, dir, 2.5f);

        if (hits.Length > 1)
        {
            if (hits[0].collider.name == hits[1].collider.name)
            {
                foreach (GameObject prefab in viableMonsters)
                {
                    if (prefab.GetComponent<Monster>().GetMonsterName() == hits[0].collider.GetComponent<Monster>().GetMonsterName())
                    {
                        viableMonsters.Remove(prefab);
                        break;
                    }
                }
            }
        }
    }

    private GameObject CalcMonsterToSpawn(Vector2 gridPos)
    {
        viableMonsters = new List<GameObject>(monsterPrefabs);

        //Now fire rays to see what monsters to cull from the list
        RemoveDuplicateMonster(gridPos, Vector2.down);
        RemoveDuplicateMonster(gridPos, Vector2.left);
        RemoveDuplicateMonster(gridPos, Vector2.right);
       
        return viableMonsters[Random.Range(0, viableMonsters.Count)];
    }

    public void StartSpawnMonster(float xPos)
    {
        spawnXPos.Add(xPos);
        Debug.Log("Added new spawn pos " + xPos);
        if (!isSpawning)
        {
            Debug.Log("Starting coroutine...");
            StartCoroutine(SpawnMonster());
        }
    }

    IEnumerator SpawnMonster()
    {
        isSpawning = true;
        while(spawnXPos.Count > 0)
        {
            Debug.Log("Spawning new monster at " + spawnXPos[0].ToString());
            Vector2 newSpawnPos = new Vector2(SPAWN_POS.x + spawnXPos[0], SPAWN_POS.y);
            Instantiate(monsterPrefabs[Random.Range(0, monsterPrefabs.Count)], newSpawnPos, transform.rotation);
            spawnXPos.RemoveAt(0);
            yield return new WaitForSeconds(0.5f);
            isSpawning = false;
        }
        
        Debug.Log("Stopping coroutine");
    }

}
