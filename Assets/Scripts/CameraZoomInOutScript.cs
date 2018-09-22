using UnityEngine;
using System.Collections;

public class CameraZoomInOutScript : MonoBehaviour {

    public Transform target;
	public float cameraMin = 5f;
	public float cameraMax = 10f;

	public bool isCameraZoomingIn = false;
	public bool isCameraZoomingOut = false;

	private bool isMovementComplete = true;
	// Use this for initialization

	//Vector3 targetPosition;
	Vector3 velocity = Vector3.zero;
	public float timeToReachTarget = 4f; //seconds

	private float speed = 1.0f;
    private float cameraOriginalOrthographicSize = 0f;
    private Vector3 cameraOriginalPosition;
    //minimum distance between the camera and the object
    public float minDistance = 1f;

    public bool showMessageOnZoom = false;
	public string messageKey = "What is this thing in the back???"; 

	Texture2D helpMeTexture;

    private GUISkin skin;

	public bool canMove = false;


	Vector3 damagerVelocity = Vector3.zero;
	public int currentLevel = 0;
	public int nextLevel = 0;
	public int previousLevel = 0;
	public int numLevels = 6;


	void Start () {

	  Time.timeScale = 1.0f;
	  // save the current values
	  cameraOriginalOrthographicSize = Camera.main.orthographicSize;
	  cameraOriginalPosition = Camera.main.transform.position; //or local position??
	  //skin = Resources.Load("GUISkin") as GUISkin;
	  //targetPosition = target.position;
	  //targetPosition.z = cameraOriginalPosition.z;
	  isMovementComplete = true;
	  canMove = false;
	  
						
	}

	void Awake() {
		//GUIResolutionHelper.Instance.CheckScreenResolution();
	}

	public void EnableMovement(bool move) {
		canMove = move;
	}


	void FixedUpdate() {
	
		
	}

	public void MoveToNextLevel (){

		if (isMovementComplete) {

			isMovementComplete = false;
			canMove = true;

			if (currentLevel + 1 <= numLevels)
				nextLevel = currentLevel +1;
			else//stay in the same level
				nextLevel = currentLevel;
			
			UpdatePosition (nextLevel);

		}


	}

	public void MoveToPreviousLevel() {


		if (isMovementComplete) {
		
			isMovementComplete = false;
			canMove = true;

			if (currentLevel - 1 >= 0)
				previousLevel = currentLevel-1;
			else//stay in the same level
				previousLevel = currentLevel;

			UpdatePosition (previousLevel);

		}
	}

	public void MoveToRespawnLevel(int respawnLevel) {


		if (isMovementComplete) {
		
			isMovementComplete = false;
			canMove = true;

			if(respawnLevel == 1) {
				previousLevel = 0;
				nextLevel = 1;
			}
			else {
				previousLevel = respawnLevel - 1;
				nextLevel = respawnLevel + 1;
			}
			

			UpdatePosition (respawnLevel);

		}
	}
	

    //-1 for left, 1 for right
	void UpdatePosition(int targetLevel) {

		GameObject obj = GameObject.FindGameObjectWithTag ("level_" + targetLevel); //"start" + targetLevel
		
		StartCoroutine(MoveToTarget(obj.transform,targetLevel));

	}
	
	
	IEnumerator MoveToTarget(Transform target,int targetLevel) {
	
		Vector3 sourcePos = transform.position;
		Vector3 destPos = target.position - transform.forward * 2;
		destPos.z = -10.0f;
		float i = 0.0f;
		while (i < 1.0f ) {
			transform.position = Vector3.Lerp(sourcePos, destPos, Mathf.SmoothStep(0,1,i));
			i += Time.deltaTime;
			yield return 0;
		}
		isMovementComplete = true;
		currentLevel = targetLevel;
	}
	
	// Update is called once per frame
   void Update () {

   }


  //called when the given level is reached
  public void SwipeToLevelEnded(int level) {
		isMovementComplete = true;
		canMove = false;
		currentLevel = level;
		Debug.Log ("RECHED LEVEL: " + level);
  }
  

  void OnGUI() {

      if(isCameraZoomingIn) {
		
		/**GUI.skin = skin;
		
		Matrix4x4 svMat = GUI.matrix;//save current matrix
		
	    int width = GUIResolutionHelper.Instance.screenWidth;
		int height = GUIResolutionHelper.Instance.screenHeight;
		Vector3 scaleVector = GUIResolutionHelper.Instance.scaleVector;
		
		bool isWideScreen = GUIResolutionHelper.Instance.isWidescreen;
		
		if(isWideScreen) {
			GUI.matrix = Matrix4x4.TRS(new Vector3( (GUIResolutionHelper.Instance.scaleX - scaleVector.y) / 2 * width, 0, 0), Quaternion.identity, scaleVector);
			
			
		}
		else {
			GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,scaleVector);
			
		}

		if(Event.current.type==EventType.Repaint) {

		}

		if(showMessageOnZoom && messageKey!=null) {
			GUI.Label (new Rect(width/2-190, height/2-200, 400, 50), "What is this thing on the back???");
		    Rect helpMeTextureRect = new Rect(width / 3 - 180,height/2-200,64,64);
		    GUI.DrawTexture(helpMeTextureRect, helpMeTexture);
		}

		GUI.matrix = svMat;
		*/
						
      }
		
  }

  

/*
    Smooth zoom in
    using UnityEngine;
 using System.Collections;
 
 public class ZoomInOut : MonoBehaviour 
 {
     public float distance;
     private float sensitivityDistance = -7.5f;
     private float damping = 2.5f;
     private float min = -15f;
     private float max = -80f;
     private Vector3 zdistance;
     
     void  Start ()
     {
         distance = -20f;
         distance = transform.localPosition.z;
     }
     void  Update ()
     {
         distance -= Input.GetAxis("Mouse ScrollWheel") * sensitivityDistance;
         distance = Mathf.Clamp(distance, min, max);
         zdistance.z = Mathf.Lerp(transform.localPosition.z, distance, Time.deltaTime * damping);
         transform.localPosition = zdistance;
     }
 }
  */  
}
