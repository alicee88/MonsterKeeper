using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineChecker : MonoBehaviour
{
    public enum Line
    {
        Row,
        Column
    };

    [SerializeField] Line line = Line.Row;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckForLine()
    {
        Vector2 dir = Vector2.right;
        if (line == Line.Column)
        {
            dir = Vector2.down;
        }
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir, 7.5f);

        if (hits.Length != 0)
        {
            GameObject firstMonster = null;
            GameObject secondMonster = null;
            bool addedFirstTwoMonsters = false;

            List<GameObject> matchingMonsters = new List<GameObject>();

            foreach(RaycastHit2D hit in hits)
            {
                Debug.Log("Looking at... " + hit.collider.name);
                if (firstMonster == null)
                {
                    firstMonster = hit.collider.gameObject;
                    Debug.Log("Set first monster to " + firstMonster.name);
                    continue;
                }
                if(secondMonster == null)
                {
                    secondMonster = hit.collider.gameObject;
                    Debug.Log("Set second monster to " + secondMonster.name);
                    continue;
                }

                if(firstMonster.name != secondMonster.name)
                {
                    firstMonster = secondMonster;
                    secondMonster = hit.collider.gameObject;
                    Debug.Log("First 2 monsters dont' match, resetting");
                    continue;
                }

                if(hit.collider.gameObject.name == firstMonster.name)
                {
                    if(!addedFirstTwoMonsters)
                    {
                        matchingMonsters.Add(firstMonster);
                        matchingMonsters.Add(secondMonster);
                        addedFirstTwoMonsters = true;
                    }

                    matchingMonsters.Add(hit.collider.gameObject);
                }
          
            }

            foreach(GameObject match in matchingMonsters)
            {
                Debug.Log("Got matching monsters! "+match.name);
                Monster matchMon = match.GetComponent<Monster>();
                if(matchMon)
                {
                    matchMon.Die();
                }
            }

        }
        
    }
}
