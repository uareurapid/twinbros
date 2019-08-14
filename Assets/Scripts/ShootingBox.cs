using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBox : MonoBehaviour {

    public string shootDirection = GameConstants.SHOOT_DIRECTION_LEFT;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void Shoot() {
    
        if(shootDirection.Equals(GameConstants.SHOOT_DIRECTION_LEFT)) {
            Debug.Log("shoot left");
        }
        else if(shootDirection.Equals(GameConstants.SHOOT_DIRECTION_RIGHT)) {
            Debug.Log("shoot right");
        }
        else if(shootDirection.Equals(GameConstants.SHOOT_DIRECTION_UP)) {
            Debug.Log("shoot up");
        }
        else if(shootDirection.Equals(GameConstants.SHOOT_DIRECTION_DOWN)) {
             Debug.Log("shoot down");
        }
    }
}
