using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour, HandlePlayerCollision {

	public bool T;
	public bool I;
	public bool W;
	public bool N;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void OnTriggerEnter2D(Collider2D col)
    {
        PlayerMovement player = col.GetComponent<PlayerMovement>();
        if(player!=null) {
            HandleCollision(player);
        }
    }

    public void HandleCollision(PlayerMovement player)
    {
        Debug.Log("BONUS COLLISION");
        LevelManager manager = player.GetLevelManager();
        bool giveBonus = false;
        if (manager != null)
        {
            GUIManager GUI = manager.GetGUIManager();
            GameManagerScript managerScript = manager.GetGameManagerScript();

            if (GUI != null)
            {
                SoundEffectsHelper.Instance.PlayBonusHitSound();

                UnityEngine.UI.Image image = null;
                //do shit
                if (T)
                {
                    image = GUI.bonusImages[0];
                    managerScript.AddBonusLetter("T");
                }
                else if (W)
                {
                    image = GUI.bonusImages[1];
                    managerScript.AddBonusLetter("W");
                }
                else if (I)
                {
                    image = GUI.bonusImages[2];
                    managerScript.AddBonusLetter("I");
                }
                else if (N)
                {
                    image = GUI.bonusImages[3];
                    managerScript.AddBonusLetter("N");
                }

                if (image != null)
                {
                    //set full opacity
                    Color color = image.color;
                    color.a = 1f;

                    image.color = color;
                }


                //check if we have all the letters
                if (managerScript.ShouldGiveBonusMove())
                {
                    Debug.Log("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$ WILL GIVE $$$$$$$");

                    SoundEffectsHelper.Instance.PlayBonusUpgradeSound();
                    //hide it right away, cause it will take a few seconds until we can destroy it
                    GetComponent<Renderer>().enabled = false;
					GetComponent<Collider2D>().enabled = false;
                    managerScript.AddBonusMove();
                    giveBonus = true;
					manager.increaseMoves(1);
                    StartCoroutine(DisableBonusImages(2f, GUI));
                    //some effect
                } else
                {
                    Debug.Log("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$ NOT GIVE $$$$$$$");
                }

            }

            if (!giveBonus)
            {
                Destroy(gameObject);
            }

        }
        
    }
    
    IEnumerator DisableBonusImages(float delay, GUIManager GUI) 
    {

        yield return new WaitForSeconds(delay);
        GUI.RestoreBonusImagesOpacity();
        Destroy(gameObject);
    }
    
    public void HandleExitCollision(PlayerMovement player) {

    }
}
