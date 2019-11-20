using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICounter : MonoBehaviour
{
	public bool increase = false;

	private double counter = 0;
	private UnityEngine.UI.Text counterObj;
    // Start is called before the first frame update
    void Start()
    {
		counterObj = GetComponent<UnityEngine.UI.Text>();
		counter = double.Parse(counterObj.text);
		if(increase) {
			InvokeRepeating("IncreaseCounter", 1f, 1f);
		}
		else {
			InvokeRepeating("DecreaseCounter", 1f, 1f);
		}
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void IncreaseCounter() {
		if(counter < 99999999) {
			counter += 1;
		} else {
			counter = 0;
		}

		counterObj.text = counter.ToString();
	}

	void DecreaseCounter() {
		if(counter > 0) {
			counter -= 1;
		} else {
			counter = 99999999;
		}

		counterObj.text = counter.ToString();
	}
}
