using UnityEngine;
using System.Collections;

	/// <summary>
	/// Add this class to a GameObject to make it rotate on itself
	/// </summary>
	public class AutoRotate : MonoBehaviour 
	{
		public Space RotationSpace = Space.Self;
        public float delay = 0f;
        private bool started = false;
        

		/// The rotation speed. Positive means clockwise, negative means counter clockwise.
		public Vector3 RotationSpeed = new Vector3(100f,0f,0f);

        void Start () {
            if(delay > 0f) {
                Invoke("StartRotation", delay);
            } else {
                started = true;
                
            }
        }
		/// <summary>
		/// Makes the object rotate on its center every frame.
		/// </summary>
		protected virtual void Update () 
		{
            if(started) {
               transform.Rotate(RotationSpeed * Time.deltaTime, RotationSpace);
            }
			
		}

        void StartRotation() {
            started = true; 
        }
	}