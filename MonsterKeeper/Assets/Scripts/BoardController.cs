using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] GameObject pandaPrefab;
    [SerializeField] GameObject monkeyPrefab;
    const int gridDims = 8;
    bool gameStarted = false;
    GameObject firstMonsterSelected;

    List<GameObject> monstersOnBoard = new List<GameObject>();

    // Start is called before the first frame update
    IEnumerator Start()
    {
        AddTestMonsters();
        while (!gameStarted)
        {
            yield return new WaitForSeconds(1.0f);
            gameStarted = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //CheckForLine();
    }

    private void AddTestMonsters()
    {
        Vector2 pos = transform.position;
        monstersOnBoard.Add(Instantiate(pandaPrefab, new Vector2(1, 4), transform.rotation));
        monstersOnBoard.Add(Instantiate(pandaPrefab, new Vector2(1, 5), transform.rotation));
        monstersOnBoard.Add(Instantiate(pandaPrefab, new Vector2(1, 6), transform.rotation));
        monstersOnBoard.Add(Instantiate(monkeyPrefab, new Vector2(1, 7), transform.rotation));
        monstersOnBoard.Add(Instantiate(pandaPrefab, new Vector2(1, 8), transform.rotation));
        monstersOnBoard.Add(Instantiate(monkeyPrefab, new Vector2(1, 9), transform.rotation));
        monstersOnBoard.Add(Instantiate(pandaPrefab, new Vector2(1, 10), transform.rotation));
        monstersOnBoard.Add(Instantiate(monkeyPrefab, new Vector2(1, 11), transform.rotation));
    }

    public bool GameStarted()
    {
        return gameStarted;
    }

    public void SelectedAMonster(GameObject monster)
    {
        if(!firstMonsterSelected)
        {
            firstMonsterSelected = monster;
            monster.GetComponent<Monster>().PlaySelectedAnimation();
        }
        else if(firstMonsterSelected != monster)
        {
            Vector2 hitDirection = MonstersAreAdjacent(firstMonsterSelected, monster);

            if(hitDirection != new Vector2(0,0))
            {
                Vector2 firstMonsterPos = firstMonsterSelected.transform.position;

                //firstMonsterSelected.transform.position = monster.transform.position;
                //monster.transform.position = firstMonsterPos;
                firstMonsterSelected.GetComponent<Monster>().SetTargetDirectionAndPosition(monster.transform.position, hitDirection);
                monster.GetComponent<Monster>().SetTargetDirectionAndPosition(firstMonsterSelected.transform.position, -hitDirection);
                firstMonsterSelected.GetComponent<Monster>().StopPlayingSelectedAnimation();
            }
            
        }
    }

    private Vector2 MonstersAreAdjacent(GameObject monster1, GameObject monster2)
    {
        // Fire a line to make sure the monsters are next to each other
        RaycastHit2D hitUp = Physics2D.Raycast(monster1.transform.position, Vector2.up);

        if (hitUp.collider && hitUp.collider.gameObject == monster2)
        {
            Debug.Log("Got monster above our selected monster " + monster2.name);
            return Vector2.up;
        }

        RaycastHit2D hitDown = Physics2D.Raycast(firstMonsterSelected.transform.position, Vector2.down);

        if (hitDown.collider && hitDown.collider.gameObject == monster2)
        {
            Debug.Log("Got monster below our selected monster " + monster2.name);
            return Vector2.down;
        }

        RaycastHit2D hitLeft = Physics2D.Raycast(firstMonsterSelected.transform.position, Vector2.left);

        if (hitLeft.collider && hitLeft.collider.gameObject == monster2)
        {
            Debug.Log("Got monster to the left of our selected monster " + monster2.name);
            return Vector2.left;
        }

        RaycastHit2D hitRight = Physics2D.Raycast(firstMonsterSelected.transform.position, Vector2.right);

        if (hitRight.collider && hitRight.collider.gameObject == monster2)
        {
            Debug.Log("Got monster to the right of our selected monster " + monster2.name);
            return Vector2.right;
        }

        return new Vector2(0, 0);

    }

    /* private void CheckForLine()
     {
         Queue<GameObject> matchingColumnTrio = new Queue<GameObject>();
         Queue<GameObject> matchingRowTrio = new Queue<GameObject>();
         int trioRowIndex = 0;

         for (int i = 0; i < gridDims; i++)
         {
             if(trioRowIndex < 3)
             {
                 //matchingRowTrio.Add(monsters[i])
             }
             for (int j = 0; j < gridDims; j++)
             {
                 if(matchingColumnTrio.Count < 3)
                 {
                     if(monsters[i,j] != null)
                     {
                         Debug.Log("ENQUEING MONSTER " + monsters[i, j].name);
                         matchingColumnTrio.Enqueue(monsters[i, j]);

                     }
                 }
                 else
                 {
                     if (GotMatchingTrio(matchingColumnTrio))
                     {
                         Debug.Log("GOT MATCHING TRIO!");
                         RemoveTrio(matchingColumnTrio);
                         matchingColumnTrio.Clear();
                         Debug.Log("NUMBER OF THINGS IN TRIO " + matchingColumnTrio.Count);
                     }
                     else
                     {
                         matchingColumnTrio.Dequeue();

                     }
                 }
             }

         }
     }

     private void RemoveTrio(Queue<GameObject> trio)
     {

         foreach (GameObject monster in trio)
         {

             for(int j = 0; j < gridDims; j++)
             {
                 for(int k = 0; k < gridDims; k++)
                 {
                     if(monsters[j, k] == monster)
                     {
                         Debug.Log("Got a match for removing from teh array, deleting it now");
                         monsters[j, k] = null;
                     }
                 }
             }
             Destroy(monster);
         }
     }

     private bool GotMatchingTrio(Queue<GameObject> trio)
     {
         if (trio.Count == 3)
         {
             Debug.Log("Got 3 in the list, checking for matches");
             bool gotMatch = true;
             string lastName = trio.Peek().name;
             foreach (GameObject monster in trio)
             {
                 if (monster.name != lastName)
                 {
                     gotMatch = false;
                 }
                 lastName = monster.name;
             }
             if (gotMatch)
             {
                 return true;
             }
         }
         return false;
     }*/

    public void AddMonster(GameObject monster)
    {
        monstersOnBoard.Add(monster);
    }
}
