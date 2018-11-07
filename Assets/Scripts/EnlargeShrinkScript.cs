using UnityEngine;
using System.Collections;

public class EnlargeShrinkScript : MonoBehaviour {


    public bool enlarge = true;
	public bool doBoth = false; //one after the other?
								//otherwise shrink

	public float percentage = 25f;
	public float speed = 0.03f;
	// Use this for initialization
	Vector3 currentScale;

	public bool isText = false;
	UnityEngine.UI.Text textElement;
	public int fontDiff = 4;
	int originalFontSize = 0;

	private Vector3 initialScale;	
	void Start () {
		initialScale = gameObject.transform.localScale;
		currentScale = initialScale;
		if(isText) {
			textElement = GetComponent<UnityEngine.UI.Text>();
			originalFontSize = textElement.fontSize;
		}
	}
	
	// Update is called once per frame
	void Update () {

	if(isText && textElement!=null) {
 		if(enlarge) {
			if(textElement.fontSize < originalFontSize + fontDiff) {
				 Enlarge();
			}
			else if(doBoth && enlarge) {
				enlarge = false;
			}
		}
		else {
			if(textElement.fontSize > originalFontSize - fontDiff) {
					Shrink();
			}
			else if(doBoth && !enlarge) {
					enlarge = true;
			}
		}

	}
	else {
		currentScale = gameObject.transform.localScale;
		
			  if(enlarge){
					//enlarge
					if (currentScale.x < initialScale.x * (1.0f + (percentage/100f) ) ) {
					Enlarge();
				 }
				 else if(doBoth && enlarge){
					enlarge = false;
					//Shrink();
				 }
				
			  }
			  else {
				//shrink
				if(currentScale.x > initialScale.x ) {
					Shrink();
				}
				else if(doBoth && !enlarge) {
					enlarge = true;
					//Enlarge();
				}
				
			    
			  }
		
			  gameObject.transform.localScale = currentScale;
		}

	  
	   
	  
	}

	void Enlarge() {

	 if(isText) {
			textElement.fontSize += 1;
	 }
	 else {
		currentScale.x = currentScale.x + (currentScale.x * Time.deltaTime) * speed;
	 	currentScale.y = currentScale.y + (currentScale.y * Time.deltaTime) * speed;
	 }
	 
	}

	void Shrink() {
	  if(isText) {
		textElement.fontSize -= 1;
	  }
	  else {
		currentScale.x = currentScale.x - (currentScale.x * Time.deltaTime) * speed;
	  	currentScale.y = currentScale.y - (currentScale.y * Time.deltaTime) * speed;
	  }
	  
	}
}
