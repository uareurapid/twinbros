using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCollider : MonoBehaviour {


	public bool isLeft = false;
	public bool isRight = false;
	public bool isTop = false;
	public bool isBottom = false;

	private Movement movement;

	public Level associatedLevel; //can only move on the associated Level
	public LevelManager levelManager;
	// Use this for initialization
	void Start () {
		movement = GetComponentInParent<Movement>();
		associatedLevel = GetComponentInParent<Level>();
		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		levelManager = scripts.GetComponent<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//only affects if on the same level
		if(levelManager.currentLevel == associatedLevel.level) {
				//portal collision
				if(other.transform.CompareTag("Portal")) {
					Portal portal = other.GetComponent<Portal>();
					portal.MoveToNextLevel();
				}
				else {
		
						if(isLeft) {
							movement.collidedLeft();
						}
						else if(isRight) {
							movement.collidedRight();
						}
						else if(isTop) {
							movement.collidedTop();
						}
						else if(isBottom) {
							movement.collidedBottom();
						}
				}
		}

		
		
		
	}
}
