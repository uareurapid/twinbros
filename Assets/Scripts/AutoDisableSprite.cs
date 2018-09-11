using UnityEngine;
using System.Collections;

public class AutoDisableSprite : MonoBehaviour
{

	SpriteRenderer ren;
	public float awakeTime = 2.0f; // 2 seconds before disable 
	private bool invoked = false;
	public bool reEnableOnStart = true;
	public float enableInterval = 8.0f;
	public bool alsoAffectCollider = false;

    //other object to enable/disable when the sprite is enabled/disabled
	public GameObject enableDisableOther;


	public bool alsoAutoDisableSound = false;

	private AudioSource audioClip;

	private Collider2D theCollider;
	// Use this for initialization
	void Start()
	{
		ren = GetComponent<SpriteRenderer>();
		theCollider = GetComponent<Collider2D>();

		if(alsoAutoDisableSound) {
			audioClip = GetComponent<AudioSource>();
		}
	}

	// Update is called once per frame
	void Update()
	{

		if (ren != null && ren.enabled && !invoked)
		{
			invoked = true;
			Invoke("DisableSprite", enableInterval);
		}

	}

	public void DisableSprite()
	{
		if (ren != null)
		{
			ren.enabled = false;
			if(alsoAffectCollider && theCollider!=null) {
				theCollider.enabled = false;
			}

			if(enableDisableOther!=null) {
				enableDisableOther.SetActive(false);
			}
			Invoke("EnableSprite",awakeTime);
		}
	}

	/*
	public void ReEnableSprite(float delay)
	{
		if (ren != null && enableInterval > 0f)
		{
			InvokeRepeating("EnableSprite", delay, enableInterval);
		}
	}*/

	void EnableSprite()
	{
		if (ren != null)
		{
			ren.enabled = true;
			invoked = false;
			if (alsoAffectCollider && theCollider != null)
			{
				theCollider.enabled = true;
			}

            if(enableDisableOther!=null) {
				enableDisableOther.SetActive(true);
			}


			if(alsoAutoDisableSound && audioClip!=null) {
				audioClip.Play();
			}
		}
	}



}
