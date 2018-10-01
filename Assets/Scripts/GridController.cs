using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {

	public bool enableCollidersOnPlay = false;
	// Use this for initialization
	void Start () {

		foreach(Collider2D childCol in GetComponentsInChildren<Collider2D>()) {
			childCol.enabled = enableCollidersOnPlay;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
