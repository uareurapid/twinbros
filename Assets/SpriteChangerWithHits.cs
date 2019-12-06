using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChangerWithHits : MonoBehaviour, HandlePlayerCollision, ResetBehaviourScript
{

    public Sprite[] sprites;
    public int numberOfHitsForChange = 1;
    public bool hasCollider = false;
    public int disableColliderAfterNumHits = 0;
    public bool deactivateAfterLastSprite = false;
    private int currentSpriteIndex = -1;
    private int numCollisions = 0;

    public Sprite originalSprite;

    // Start is called before the first frame update
    // Use this for initialization
    void Start()
    {
        currentSpriteIndex = -1;
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        originalSprite = rend.sprite;
        StartCoroutine(AddToResetableList());

    }

    IEnumerator AddToResetableList()
    {
        yield return new WaitForSecondsRealtime(2f);
        GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
        if (scripts != null)
        {

            LevelManager levelManager = scripts.GetComponent<LevelManager>();
            if (levelManager != null)
            {
                levelManager.AddResetableBehaviourObject(this);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleExitCollision(PlayerMovement player)
    {

    }

    public void HandleCollision(PlayerMovement player)
    {
        numCollisions += 1;
        currentSpriteIndex += 1;
        if(currentSpriteIndex <= sprites.Length-1)
        {

            SpriteRenderer rend = GetComponent<SpriteRenderer>();
            if (rend!=null)
            {
                rend.sprite = sprites[currentSpriteIndex];
            }


            if(numCollisions == disableColliderAfterNumHits)
            {
                Collider2D col = GetComponent<Collider2D>();
                if(col!=null)
                {
                    col.enabled = false;
                }
            }
        }

        if(deactivateAfterLastSprite && numCollisions == sprites.Length)
        {
            StartCoroutine(Deactivate(0.125f));
        }
    }

    IEnumerator Deactivate(float wait)
    {
        yield return new WaitForSeconds(wait);
        gameObject.SetActive(false);

    }

    public void ResetOriginalBehaviour()
    {
        if (gameObject != null)
        {
            gameObject.SetActive(true);

            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = true;
            }
        }
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        if (rend != null)
        {
            rend.sprite = originalSprite;
        }
        currentSpriteIndex = -1;
        numCollisions = 0;
    }
}
