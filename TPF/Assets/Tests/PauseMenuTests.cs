using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PauseMenuTests
{
    GameObject pausePanel;
    PauseManager pauseManager;

    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.LoadScene("DungeonMap");
    }

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        yield return WaitForSceneLoad();

        GameObject canvas = GameObject.Find("Canvas");
        Assert.IsNotNull(canvas, "Canvas GameObject not found in the scene");

        pausePanel = canvas.transform.Find("PausePanel").gameObject;
        Assert.IsNotNull(pausePanel, "PausePanel GameObject not found under Canvas");

        pauseManager = GameObject.FindObjectOfType<PauseManager>();
        Assert.IsNotNull(pauseManager, "PauseManager script not found in the scene");
    }

    IEnumerator WaitForSceneLoad()
    {
        while (!SceneManager.GetSceneByName("DungeonMap").isLoaded)
        {
            yield return null;
        }
    }

    [Test, Order(1)]
    public void PausePanelIsNotActiveInitially()
    {
        Assert.IsFalse(pausePanel.activeSelf);
    }

    [UnityTest, Order(2)]
    public IEnumerator CanPauseGame()
    {
        yield return null; // Make sure any initial frame updates are done
        pauseManager.PauseGame();
        yield return null;

        Assert.IsTrue(pausePanel.activeSelf, "PausePanel should be active after pausing the game");
        Assert.AreEqual(0f, Time.timeScale, "Time should be stopped after pausing the game");
    }

    [UnityTest, Order(3)]
    public IEnumerator CanUnpauseGame()
    {
        yield return null;
        pauseManager.PauseGame();
        yield return null;
        pauseManager.ResumeGame();
        yield return null;

        Assert.IsFalse(pausePanel.activeSelf, "PausePanel should be inactive after resuming the game");
        Assert.AreEqual(1f, Time.timeScale, "Time should be resumed after resuming the game");
    }

    [UnityTest, Order(4)]
    public IEnumerator CanDisablePause()
    {
        yield return null;
        pauseManager.DisablePause();
        yield return null;

        Assert.IsFalse(pausePanel.activeSelf, "PausePanel should be inactive after disabling pause");
        Assert.AreEqual(1f, Time.timeScale, "Time should not be affected after disabling pause");

        pauseManager.PauseGame();
        yield return null;

        Assert.IsFalse(pausePanel.activeSelf, "Pause should not make PausePanel active after disabling pause");
        Assert.AreEqual(1f, Time.timeScale, "Pause should not stop time after disabling pause");
    }
}
