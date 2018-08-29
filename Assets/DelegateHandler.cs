using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class DelegateHandler : MonoBehaviour
{
 
     public delegate void OnActionFinishedDelegate ();
     public static event OnActionFinishedDelegate actionDelegate;
      
     public void ActionCompleted()
     {
       actionDelegate ();
     }
 
}
