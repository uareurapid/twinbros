using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

	public int level = 1;
	public Level nextLevel;
    public bool isBossLevel = false; //if boss level the moves are infinite

	private int bossesInLevel = 0; //number of bosses, usually should be 2

	
	// Use this for initialization
	void Start () {
		//StartCoroutine(RotateObject());
		if(isBossLevel) {
			bossesInLevel = FindObjectsOfType<BossEnemy>().Length;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool KilledBoss() {
		bossesInLevel--;
		return bossesInLevel <= 0;
	}

	/*IEnumerator RotateObject()
     {
        while (true) {

        transform.Rotate(0.0f, 0.0f, 90.0f);

        yield return new WaitForSeconds(13);

       }
   }*/
}
