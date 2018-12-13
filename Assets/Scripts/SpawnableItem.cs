using System.Collections;
using System.Collections.Generic;
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
		if(transform !=null) {
			transform.position = possibleSpawnPositions[Random.Range(0, possibleSpawnPositions.Length)].transform.position;
		}
		
	}
	// Update is called once per frame
	void Update () {
		
	}
}
