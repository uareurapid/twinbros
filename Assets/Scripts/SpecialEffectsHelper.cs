using UnityEngine;
using System.Collections;


/// <summary>
/// Creating instance of particles from code with no effort
/// </summary>
public class SpecialEffectsHelper : MonoBehaviour
{
	/// <summary>
	/// Singleton
	/// </summary>
	private static SpecialEffectsHelper instance;

	public UnityEngine.UI.Image wrongPathImage;
	public UnityEngine.UI.Image skullImage;
	public Texture2D lightNingTexture;

    public Transform lightningBolt;

	public Transform explodeTransform;

	public Transform[] bloodSplatters;
    
    public Transform smokeTrailTransform;

	public ParticleSystem touchGroundEffect;
	
	public ParticleSystem doubleJumpEffect;
	public Transform dustEffect;
	public ParticleSystem burstBubbleEffect;
	public Transform startRespawnEffect;
	public ParticleSystem dieAndSplitEffect;
	public ParticleSystem waterSplashEffect;
	public ParticleSystem coinBurstEffect;
	public ParticleSystem parachuteReleaseEffect;
	public ParticleSystem laserExplosionEffect;

	public ParticleSystem fireworksEffect;
	public ParticleSystem levelDoneEffect;
	public ParticleSystem levelStartNameEffect;
	
	public Transform explosionEffect;

	public ParticleSystem electricityEffect;	
	public ParticleSystem checkpointEffect;	

	public ParticleSystem invincibilityEffect;
	public ParticleSystem speedupEffect;

	public ParticleSystem lavaSplashEffect;

	public ParticleSystem bloodBurstEffect;
	
	public ParticleSystem txt5PointsEffect;
	public ParticleSystem txt10PointsEffect;

	public ParticleSystem missedEffect;

	public ParticleSystem alienHitEffect;

	public Transform brakeEffect;

	public Transform inFlamesEffect;

	public ParticleSystem overheatEffect;

	public Transform riseEffectTransform;

    public ParticleSystem impactEffect;

	public ParticleSystem boxCollisionEffect;
	
	void Awake()
	{
		// Register the singleton
		if(instance!=null) {
			Debug.Log("There is another instance special effects running");
		}
		else {
			instance = this;
		}
		
	}
	
	/// <summary>
	/// Create an explosion at the given location
	/// </summary>
	/// <param name="position"></param>
	/*public void Explosion(Vector3 position)
	{
		// Smoke on the water
		instantiate(smokeEffect, position);
		
		// Tu tu tu, tu tu tudu
		
		// Fire in the sky
		instantiate(fireEffect, position);


		instantiate(boomExplosion, position);


	}*/

	public ParticleSystem Play5PointsTextEffect(Vector3 position) {
		return instantiate(txt5PointsEffect, position);
	}
	
	public ParticleSystem Play10PointsTextEffect(Vector3 position) {
		return instantiate(txt10PointsEffect, position);
	}

	public ParticleSystem PlayAlienHitEffect(Vector3 position)
	{
		return instantiate(alienHitEffect, position);
	}
	
	public ParticleSystem PlayDoubleJumpEffect(Vector3 position) {
		return instantiate(doubleJumpEffect, position);
	}

	public ParticleSystem PlayCheckpointEffect(Vector3 position) {
		return instantiate(checkpointEffect, position);
	}

	public ParticleSystem PlayFireworksEffect(Vector3 position)
	{
		return instantiate(fireworksEffect, position);
	}

	public ParticleSystem PlayLevelDoneEffect(Vector3 position)
	{
		return instantiate(levelDoneEffect, position);
	}

	public ParticleSystem PlayLevelStartNameEffect(Vector3 position)
	{
		return instantiate(levelStartNameEffect, position);
	}

	public ParticleSystem PlayBurstBubbleEffect(Vector3 position) {
		return instantiate(burstBubbleEffect, position);
	}
	
	public Transform PlayDustEffect(Vector3 position) {
		return instantiateTransform(dustEffect, position);
	}

	public ParticleSystem PlaySpeedupEffect(Vector3 position)
	{
		return instantiate(speedupEffect, position);
	}

	public ParticleSystem PlayBoxCollisionEffect(Vector3 position)
	{
		return instantiate(boxCollisionEffect, position);
	}

	public ParticleSystem PlayInvincibilityEffect(Vector3 position)
	{
		return instantiate(invincibilityEffect, position);
	}
	
	public Transform PlayStartRespawnEffect(Vector3 position) {
		return instantiateTransform(startRespawnEffect, position);
	}

	public ParticleSystem PlayTouchGroundEffect(Vector3 position) {
		return instantiate(touchGroundEffect, position);
	}

	public ParticleSystem PlayOverheatEffect(Vector3 position) {
		return instantiate(overheatEffect, position);
	}

	public ParticleSystem PlayBloodHurtEffect(Vector3 position) {
		return instantiate(bloodBurstEffect, position);
	}
	
	public ParticleSystem PlayParachuteReleaseEffect(Vector3 position) {
		return instantiate(parachuteReleaseEffect, position);
	}

	public ParticleSystem PlayCoinBurstEffect(Vector3 position) {
		return instantiate(coinBurstEffect, position);
	}
	
	public ParticleSystem PlayWaterSplashEffect(Vector3 position) {
		return instantiate(waterSplashEffect, position);
	}

	public ParticleSystem PlayLavaSplashEffect(Vector3 position) {
		return instantiate(lavaSplashEffect, position);
	}

	public ParticleSystem PlayMissedEffect(Vector3 position) {
		return instantiate(missedEffect, position);
	}
	
	public ParticleSystem PlayLaserExplosionEffect(Vector3 position) {
		return instantiate(laserExplosionEffect, position);
	}
	
	public Transform PlaySmokeTrailTransform(Vector3 position) {
		return instantiateTransform(smokeTrailTransform, position);
	}

	public ParticleSystem PlayDieAndSplitEffect(Vector3 position) {
		return instantiate(dieAndSplitEffect, position);
	}

	public ParticleSystem PlayElectricityEffect(Vector3 position) {
		return instantiate(electricityEffect, position);
	}
	
	public Transform PlayExplosionEffect(Vector3 position) {
		return instantiateTransform(explosionEffect, position);
	}

	public ParticleSystem PlayImpactEffect(Vector3 position) {
		return instantiate(impactEffect, position);
	}

    public Transform PlayEffect(Transform effect, Vector3 position) {
        return instantiateTransform(effect, position);
    }
	//TRANSFORMS

	public Transform PlayInFlamesEffect(Vector3 position) {
		return instantiateTransform(inFlamesEffect, position);
	}

	public Transform InstantiateExplosionTransform(Vector3 position) {
		return instantiateTransform(explodeTransform,position);
	}

	public Transform PlayBreakEffect(Vector3 position) {
		return instantiateTransform(brakeEffect, position);
	}

	public Transform PlayRiseEffect(Vector3 position) {
		return instantiateTransform(riseEffectTransform, position);
	}

	
	public Transform InstatiateBloodSplatterEffect(Vector3 position) {
		int pos = Random.Range(0, bloodSplatters.Length);
		return instantiateTransform(bloodSplatters[pos], position);
	}
	
	public static SpecialEffectsHelper Instance {
		get
		{
			if (instance == null)
			{
				instance = (SpecialEffectsHelper)FindObjectOfType(typeof(SpecialEffectsHelper));
				if (instance == null)
					instance = (new GameObject("SpecialEffectsHelper")).AddComponent<SpecialEffectsHelper>();
			}
			return instance;
		}
	}

	//public void ThunderboltEffect(Vector3 position) {
	//	instantiate(thunderbolt, position);
	//}
	
	/// <summary>
	/// Instantiate a Particle system from prefab
	/// </summary>
	/// <param name="prefab"></param>
	/// <returns></returns>
	private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
	{

		ParticleSystem newParticleSystem = Instantiate(
			prefab,
			position,
			Quaternion.identity
			) as ParticleSystem;

			/*if(prefab.tag!=null && prefab.CompareTag("Blood") ) {
				newParticleSystem.playbackSpeed = newParticleSystem.playbackSpeed * 4.0f;
			}*/

		
		// Make sure it will be destroyed
		/*Destroy(
			newParticleSystem.gameObject,
			newParticleSystem.startLifetime
			);*/
		
		return newParticleSystem;
	}

	private ParticleSystem instantiateWithRotation(ParticleSystem prefab, Vector3 position, Quaternion rotation)
	{

		ParticleSystem newParticleSystem = Instantiate(
			prefab,
			position,
			rotation
			) as ParticleSystem;

			/*if(prefab.tag!=null && prefab.CompareTag("Blood") ) {
				newParticleSystem.playbackSpeed = newParticleSystem.playbackSpeed * 4.0f;
			}*/

		
		// Make sure it will be destroyed
		/*Destroy(
			newParticleSystem.gameObject,
			newParticleSystem.startLifetime
			);*/
		
		return newParticleSystem;
	}
	
	private Transform instantiateTransform(Transform prefab, Vector3 position)
	{
		Transform newTransform = Instantiate(
			prefab,
			position,
			Quaternion.identity
			) as Transform;
		
		// Make sure it will be destroyed
		//Destroy(
		//	newTransform.gameObject,
		//	3f
		//	);
		
		return newTransform;
	}

	private GameObject instantiateGameObject(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		GameObject newTransform = Instantiate(
			prefab,
			position,
			rotation
			) as GameObject;
		
		return newTransform;
	}

	/*public void ProduceLightning(float speed) {
		FadeInOut fade = Camera.main.GetComponent<FadeInOut>();
		if (fade != null)
		{
			fade.enabled = true;//TODO check
			fade.SetNewFadeTexture(lightNingTexture, speed);
			fade.FadeIn();
			Invoke("ResetFadeTexture", 2f);
		}
        //if(lightningBolt!=null){
		//	lightningBolt.gameObject.SetActive(true);
		//	Invoke("hideIt", 0.5f);
		//}
	}*/

    //void hideIt(){
    //  if(lightningBolt!=null){
	//		lightningBolt.gameObject.SetActive(false);
	//  }
    //}

	/*void ResetFadeTexture() {
		FadeInOut fade = Camera.main.GetComponent<FadeInOut>();
		if (fade != null)
		{
			fade.ResetFadeTexture();
		}
			
			
	}*/

	/*public void ShowWrongPathImage(int remainingAttempts) {
		if(wrongPathImage!=null && skullImage!=null) {

			skullImage.enabled = true;
			wrongPathImage.enabled = true;

			GUIManager.Instance.ShowNumWrongPaths(remainingAttempts);

			AutoRotate rotate = wrongPathImage.GetComponent<AutoRotate>();
			if(rotate!=null) {
				rotate.enabled = true;
			}
			StartCoroutine(HideWrongPathImage(rotate));
		}
		
	}

	IEnumerator HideWrongPathImage(AutoRotate rotate) {
		yield return new WaitForSeconds(1.0f);
		wrongPathImage.enabled = false;
		skullImage.enabled = false;
		GUIManager.Instance.HideNumWrongPaths();
		if(rotate!=null) {
			rotate.enabled = false;
		}
	}*/
	
	//Note: Because we can have multiple particles in the scene at the same time, 
	//we are forced to create a new prefab each time. 
	//If we were sure that only one system was used at a time, 
	//we would have kept the reference and use the same everytime.

}
