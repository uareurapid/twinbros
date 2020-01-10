using UnityEngine;
using System.Collections;

public class WayPoint : MonoBehaviour
{
    public float waitSeconds = 0;
    public float speedOut = 0;
	//disable the collider on Start()
	public bool enableCollidersOnPlay = false;
    //instead of moving to the next
    public bool isDirectTeleport = false;
    //if tru it will hidde the sprite when it passes by
    public bool isHiddeSprite = false;
    //if true it will show sprite as it passes by
    public bool isShowSprite = true;
    //flip x axis
    public bool isRevertSprite = false;

    public Transform outEffect; //play some effect on exit
	void Start() {
		Collider2D childCol = GetComponent<Collider2D>();
		if(childCol!=null) {
			childCol.enabled = enableCollidersOnPlay;
		}
	}

	/*void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 1);
    }*/
}
