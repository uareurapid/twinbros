using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class FixedScroll : MonoBehaviour 
{
	public float theScrollSpeed = 0.025f;

	public Transform theStage;

	void Start () 
	{

	}
	
	void Update ()
	{
        theStage.position = new Vector3 ( theStage.position.x, theStage.position.y + theScrollSpeed, theStage.position.z );
	}
}


*/




public class FixedScroll : MonoBehaviour
{

    public float scrollSpeed;
    public float tileSizeZ;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
        transform.position = startPosition + Vector3.up * newPosition;
    }
}