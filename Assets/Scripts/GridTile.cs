using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        if(col.gameObject.CompareTag("Player"))
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            if(sprite!=null)
            {
                sprite.enabled = true;
                StartCoroutine(HideSprite(sprite));
            }
        }
    }

    IEnumerator HideSprite(SpriteRenderer sprite)
    {
        yield return new WaitForSeconds(0.25f);
        sprite.enabled = false;
    }
}
