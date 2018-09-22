using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {

	public float lifeTime = 3f;
	// Use this for initialization
	void Start () {
		Invoke("DestroyMe", lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void DestroyMe() {
		Destroy(gameObject);
	}
}
