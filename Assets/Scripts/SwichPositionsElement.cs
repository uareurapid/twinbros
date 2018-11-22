using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwichPositionsElement : MonoBehaviour {

	public SpawnPosition[] possibleSpawnPositions;
	public float delay = 1f;
	public float interval = 4f;
	public bool randomize = true;
	private int lastPosition = 0;

	// Use this for initialization
	void Start () {
		if(possibleSpawnPositions!=null && possibleSpawnPositions.Length>0) {

			InvokeRepeating("CalculateRandomNexPosition",delay,interval);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CalculateRandomNexPosition()
	{

		if (randomize)
		{
			int newPosition = Random.Range(0, possibleSpawnPositions.Length);
			transform.position = possibleSpawnPositions[newPosition].transform.position;
		}
		else {
			transform.position = possibleSpawnPositions[lastPosition].transform.position;
			lastPosition++;
			if(lastPosition > possibleSpawnPositions.Length-1) {
				lastPosition = 0;
			}
		}
	}
		
		

}
