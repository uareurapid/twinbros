using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb: SpawnableItem, HandlePlayerCollision, ResetBehaviourScript { 
//we do not add the behaviour but use the interface anyway

	public float delayBeforeBurn = 1.0f;
	public float delayBeforeExplosion = 2.0f;
	public bool isLightUp = false;//acendida
	public bool isBurning = false;
	Animator anim;

	private Explodable explodeAction;


	private bool originallyLightUp = false;
	private bool originallyBurning = false;
	private AnimationController bombController;
	private Sprite initialSprite;

	// Use this for initialization
	void Start () {
		
		anim = GetComponentInChildren<Animator>();
		explodeAction = GetComponent<Explodable>();
		//get the initial values for reset after player death
		bombController = GetComponentInChildren<AnimationController>();

		initialSprite = GetComponentInChildren<SpriteRenderer>().sprite;

		originallyLightUp = isLightUp;
		originallyBurning = isBurning;
	}
	
	public void HandleExitCollision(PlayerMovement player) {

	}

	public void HandleCollision(PlayerMovement movement)
	{
		Debug.Log("#### HandleBombCollisions CALLED IN BOMB!!!");
		
		if(isBurning) {
			//explode
			StartCoroutine(ExplodeBombAfterBurn(movement));
		}
		else if(isLightUp) {

			//burn
			StartCoroutine(BurnBombAfterFuse());
			//explode
			StartCoroutine(ExplodeBombAfterBurn(movement));
			
		}
		if(!isLightUp && !isBurning) {

			//fuse
			StartBombFuse();
			//burn
			StartCoroutine(BurnBombAfterFuse());
			//explode
			StartCoroutine(ExplodeBombAfterBurn(movement));
		}
		
		
	}

	void StartBombFuse() {
		anim.SetBool("burn", false);
		anim.SetBool("fuse", true);
	}

	IEnumerator BurnBombAfterFuse() {
		yield return new WaitForSeconds(delayBeforeBurn);
		anim.SetBool("burn", true);
		anim.SetBool("fuse", false);
	}

	void StartBombRedAnimation() {
		anim.SetBool("burn", false);
		anim.SetBool("fuse", false);
		anim.SetBool("red", true);
	}

	IEnumerator ExplodeBombAfterBurn(PlayerMovement movement)
	{
		StartBombRedAnimation();
		yield return new WaitForSeconds(delayBeforeExplosion);
		SoundEffectsHelper.Instance.PlayLargeExplosionSound();
		SpecialEffectsHelper.Instance.PlayExplosionEffect(transform.position);

		//only if it is in range of the explosion
		if (explodeAction.ExplodePlayer()) {
			movement.ShowBurnSpriteAnimation();
			//movement.ShowElectrocutedSpriteAnimation();
		}

		gameObject.SetActive(false);
        //also disable collider
        Collider2D col = GetComponent<Collider2D>();
        if(col!=null)
        {
            col.enabled = false;
        }
		
		GetComponentInChildren<SpriteRenderer>().sprite = initialSprite;

		//tile position is free now
		if(tileOccupied!=null) {
			tileOccupied.isOccupied = false;
		}
		
		
	}

	//called from ResetActiveState TODO find a better way, still not working all the time!
	public void ResetOriginalBehaviour() {
		isLightUp = originallyLightUp;
		isBurning = originallyBurning;

        //also re-enable collider
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = true;
        }

        GetComponentInChildren<SpriteRenderer>().sprite = initialSprite;

		//restore also the controller parameters
		Dictionary <string,bool> parameters = bombController.GetOriginalParameters();
	 	foreach( KeyValuePair<string, bool> keyValue in parameters) {
			bombController.SetAnimationParameter(keyValue.Key, keyValue.Value);
		}

		//the place where the bomb stands is occupied untile it explodes
		if(tileOccupied!=null) {
			tileOccupied.isOccupied = true;
		}
	}

}
