using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleOnScreen : MonoBehaviour {

	private bool isVisible = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   //not called on UI canvas elements
	void OnBecameInvisible() {
        isVisible = false;
    }
    void OnBecameVisible() {
        isVisible = true;
    }

	public bool IsVisibleOnScreen() {

		return isVisible;
	}

	public bool IsRectTransformVisibleOnScreen() {

		return RendererExtensions.IsFullyVisibleFrom(GetComponent<RectTransform>(), Camera.main);
	}
}
