using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> monsterPrefabs;
    [SerializeField] int numberToSpawn = 8;

    private IEnumerator Start()
    {
        int numberSpawned = 0;
        while (numberSpawned < numberToSpawn)
        {

            GameObject prefabToSpawn = CalcMonsterToSpawn();
            if(prefabToSpawn)
            {
                Instantiate(prefabToSpawn, transform.position, transform.rotation);
                numberSpawned++;
            }
            else
            {
                Debug.Log("MOSTER PREFAB LIST IS EMPTY");
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private GameObject CalcMonsterToSpawn()
    {
        List<GameObject> viableMonsterList = new List<GameObject>(monsterPrefabs);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 8f);
        if(hit.collider)
        {
            // Our empty position is one above what we hit
            Vector2 emptyPos = new Vector2(transform.position.x, hit.collider.transform.position.y + 1);
            //Now fire rays to see what monsters to cull from the list
            RaycastHit2D[] hits = Physics2D.RaycastAll(emptyPos, Vector2.down, 2.5f);
            
            if(hits.Length < 2)
            {
                // We're near the bottom of the grid already
                return monsterPrefabs[Random.Range(0, monsterPrefabs.Count)];
            }

            // The 2 things below us are the same monster so don't spawn another one of those
            if (hits[0].collider.name == hits[1].collider.name)
            {                
                foreach(GameObject prefab in viableMonsterList)
                {
                    if(prefab.GetComponent<Monster>().GetMonsterName() == hits[0].collider.GetComponent<Monster>().GetMonsterName())
                    {
                        bool removed = viableMonsterList.Remove(prefab);

                        if (removed)
                        {
                            return viableMonsterList[Random.Range(0, viableMonsterList.Count)];
                        }

                    }
                }
            }
        }
        return monsterPrefabs[Random.Range(0, monsterPrefabs.Count)];
    }
}
