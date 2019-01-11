using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour {

	public SpawnPosition[] possibleSpawnPositions;
	public Transform[] possibleFruits;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnFruit() {
		
		if(possibleSpawnPositions.Length > 0 && possibleFruits.Length > 0)
		{
			SpawnPosition position = possibleSpawnPositions[UnityEngine.Random.Range(0, possibleSpawnPositions.Length - 1)];
			Transform fruit = possibleFruits[UnityEngine.Random.Range(0, possibleFruits.Length - 1)];
			instantiateTransform(fruit, position.transform.position);
		}
		
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
}
