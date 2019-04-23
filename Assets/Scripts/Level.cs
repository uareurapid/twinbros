using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

	public int level = 1;
	public Level nextLevel;
	// Use this for initialization
	void Start () {
		//StartCoroutine(RotateObject());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*IEnumerator RotateObject()
     {
        while (true) {

        transform.Rotate(0.0f, 0.0f, 90.0f);

        yield return new WaitForSeconds(13);

       }
   }*/
}
