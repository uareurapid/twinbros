using UnityEngine;
using System.Collections;

public class BlinkSpriteScript : MonoBehaviour {

    public float delay = 0f;
    public float blinkInterval = 0f;
   
	public bool isUIImage = false;
	private bool canBlink = true;
	public bool isEnabled = true;

    public bool onlyChangeColor = false;
    public Color color;

    private Color originalColor;
    // Use this for initialization

    private bool stop = false;
	void Start () {

	}
	
	// Update is called once per frame
	void OnEnable () {

        isEnabled = true;
        if(onlyChangeColor) {
            if (isUIImage)
            {
               originalColor = GetComponent<UnityEngine.UI.Image>().color;

            }
            else {
            
              originalColor = GetComponent<SpriteRenderer>().color;
            }       }
        stop = false;    
		InvokeRepeating("Blink",delay,blinkInterval);
	}

	public void Blink() {
        if(!stop) {
        
            isEnabled = !isEnabled;
            if(isUIImage) {
                UnityEngine.UI.Image image = GetComponent<UnityEngine.UI.Image>();
                if(onlyChangeColor) {
                    if(image.color == originalColor) {
                        image.color = color;
                    } else {
                        image.color = originalColor;
                    }
                }
                else {
                    image.enabled = isEnabled;
                }
                
            }
            else {
                SpriteRenderer image = GetComponent<SpriteRenderer>();
                if(onlyChangeColor) {
                    if(image.color == originalColor) {
                        image.color = color;
                    } else {
                        image.color = originalColor;
                    }
                } else {
                   image.enabled = isEnabled;
                }
                
            }
        
        } 
		
	}
    
    public void StopBlinking() {

        stop = true;
        CancelInvoke("Blink");
        if (isUIImage)
        {
            GetComponent<UnityEngine.UI.Image>().color = originalColor;
        } else {

            GetComponent<SpriteRenderer>().color = originalColor;
        }
    }


}
