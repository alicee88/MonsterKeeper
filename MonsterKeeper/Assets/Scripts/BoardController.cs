using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    const int gridDims = 8;
    GameObject firstMonsterSelected;

    List<GameObject> monstersOnBoard = new List<GameObject>();
    LineChecker[] lineCheckers; 

    // Start is called before the first frame update
    private void Start()
    {
        lineCheckers = FindObjectsOfType<LineChecker>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SelectedAMonster(GameObject monster)
    {
        if(!firstMonsterSelected)
        {
            Debug.Log("No first monster selected - make " + monster.name + " first selected");
            firstMonsterSelected = monster;
            monster.GetComponent<Monster>().PlaySelectedAnimation();
        }
        else if(firstMonsterSelected != monster)
        {
            Debug.Log("Already got a first monster selected (" + firstMonsterSelected.name + ")");
            Vector2 hitDirection = MonstersAreAdjacent(firstMonsterSelected, monster);

            if(hitDirection != new Vector2(0,0))
            {

                SwapMonsterWithFirstSelected(monster);
            }
            else
            {
                Debug.Log("Monsters aren't adjacent");
                firstMonsterSelected.GetComponent<Monster>().StopPlayingSelectedAnimation();
                firstMonsterSelected = monster;
                monster.GetComponent<Monster>().PlaySelectedAnimation();
            }
        }
    }

    public void CheckForLine()
    {
        foreach(LineChecker lc in lineCheckers)
        {
            lc.CheckForLine();
        }
    }

    private void SwapMonsterWithFirstSelected(GameObject monster)
    {
        Debug.Log("Got 2 adjacent monsters. First: " + firstMonsterSelected.name + " Second: " + monster.name);
        Vector2 firstMonsterPos = firstMonsterSelected.transform.position;
        firstMonsterSelected.GetComponent<Monster>().SetTargetDirectionAndPosition(monster.transform.position);
        firstMonsterSelected.GetComponent<Monster>().CheckForLine();

        monster.GetComponent<Monster>().SetTargetDirectionAndPosition(firstMonsterSelected.transform.position);
        firstMonsterSelected.GetComponent<Monster>().StopPlayingSelectedAnimation();
        firstMonsterSelected = null;
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

    public void AddMonster(GameObject monster)
    {
        monstersOnBoard.Add(monster);
    }
}
