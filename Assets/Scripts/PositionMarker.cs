using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionMarker : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Collider2D col = GetComponent<Collider2D>();
		if(col!=null) {
			col.enabled = false;
		}
		SpriteRenderer spr = GetComponent<SpriteRenderer>();
		if(spr!=null) {
			spr.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
