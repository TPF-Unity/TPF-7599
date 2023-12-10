using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum GameScene
    {
        MenuScene = 0,
        MainScene = 1,
    }

    public void LoadScene(GameScene scene)
    {
        SceneManager.LoadScene((int)scene);
    }
}
