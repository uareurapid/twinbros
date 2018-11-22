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

	public const string TXT_LEVEL_KEY = "level";
	public const string TXT_REMOVE_ADS_KEY = "buy_remove_ads";
	public const string TXT_EXTRA_MOVES_KEY = "buy_extra_moves";
	public const string TXT_INFINITE_REVIVES_KEY = "buy_infinite_revives";

	public const string LEADERBOARD_ID = "twins_high_cores";

	public const string ACHIEVEMENT_STAGE_1_ID = "twins_stage_1";
	public const string ACHIEVEMENT_STAGE_2_ID = "twins_stage_2";
	public const string ACHIEVEMENT_STAGE_3_ID = "twins_stage_3";
	public const string ACHIEVEMENT_STAGE_4_ID = "twins_stage_4";
	public const string ACHIEVEMENT_STAGE_5_ID = "twins_stage_5";
	public const string ACHIEVEMENT_STAGE_GENERIC_ID = "twins_stage_";

}
