using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : SpawnableItem, HandlePlayerCollision {

    //TODO the surprise must be destroyed when the level restarts, after player dies
	public bool isSurpriseBox = false;
	public Transform[] possibleSurprises;
    
    public bool isShootingBox = false; //this can fire a projectile

	private PlayerMovement twinPlayer;

	private int numCollisions = 0;
	public int dropSurpriseAfterNumCollisions = 2;
	void Start()
	{
      numCollisions = 0;
	}
	// TODO use this to unlock the surprise
	public void FadeSurpriseBox(PlayerMovement twin) {
		FadeSprite fade = GetComponent<FadeSprite>();
		numCollisions += 1;
		if(fade!=null && !fade.IsFadingInOrOut()) {
			fade.enabled = true;
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

	public void HandleExitCollision(PlayerMovement player) {

	}

    public void HandleCollision(PlayerMovement player)
    {
     
        //TODO there are game objects that are tagged box, but do not have the component CHECK!!!
        if (isSurpriseBox)
        {

            FadeSurpriseBox(player);
        } else if(isShootingBox) {

            ShootingBox shooting = GetComponent<ShootingBox>();
            shooting.HandleCollision(player);
            
        }
        
        SpecialEffectsHelper.Instance.PlayBoxCollisionEffect(player.transform.position);

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
		Debug.Log("FADE COMPLETED: " + numCollisions + "%=" + (numCollisions % dropSurpriseAfterNumCollisions));
		if(numCollisions > 0 && (numCollisions % dropSurpriseAfterNumCollisions == 0) ) {
			InstantiateSurprise();
			FadeSprite fade = GetComponent<FadeSprite>();
			gameObject.SetActive(false);
			if(fade!=null) {
				fade.ResetSprite();
			}
			numCollisions = 0;
		}
	}
}
