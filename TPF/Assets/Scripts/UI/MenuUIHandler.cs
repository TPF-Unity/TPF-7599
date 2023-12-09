using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(10000)] // Ensures UI initializes last
public class MenuUIHandler : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
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
