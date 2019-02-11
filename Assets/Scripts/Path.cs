using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {

	public Tile[] tiles;
	public bool showPath = false;
	public GameObject tileMarker;
	// Use this for initialization
	void Start () {
		if(showPath) {
			ShowTilesPath();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowTilesPath() {
		/*foreach(Tile t in tiles) {
			t.gameObject.AddComponent<>();
		}*/
	}
}
