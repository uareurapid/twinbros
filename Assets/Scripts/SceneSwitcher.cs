using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneSwitcher
{
    public static void LoadSceneOnTop(string scene)
    {
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
    }

    public static void UnLoadSceneOnTop(string scene) // Scene scene && scene.ToString()
    {
        int n = SceneManager.sceneCount;
        if (n > 1)
        {
            SceneManager.UnloadSceneAsync(scene);
        }
    }

    public static void UnLoadCurrentSceneFromTop() // Scene scene && scene.ToString()
    {
        int n = SceneManager.sceneCount;
        Debug.Log("SCENE COUNT: " + n);
        if (n > 1)
        {
            Debug.Log("UNLOAD SCENE AT INDEX: " + (n -1));
            Scene scene = SceneManager.GetSceneAt(n-1);
            SceneManager.UnloadSceneAsync(scene);
        }
    }
}