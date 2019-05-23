using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelector : MonoBehaviour {

    public MoveWayPoint[] courtineDoors;
    AsyncOperation asyncOperation;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadStageOne() {
        StartCoroutine(LoadScene("Level1"));
    }

    public void LoadStageTwo()
    {
        StartCoroutine(LoadScene("Level2"));
    }

    public void LoadStageThree()
    {
        StartCoroutine(LoadScene("Level3"));
    }

    public void LoadStageFour()
    {
        StartCoroutine(LoadScene("Level3"));
    }

    public void LoadStageFive()
    {
        StartCoroutine(LoadScene("Level3"));
    }

    IEnumerator LoadScene(string scene)
    {

        asyncOperation = SceneManager.LoadSceneAsync(scene);
        asyncOperation.allowSceneActivation = false;

        foreach (MoveWayPoint point in courtineDoors)
        {
            point.stopAfterXPassages = 2;
            point.justOnce = false;
            point.ContinueMovement();
        }
        while (!asyncOperation.isDone)
        {

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {

                if (courtineDoors[0].GetNumPassages() == 1 && courtineDoors[1].GetNumPassages() == 1)
                {
                    //Activate the Scene
                    //scene is ready now
                    Debug.Log("####################  scene is ready now");
                    asyncOperation.allowSceneActivation = true;
                }

            }
            yield return null;
        }
        asyncOperation.allowSceneActivation = true;
    }
}
