using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : SpawnableItem {

	public bool isSurpriseBox = false;
	public Transform[] possibleSurprises;

	private bool collisionTop = false;
	private bool collisionBottom = false;
	private bool collisionLeft = false;
	private bool collisionRight = false;

	private PlayerMovement twinPlayer;

	private int numCollisions = 0;
	public int dropSurpriseAfterNumCollisions = 2;
	void Start()
	{
      numCollisions = 0;
	}
	// TODO use this to unlock the surprise
	public void FadeSurpriseBox(bool colUp, bool colRight, bool colDown, bool colLeft, PlayerMovement twin) {
		FadeSprite fade = GetComponent<FadeSprite>();
		numCollisions += 1;
		if(fade!=null && !fade.IsFadingInOrOut()) {
			fade.enabled = true;
			collisionTop = colUp;
			collisionLeft = colLeft;
			collisionRight = colRight;
			collisionBottom = colDown;
			twinPlayer = twin;
			fade.FadeSpriteNow(false, this);
		}
	}

	public void InstantiateSurprise() {

		if(possibleSurprises !=null && possibleSurprises.Length>0) {
			Transform t = instantiateTransform( possibleSurprises[Random.Range(0, possibleSurprises.Length - 1)], transform.position);
			t.gameObject.AddComponent<Destroyable>();
			AddToDestroyableList(t.gameObject.GetComponent<Destroyable>());
		}
		
	}

	private void AddToDestroyableList(Destroyable des) {

		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		if(scripts!=null) {

			LevelManager levelManager = scripts.GetComponent<LevelManager>();
			if(levelManager!=null) {
				levelManager.AddDestroyableObject(des);
			}
				
		}
	}

	public Collider2D GetColliderBox() {

		return GetComponent<Collider2D>();
	}

	public PlayerMovement GetTwin() {
		return twinPlayer;
	}

	private Transform instantiateTransform(Transform prefab, Vector3 position)
	{
		Transform newTransform = Instantiate(
			prefab,
			position,
			Quaternion.identity
			) as Transform;
		
		return newTransform;
	}

	public void FadeCompletedCallback() {
		if(twinPlayer!=null) {
			if(collisionTop) {
				twinPlayer.canMoveUp = true;
				twinPlayer.canMoveDown = true;
			}
			if(collisionBottom) {
				twinPlayer.canMoveDown = true;
				twinPlayer.canMoveUp = true;
			}
			if(collisionLeft) {
				twinPlayer.canMoveLeft = true;
				twinPlayer.canMoveRight = true;
			}
			if(collisionRight) {
				twinPlayer.canMoveRight = true;
				twinPlayer.canMoveLeft = true;
			}
		}
		if(numCollisions % dropSurpriseAfterNumCollisions == 0) {
			InstantiateSurprise();
			gameObject.SetActive(false);
		}
	}
}
