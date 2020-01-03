using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTile : MonoBehaviour
{
    public bool canTurnIntoEnemy = false;
    public float delay = 0f;
    public float interval = 0f;
    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        if(canTurnIntoEnemy)
        {
            if(delay > 0f)
            {
                StartCoroutine(WaitRoutine());

            } else
            {
                StartCoroutine(TurnIntoEnemy());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitRoutine()
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(TurnIntoEnemy());
    }

    IEnumerator TurnIntoEnemy()
    {
        
        Collider2D col = GetComponent<Collider2D>();
        if(col!=null && enemy!=null)
        {
            col.enabled = false;
            enemy.SetActive(true);
        }

        yield return new WaitForSeconds(interval);
        StartCoroutine(TurnIntoTile());
    }

    IEnumerator TurnIntoTile()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null && enemy != null)
        {
            col.enabled = true;
            enemy.SetActive(false);
        }

        yield return new WaitForSeconds(interval);
        StartCoroutine(TurnIntoEnemy());
    }


}
