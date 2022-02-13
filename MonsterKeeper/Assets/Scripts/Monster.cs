using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] string monsterName;
    [SerializeField] GameObject sfxPrefab;

    Animator anim;
    BoardController boardController;
    Vector3 targetPos;
    bool isMoving = false;
    bool checkForLine = false;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        boardController = FindObjectOfType<BoardController>();
        boardController.AddMonster(gameObject);

        StartCoroutine(Blink());
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, 3.0f * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPos) <= 0)
            {
                isMoving = false;
                if (checkForLine)
                {
                    boardController.CheckForLine();
                    checkForLine = false;
                }
            }
        }
    }

    IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0f, 60f));
            anim.Play("Base Layer.Blink");     
        }
    }

    public void Die()
    {
        Debug.Log("Destroying!");
        anim.Play("Base Layer.Destroyed");
        GameObject deathSFX = Instantiate(sfxPrefab, transform.position, transform.rotation);
        Destroy(deathSFX, 0.75f);
        Destroy(gameObject, 0.75f);
    }


    public void CheckForLine()
    {
        checkForLine = true;
    }

    /*private IEnumerator Fall()
    {
        while (true)
        {
            RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 0.5f);
            if(hitDown.collider)
            {
                float yPos = Mathf.Round(transform.position.y);
                transform.position = new Vector2(transform.position.x, yPos);
                yield break;
            }
            Vector2 fallPos = new Vector2(0, -Time.deltaTime * 5f);
            transform.Translate(fallPos);
            yield return null;
        }
    }*/

    public void SetTargetDirectionAndPosition(Vector3 pos)
    {
        Debug.Log("SETTING TARGET POSITION " + pos + "for " + gameObject.name);
        targetPos = pos;
        isMoving = true;
    }

    public void PlaySelectedAnimation()
    {
        GetComponent<Animator>().SetBool("isSelected", true);
    }

    public void StopPlayingSelectedAnimation()
    {
        GetComponent<Animator>().SetBool("isSelected", false);
    }

    private void OnMouseDown()
    {
        
        boardController.SelectedAMonster(gameObject);
    
    }

    public string GetMonsterName()
    {
        return monsterName;
    }

}
