using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    public float speed = 3.0f;

    //public bool moveOnStartup = false;
    //public Vector2 moveDirection;
	// Use this for initialization
	void Start () {
		//if(moveOnStartup && moveDirection != null) {
        //    StartMoving(moveDirection);
        //}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void StartMoving(Vector2 direction) {
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

}
