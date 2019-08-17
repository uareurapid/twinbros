using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour {

    public int initialLife = 10;
    private int currentLife = 0;

	public Level level; //the level the boss belongs to
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
			TakeDamage();
        }
        
        if(currentLife <= 0) {

			NotifyBossDeath();
			Destroy(gameObject);
        }
    }

	void TakeDamage() {

		BlinkSpriteScript blink = GetComponentInChildren<BlinkSpriteScript>();
		if(blink!=null) {
			blink.enabled = true;
			StartCoroutine(RestoreAfterHit());
		}
		
	}

	IEnumerator RestoreAfterHit() {
		yield return new WaitForSeconds(1.5f);
		BlinkSpriteScript blink = GetComponentInChildren<BlinkSpriteScript>();
		if(blink!=null) {
			blink.StopBlinkingWithOptions(true);
			yield return new WaitForSeconds(0.5f);
			blink.enabled = false;
		}
	}

	//one of the bosses died	
	public void NotifyBossDeath() {

		//check if all are gone now
		if(level.KilledAllBosses()) {
			//destroy fence, make explosion
			level.KillDieWithBossObjects();
			//enable portals again
			level.EnablePortals();
			LevelManager levelManager = FindObjectOfType<LevelManager>();
			levelManager.LevelCleared();
		}
	}
}
