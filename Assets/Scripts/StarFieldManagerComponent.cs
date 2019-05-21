using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class StarFieldManagerComponent : MonoBehaviour 
{
 public int numberOfStars = 10;

 private float[] mScreenX;
 private float[] mScreenY;

 private List<GameObject> mStars;

 private Rect mWorldViewPort;

 private float[] mX;
 private float[] mY;
 private float[] mZ;
 private float[] mVZ;

 private bool isMoving = true;
 public GameObject star;

 private Vector3 originalLocalPosition;

  void Start() 
  {
        isMoving = true;
        mStars = new List<GameObject>();
        originalLocalPosition = transform.localPosition;
        InitStarField();
        isMoving = false;

  }

  public void InitStarField() {



        mScreenX = new float[numberOfStars];
        mScreenY = new float[numberOfStars];

        mX = new float[numberOfStars];
        mY = new float[numberOfStars];

        mZ = new float[numberOfStars];
        mVZ = new float[numberOfStars];

        Vector3 mV1 = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMin, Camera.main.pixelRect.yMin, 0F));
        Vector3 mV2 = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMax, Camera.main.pixelRect.yMax, 0F));
        Vector3 mV3 = (mV2 - mV1);

        mWorldViewPort = new Rect(mV1.x, mV1.y, mV3.x, mV3.y);

        GameObject mTempObject = (GameObject)Instantiate(star); //Resources.Load<GameObject>("Prefabs/Scene/Star")

        for (int A = 0; A < numberOfStars; A++)
        {
            mX[A] = GetRandom(mWorldViewPort.xMin, mWorldViewPort.xMax);
            mY[A] = GetRandom(mWorldViewPort.yMin, mWorldViewPort.yMax);
            mZ[A] = 1F;
            mVZ[A] = GetRandom(0.001F, 0.01F);

            GameObject mObject = Instantiate(mTempObject);
            mObject.name = String.Format("Star - {0}", A);
            mObject.transform.SetParent(this.transform, true);
            //mObject.GetComponent<SpriteRenderer>().color = new Color(0.8F, 0.8F, 0.8F);  
            mScreenX[A] = (mX[A] / mZ[A]);
            mScreenY[A] = (mY[A] / mZ[A]);

            mObject.transform.position = new Vector3(mScreenX[A], mScreenY[A], 1F);

            mStars.Add(mObject);
        }

        Destroy(mTempObject);
    }

    //called when moving to the next level
    public void MoveStarFieldToNextLevel() {

        isMoving = true;
        for (int i = 0; i < numberOfStars; i++) {
            Destroy(mStars[i]);
        }
        mStars.Clear();
    }

    public void RestartStarFieldOnNewLocation(Transform newParent) {
        transform.parent = newParent;
        transform.localPosition = originalLocalPosition;
        InitStarField();
        isMoving = false;
    }
	
  void Update () 
  {
     if (numberOfStars > 0 && !isMoving)
     {
            for (int A = 0; A < numberOfStars; A++)
      {
       float mSize = (mVZ[A] * 0.125F);

       mZ[A] -= mVZ[A];

       if (mZ[A] > 0.0)
         {
          mScreenX[A] = (mX[A] / mZ[A]);
          mScreenY[A] = (mY[A] / mZ[A]);

          if (mWorldViewPort.Contains(new Vector3(mScreenX[A], mScreenY[A], 1F)) == false)
            {
             mX[A] = GetRandom(mWorldViewPort.xMin, mWorldViewPort.xMax);
             mY[A] = GetRandom(mWorldViewPort.yMin, mWorldViewPort.yMax);
             mZ[A] = 1F;
             mVZ[A] = GetRandom(0.001F, 0.01F);
             mStars[A].transform.position = new Vector3(mX[A], mY[A], 1F);
             mStars[A].transform.localScale = new Vector3(1F, 1F, 1F);
            }
         }
       else
           {
            mVZ[A] = GetRandom(0.001F, 0.01F);
            continue;
           }

       mStars[A].transform.position = new Vector3(mScreenX[A], mScreenY[A], 1f);
       mStars[A].transform.localScale += new Vector3(mSize, mSize, 1F);
      }
     }	
  }

  private float GetRandom(float min, float max)
  {
   return UnityEngine.Random.Range(min, max);
  }
}
