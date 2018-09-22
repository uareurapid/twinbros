using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBox : MonoBehaviour {

	public LevelManager levelmanager;
	public bool killPlayerOnTouch = true; //if not just dcrease a move or something

	private Tile associatedTile;
	// Use this for initialization
	void Start () {
		associatedTile = GetComponent<Tile>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Tile GetTile() {

		return associatedTile;
	}

	public void HandlePlayerCollision() {
		if(levelmanager==null) {
			GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
			levelmanager = scripts.GetComponent<LevelManager>();

		}

		if(killPlayerOnTouch  ) {
			//TODO i am killing disregard of the direction  of movmenet CHECKE!!!!!!
			levelmanager.KillPlayer();
		}
	}

}
