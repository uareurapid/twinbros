using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    public bool isOccupied = false;
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
        if (!isOccupied && col.gameObject.tag != null)
        {

            if (col.gameObject.CompareTag("Player"))
            {
                SpriteRenderer sprite = GetComponent<SpriteRenderer>();
                if (sprite != null)
                {
                    sprite.enabled = true;
                    //will keep
                    //StartCoroutine(HideSprite(sprite));
                }
            }
            else if(col.gameObject.GetComponent<GridTileDropper>()!=null)
            {

                GridTileDropper dropper = col.gameObject.GetComponent<GridTileDropper>();
        
                if(dropper!= null && dropper.CanDropNext())
                {
                    //random pick one
                    Transform gridTileOccupier = dropper.GetOccuppier();
                    if(gridTileOccupier!=null)
                    {
                        isOccupied = true;
                        dropper.Drop();
                        Transform theOne = SpecialEffectsHelper.Instance.instantiateTransformPublic(gridTileOccupier, transform.position, transform.rotation);
                        //it allows to un-occupy the tile again when the occupier gets destroyed
                        theOne.gameObject.GetComponent<GridTileOccupier>().SetParentGridTile(this);
                    }
                    
                } 
            }//if a box is standing here, then it is occupied
            //TODO double check
            else if(col.gameObject.CompareTag("Box"))
            {
                isOccupied = true;
            }

        }
    }
        

    /*IEnumerator HideSprite(SpriteRenderer sprite)
    {
        yield return new WaitForSeconds(0.25f);
        sprite.enabled = false;
    }*/
}
