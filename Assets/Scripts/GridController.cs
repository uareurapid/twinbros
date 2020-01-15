using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {

	public bool enableCollidersOnPlay = false;
	// Use this for initialization
	void Start () {

		foreach(Collider2D childCol in GetComponentsInChildren<Collider2D>()) {
            //do not touch the trigger ones
            if (!childCol.isTrigger)
            {
                childCol.enabled = enableCollidersOnPlay;
                if(childCol.GetComponent<GridTile>()!=null)
                {
                    Debug.Log("WHAHAHAHAHAHHAHAHAH$$$$$$");
                }
            }
			
		}

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
