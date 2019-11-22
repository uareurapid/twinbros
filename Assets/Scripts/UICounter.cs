using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICounter : MonoBehaviour
{
	public bool increase = false;
    public int step = 5; // how much to increase/decrease on each call?
	private double counter = 0;
    public int counterMax = 99999999;
    public int counterMin = 0;
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
		if(counter < counterMax) {
			counter += step;
		} else {
			counter = counterMin;
		}

		counterObj.text = counter.ToString();
	}

	void DecreaseCounter() {
		if(counter > counterMin) {
			counter -= step;
		} else {
			counter = counterMax;
		}

		counterObj.text = counter.ToString();
	}
}
