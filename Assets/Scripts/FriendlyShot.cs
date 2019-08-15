using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyShot : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        //if is normal enemy destroy it?
        BossEnemy enemy = collision.gameObject.GetComponent<BossEnemy>();
        if(enemy!=null) {
            enemy.DecreaseLife();
        }

        Destroy(gameObject); //destroy the shot
    }
    
}
