using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants : MonoBehaviour {

	//https://docs.unity3d.com/Manual/UnityIAPAppleConfiguration.html
	//no ads in game
	public const string PRODUCT_REMOVE_ADS = "product_remove_ads";
	//has 2 extra moves per level
	public const string PRODUCT_EXTRA_MOVES = "product_extra_moves";
	//can always revive/continue on every level it dies
	public const string PRODUCT_INFINITE_REVIVES = "product_infinite_revives";
}
