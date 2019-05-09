using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideSprite : MonoBehaviour {

    SpriteRenderer rend;
    public float delay = 0;
    public float interval = 0.1f;
    public bool enabled = true;
	// Use this for initialization
	void Start () {
        rend = GetComponent<SpriteRenderer>();
        InvokeRepeating("EnableDisable", delay, interval);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void EnableDisable() {
        enabled = !enabled;
        rend.enabled = enabled;
    }
}
