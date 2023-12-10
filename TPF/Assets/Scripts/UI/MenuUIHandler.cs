using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(10000)] // Ensures UI initializes last
public class MenuUIHandler : MonoBehaviour
{
    public SceneLoader sceneLoader;

    public void StartGame()
    {
        sceneLoader.LoadScene(SceneLoader.GameScene.MainScene);
    }

    public void Exit()
    {
        #if UNITY_EDITOR // If playing in the Unity Editor
            EditorApplication.ExitPlaymode();
        #else // If playing in the built application
            Application.Quit(); 
        #endif
    }
}
