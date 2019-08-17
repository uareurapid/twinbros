using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

	public int level = 1;
	public Level nextLevel;
    public bool isBossLevel = false; //if boss level the moves are infinite

	private int bossesInLevel = 0; //number of bosses, usually should be 2

	public GameObject[] dieWithBoss; //destroy these when boss dies

	private Portal[] portals;
	
	// Use this for initialization
	void Start () {

		portals = GetComponentsInChildren<Portal>();
		//StartCoroutine(RotateObject());
		if(isBossLevel) {
			bossesInLevel = FindObjectsOfType<BossEnemy>().Length;
			DisablePortals();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool KilledAllBosses() {
		bossesInLevel--;
		return bossesInLevel <= 0;
	}

	public void KillDieWithBossObjects() {

		//all bosses are gone, detroy the other list
		foreach(GameObject obj in dieWithBoss) {

			SpecialEffectsHelper.Instance.PlayExplosionEffect(obj.transform.position);
			Destroy(obj);
		}
	}

	//enable all the portals on th elevel
	public void EnablePortals() {

		foreach(Portal portal in portals) {

			portal.EnablePortal();
		}	
	
	}

	//disable portals
	public void DisablePortals() {

		foreach(Portal portal in portals) {

			portal.DisablePortal();
		}

	}

	/*IEnumerator RotateObject()
     {
        while (true) {

        transform.Rotate(0.0f, 0.0f, 90.0f);

        yield return new WaitForSeconds(13);

       }
   }*/
}
