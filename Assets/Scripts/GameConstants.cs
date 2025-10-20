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

	// for mobile (IOS and Android)
	//public const string LEADERBOARD_ID = "grp.twins_high_cores";
	// for webgl
	public const string LEADERBOARD_ID = "twins_high_cores";
	public const string CURRENT_SCORE = "twins_current_cores";


	public const string ACHIEVEMENT_STAGE_STR = "achievement_stage_"; // append level num
	public const string ACHIEVEMENT_STAGE_1_ID = "grp.twins_stage_1";
	public const string ACHIEVEMENT_STAGE_2_ID = "grp.twins_stage_2";
	public const string ACHIEVEMENT_STAGE_3_ID = "grp.twins_stage_3";
	public const string ACHIEVEMENT_STAGE_4_ID = "grp.twins_stage_4";
	public const string ACHIEVEMENT_STAGE_5_ID = "grp.twins_stage_5";
	public const string ACHIEVEMENT_STAGE_GENERIC_ID = "grp.twins_stage_";

	public const string LEFT_DIRECTION = "left";
	public const string RIGHT_DIRECTION = "right";
	public const string UP_DIRECTION = "up";
	public const string DOWN_DIRECTION = "down";

    public const string HAS_SHOWN_TUTORIAL = "has_shown_tutorial";


    public const string HAS_BONUS_MOVE = "has_bonus_move";

    public const string SHOOT_DIRECTION_LEFT = "left";
    public const string SHOOT_DIRECTION_RIGHT = "right";
    public const string SHOOT_DIRECTION_UP = "up";
	public const string SHOOT_DIRECTION_DOWN = "down";

}
