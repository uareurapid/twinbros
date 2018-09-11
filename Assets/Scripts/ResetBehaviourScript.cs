using UnityEngine;
using System.Collections;



    //objects that implement this interface will be invited to reset it´s original
    //internal status, we don´t care about the specific implementation details
	public interface ResetBehaviourScript
	{
		void ResetOriginalBehaviour();
	}
