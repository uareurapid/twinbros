using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {

	public bool enableCollidersOnPlay = false;
	// Use this for initialization
	void Start () {

		foreach(Collider2D childCol in GetComponentsInChildren<Collider2D>()) {
            if(!childCol.isTrigger)
            {
                childCol.enabled = enableCollidersOnPlay;//do not touch the trigger ones
            }
			
		}

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
