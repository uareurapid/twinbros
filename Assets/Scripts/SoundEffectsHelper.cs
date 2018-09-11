using UnityEngine;
using System.Collections;

/// <summary>
/// Creating instance of sounds from code with no effort
/// </summary>
public class SoundEffectsHelper : MonoBehaviour
{
	
	/// <summary>
	/// Singleton
	/// </summary>
	private static SoundEffectsHelper instance;

	public AudioClip jumpSound;
	public AudioClip doubleJumpSound;
	public AudioClip burstBubbleSound;
	public AudioClip pickupCoinSound;
	public AudioClip hitDeadSound;
	public AudioClip waterSplashSound;
	public AudioClip checkpointSound;
	public AudioClip electricitySound;
	public AudioClip replaySound;
	public AudioClip settingsSound;
	public AudioClip countdownSound;
	public AudioClip powerupSound;
	public AudioClip landingSound;
	public AudioClip flipTimeSound;
	public AudioClip successSound;
	public AudioClip sloganMovieSound;
	public AudioClip teleportSound;
	public AudioClip moveSound;

	public AudioClip gameOverPanelSound;

	public AudioClip uiSettingsSound;

	public AudioClip largeExplosionSound;
	public AudioClip bounceSound;

	public AudioClip riseSound;

	public AudioClip jumpOnHeadSound;

		//todo credit Freesound.org - "Energy Weapon 001.wav" by DJ Chronos
		//Freesound.org - "Medium Explosion.wav" by ryansnook
		//Freesound.org - "Distant explosion.wav" by juskiddink
	//https://www.freesound.org/people/fins/sounds/146729/
	
	void Awake()
	{
		// Register the singleton
		//if (Instance != null)
		//{
		//	Debug.LogError("Multiple instances of SoundEffectsHelper!");
		//}
		//Instance = this;
		if(instance!=null) {
			Debug.Log("There is another instance sound effects running");
		}
		else {
			instance = this;
		}
		
		
	}


	public void PlayUiSettingsSound() {
		MakeSound(uiSettingsSound);
	}

	public void PlayGameOverPanelSlideSound() {
	  MakeSound(gameOverPanelSound);
	}

	public void PlayLargeExplosionSound()
	{
		MakeSound(largeExplosionSound);
	}

	public void PlayPickupCoinSound()
	{
		MakeSound(pickupCoinSound);
	}

	public void PlayBurstBubbleSound()
	{
		MakeSound(burstBubbleSound);
	}

	public void PlayMovieSloganSound()
	{
		MakeSound(sloganMovieSound);
	}

	public void PlayBounceSound()
	{
		MakeSound(bounceSound);
	}

	public void PlaySuccessSound()
	{
		MakeSound(successSound);
	}

	public void PlayJumpOnHeadSound()
	{
		MakeSound(jumpOnHeadSound);
	}

	public void PlayTeleportSound()
	{
		MakeSound(teleportSound);
	}

	public void PlayMoveSound()
	{
		MakeSound(moveSound);
	}

	public void PlayCountdownSound()
	{
		MakeSound(countdownSound);
	}

	public void PlayPowerupSound()
	{
		MakeSound(powerupSound);
	}

	public void PlayHitDeadSound()
	{
		MakeSound(hitDeadSound);
	}

	public void PlayJumpSound() {
		MakeSound(jumpSound);
	}

	public void PlayJRiseSound() {
		MakeSound(riseSound);
	}

	public void PlayDoubleJumpSound() {
		MakeSound(doubleJumpSound);
	}

	public void PlayLandingSound() {
		MakeSound(landingSound);
	}

	public void PlayWaterSplashSound()
	{
		MakeSound(waterSplashSound);
	}

	public void PlayReplaySound()
	{
		MakeSound(replaySound);
	}

	public void PlaySettingsSound()
	{
		MakeSound(settingsSound);
	}

	public void PlayCheckpointSound()
	{
		MakeSound(checkpointSound);
	}
	
	public void PlayElectricitySound()
	{
		MakeSound(electricitySound);
	}

	public void PlayFlipTimeSound()
	{
		MakeSound(flipTimeSound);
	}

	public static SoundEffectsHelper Instance {
		get
		{
			if (instance == null)
			{
				instance = (SoundEffectsHelper)FindObjectOfType(typeof(SoundEffectsHelper));
				if (instance == null)
					instance = (new GameObject("SoundEffectsHelper")).AddComponent<SoundEffectsHelper>();
			}
			return instance;
		}
	}
	
	/// <summary>
	/// Play a given sound
	/// </summary>
	/// <param name="originalClip"></param>
	private void MakeSound(AudioClip originalClip)
	{
		//play at the camera position, otherwise you cannot hear it
		AudioSource.PlayClipAtPoint(originalClip, Camera.main.transform.position,1f);
	}

	//initialization
	public void Start() {
				
	}
	//called at every run loop
	public void Update() {

	}

	//TODO need a sound for the pickups (could be from Fishy Bird Flapy)
}
