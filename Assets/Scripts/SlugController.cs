using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugController : MonoBehaviour {

	public SpawnPosition[] nextPositions;
	public Transform spawnShadow;

	int nextPositionIndex = 0;
	public float delay = 3f;
	public float interval = 3f;
	private Vector3 nexPosition;
	// Use this for initialization
	void Start () {
		InvokeRepeating("SpawnOnNextposition", delay, interval);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SpawnOnNextposition() {
		//avoid errors if object not active
		if(gameObject.activeSelf) {
			nexPosition = nextPositions[nextPositionIndex++].transform.position;
			Transform shadow = instantiateTransform(spawnShadow, nexPosition);
			if(nextPositionIndex > nextPositions.Length - 1) {
				nextPositionIndex = 0;
			}
			StartCoroutine(MoveSlugIntoPosition(shadow));
		}
		
		
	}

	IEnumerator MoveSlugIntoPosition(Transform shadow) {
		yield return new WaitForSeconds(0.8f);
		Destroy(shadow.gameObject);
		transform.position = nexPosition;
	}

	private Transform instantiateTransform(Transform prefab, Vector3 position)
	{
		Transform newTransform = Instantiate(
			prefab,
			position,
			Quaternion.identity
			) as Transform;
		
		// Make sure it will be destroyed
		//Destroy(
		//	newTransform.gameObject,
		//	3f
		//	);
		
		return newTransform;
	}
}
