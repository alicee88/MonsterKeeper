using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] monsterPrefabs;
    int numberOfMonstersInColumn = 0;
    const int maxMonstersInColumn = 8;

    IEnumerator Start()
    {
        while (!ColumnIsFull())
        {
            SpawnMonster();
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void SpawnMonster()
    {
        int randomMonsterIndex = Random.Range(0, monsterPrefabs.Length);
        Instantiate(monsterPrefabs[randomMonsterIndex], transform.position, transform.rotation);
        numberOfMonstersInColumn++;
    }

    private bool ColumnIsFull()
    {
        return numberOfMonstersInColumn == maxMonstersInColumn;
    }
}
