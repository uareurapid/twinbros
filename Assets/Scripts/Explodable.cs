using UnityEngine;
using System.Collections;

public class Explodable : MonoBehaviour {

	public float explosionRadius = 15.0f;
	public string explosionLayer = "Player";
	public Transform impactPosition;
	//public float explosionForce = 600f;
    
	// Use this for initialization
	void Start () {
		if(impactPosition == null) {
			impactPosition = transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool ExplodePlayer() {
		Collider2D[] elements = Physics2D.OverlapCircleAll(impactPosition.position, explosionRadius, 1 << LayerMask.NameToLayer(explosionLayer));
		return elements.Length > 0;
	}


}
