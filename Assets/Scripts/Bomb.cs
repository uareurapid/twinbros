using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb: MonoBehaviour {

	public float delayBeforeBurn = 1.0f;
	public float delayBeforeExplosion = 2.0f;
	public bool isLightUp = false;//acendida
	public bool isBurning = false;
	Animator anim;

	private Explodable explodeAction;

	// Use this for initialization
	void Start () {
		anim = GetComponentInChildren<Animator>();
		explodeAction = GetComponent<Explodable>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void HandlePlayerCollision(PlayerMovement movement)
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

	IEnumerator ExplodeBombAfterBurn(PlayerMovement movement)
	{
		yield return new WaitForSeconds(delayBeforeExplosion);
		SpecialEffectsHelper.Instance.PlayExplosionEffect(transform.position);
		Destroy(gameObject);

		//only if it is in range of the explosion
		if (explodeAction.ExplodePlayer()) {
			movement.ShowBurnSpriteAnimation();
		}
		
		
	}
}
