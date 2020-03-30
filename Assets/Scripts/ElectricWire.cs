using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricWire : SpawnableItem {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void ElectrocutePlayer(PlayerMovement movement) {

		SoundEffectsHelper.Instance.PlayElectricitySound();
		movement.ShowElectrocutedSpriteAnimation();
		SpecialEffectsHelper.Instance.PlayElectricityEffect(transform.position);
	}
}
