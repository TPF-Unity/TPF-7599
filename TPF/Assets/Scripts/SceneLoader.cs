using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum GameScene
    {
        MenuScene = 0,
        MainScene = 1,
        GameWinScene = 2,
    }

    public void LoadScene(GameScene scene)
    {
        SceneManager.LoadScene((int)scene);
    }

    public void LoadMenuScene()
    {
        GameData.RestartLevel();
        SceneManager.LoadScene((int)GameScene.MenuScene);
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene((int)GameScene.MainScene);
    }

    public void LoadGameWinScene()
    {
        SceneManager.LoadScene((int)GameScene.GameWinScene);
    }
}
