using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTileDropper : MonoBehaviour
{
    public Transform[] gridTileOccupiers; //what instantiate there

    public float delayBetweenDrops = 10f; //how much between each drop?
    // Start is called before the first frame update

    private bool canDrop = true;
    void Start()
    {
        
    }

    public Transform GetOccuppier()
    {
        if(gridTileOccupiers!=null && gridTileOccupiers.Length > 0)
        {
            return gridTileOccupiers[Random.Range(0, gridTileOccupiers.Length - 1)];
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Drop()
    {
        canDrop = false;
        StartCoroutine(EnableDropAgain());
    }

    public bool CanDropNext()
    {
        return canDrop;
    }

    IEnumerator EnableDropAgain()
    {
        yield return new WaitForSeconds(delayBetweenDrops);
        canDrop = true;
    }
}
