using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//something that is dropped at a grid tile position
public class GridTileOccupier : MonoBehaviour
{

    public float lifeTime = 3f; //autodestroy after
    private GridTile parentGrid;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetParentGridTile(GridTile parent)
    {
        parentGrid = parent;
        parentGrid.isOccupied = true;
        transform.parent = parent.transform;
        Invoke("DestroyMe", lifeTime);
    }

    //clear the grid position
    public void OnDestroy()
    {
        if(parentGrid!=null && parentGrid.isOccupied)
        {
            parentGrid.isOccupied = false;
        }
    }

    void DestroyMe()
    {
        Debug.Log("DESTROYING TOO SOON???");
        Destroy(gameObject);
    }


}
