using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {

	public float lifeTime = 3f;
	// Use this for initialization
	void Start () {
		if(lifeTime > 0) {
			Invoke("DestroyMe", lifeTime);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DestroyMe() {
		Destroy(gameObject);
	}
}
