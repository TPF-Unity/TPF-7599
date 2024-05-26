using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class MenuSceneTests
{
    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    IEnumerator WaitForSceneLoad()
    {
        while (!SceneManager.GetSceneByName("MenuScene").isLoaded)
        {
            yield return null;
        }
    }

    [Test]
    public void FirstSceneInBuildSettingsIsMenuScene()
    {
        string firstSceneName = EditorBuildSettings.scenes[0].path;
        Assert.AreEqual("Assets/Scenes/MenuScene.unity", firstSceneName, "The first scene in the build settings is not the MenuScene");
    }

    [UnityTest]
    public IEnumerator MenuSceneContainsStartButton()
    {
        yield return WaitForSceneLoad();

        Button startGameButton = GameObject.Find("StartButton").GetComponent<Button>();
        Assert.IsNotNull(startGameButton, "Start Game button not found in MenuScene");

        startGameButton.onClick.Invoke();
        yield return new WaitForSeconds(1f);

        string currentSceneName = SceneManager.GetActiveScene().name;
        Assert.AreEqual("ProcGen", currentSceneName, "Clicking Start Game button did not redirect to DungeonMap scene");
    }

    [UnityTest]
    public IEnumerator MenuSceneContainsExitButton()
    {
        yield return WaitForSceneLoad();

        Button exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
        Assert.IsNotNull(exitButton, "Exit button not found in MenuScene");
    }
}
