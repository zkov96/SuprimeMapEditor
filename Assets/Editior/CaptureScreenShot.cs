using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class CaptureScreenShot:MonoBehaviour
{
    private void Start()
    {
        GameObject[] gos = SceneManager.GetActiveScene().GetRootGameObjects();
//            .Where(go => go.hideFlags == HideFlags.HideInHierarchy).ToArray();
        Debug.Log("");
    }

    private static void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
    {
        GameObject[] gos = scene.GetRootGameObjects().Where(go => go.hideFlags == HideFlags.HideInHierarchy).ToArray();
                
        Debug.Log("");
    }

    private static void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        GameObject[] gos = scene.GetRootGameObjects().Where(go => go.hideFlags == HideFlags.HideInHierarchy).ToArray();
        
        Debug.Log("");
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
    }

//    private void Update()
//    {
//        GameObject[] gos = SceneManager.GetActiveScene().GetRootGameObjects().Where(go => go.hideFlags == HideFlags.HideInHierarchy).ToArray();
//        Debug.Log("");
//    }
}