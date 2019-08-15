using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour {

    public int initialLife = 10;
    private int currentLife = 0;
	// Use this for initialization
	void Start () {
        currentLife = initialLife;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    
    public void DecreaseLife() {
    
        if(currentLife > 0) {

            currentLife--;
        }
        
        if(currentLife <= 0) {
            Destroy(gameObject); //dead
        }
    }
}
