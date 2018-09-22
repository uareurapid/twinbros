using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

	public bool isSurpriseBox = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D other) {
		Debug.Log("###################### COLLISION WITH THE BOXXX");
		//PlayerMovement move = other.gameObject.GetComponent<PlayerMovement>();

		//if(move!=null) {
		//	if(move.IsMovingLeft()) {
		//		Debug.Log("###################### WITH THE BOXXX LEFTTTTTTTTT");
		//		Rigidbody2D body = move.GetBody();
		//		move.transform.Translate(-body.velocity); 
		//	}
		//}

	}
}
