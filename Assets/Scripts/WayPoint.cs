using UnityEngine;
using System.Collections;

public class WayPoint : MonoBehaviour
{
    public float waitSeconds = 0;
    public float speedOut = 0;
	//disable the collider on Start()
	public bool enableCollidersOnPlay = false;

	void Start() {
		Collider2D childCol = GetComponent<Collider2D>();
		if(childCol!=null) {
			childCol.enabled = enableCollidersOnPlay;
		}
	}
}
