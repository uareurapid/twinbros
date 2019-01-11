using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpawnableItem : MonoBehaviour {

	public SpawnPosition[] possibleSpawnPositions;
	// Use this for initialization
	void Start () {
		if(possibleSpawnPositions!=null && possibleSpawnPositions.Length>0) {

			CalculateRandomSpawnPosition();
		}
	}

	void CalculateRandomSpawnPosition() {

		try
		{
			if (transform != null && possibleSpawnPositions.Length > 0)
			{
				transform.position = possibleSpawnPositions[UnityEngine.Random.Range(0, possibleSpawnPositions.Length - 1)].transform.position;
		}
		}catch(Exception e) {
			Debug.Log("GOT ERROR HERE " + gameObject.name);
		}
		
	}
	// Update is called once per frame
	void Update () {
		
	}
}
