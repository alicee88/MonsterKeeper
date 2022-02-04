using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] string monsterName;
    Animator anim;
    BoardController boardController;
    public ContactFilter2D filter;
    bool isSelected = false;
    Vector3 targetPos;
    Vector2 targetDir;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        boardController = FindObjectOfType<BoardController>();

        StartCoroutine(Blink());
        StartCoroutine(Fall());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0f, 60f));
            anim.Play("Base Layer.Blink");     
        }
    }

    private IEnumerator Fall()
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
    }

    public void SetTargetDirectionAndPosition(Vector3 pos, Vector2 dir)
    {
        Debug.Log("SETTING TARGET POSITION " + pos + "for " + gameObject.name);
        targetPos = pos;
        targetDir = dir;
    }

    public void PlaySelectedAnimation()
    {
        // Make the objects up and down from the selected monster not jiggle
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up);

        if (hitUp.collider && hitUp.collider.GetComponent<Rigidbody2D>())
        {
            hitUp.collider.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        }

        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down);

        if (hitDown.collider && hitDown.collider.GetComponent<Rigidbody2D>())
        {
            hitDown.collider.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        }
        GetComponent<Animator>().SetBool("isSelected", true);
    }

    public void StopPlayingSelectedAnimation()
    {
        GetComponent<Animator>().SetBool("isSelected", false);
        isSelected = false;
    }

    private void OnMouseDown()
    {
        if (!isSelected)
        {
            boardController.SelectedAMonster(gameObject);
        }
        isSelected = true;

    }

    private void CheckForLine()
    {
        // Look up
        RaycastHit2D[] hits = new RaycastHit2D[8];
        int objectsHit = Physics2D.Raycast(transform.position, Vector2.up, filter, hits);
    }

    public string GetMonsterName()
    {
        return monsterName;
    }

}
