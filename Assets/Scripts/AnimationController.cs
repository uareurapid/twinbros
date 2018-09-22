using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

	public string param1Name = "";
	public string param2Name = "";
	//public string param3Name = "";

	public bool param1Value = false;
	public bool param2Value = false;
	//public bool param3Value = false;

	Animator anim;

	void Awake() {

		anim = GetComponent<Animator>();
		anim.SetBool(param1Name, param1Value);
		anim.SetBool(param2Name, param2Value);
		anim.enabled = true;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
