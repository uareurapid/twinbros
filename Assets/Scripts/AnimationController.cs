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

	//save the initial parameters state
	Dictionary<string, bool> initialParameters;
	Animator anim;

	void Awake() {

		anim = GetComponent<Animator>();

		initialParameters = new Dictionary<string, bool>();
		anim.SetBool(param1Name, param1Value);
		anim.SetBool(param2Name, param2Value);

		//save the state
		initialParameters.Add(param1Name, param1Value);
		initialParameters.Add(param2Name, param2Value);
		
		anim.enabled = true;
		
	}
	// Use this for initialization
	void Start () {
		
	}

	//restore the initial params
	public void SetAnimationParameter(string name, bool value) {

		anim.SetBool(name, value);
	}

	//for reseting
	public Dictionary<string, bool> GetOriginalParameters() {
		return initialParameters;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
