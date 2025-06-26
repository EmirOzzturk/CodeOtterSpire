using UnityEngine;

public class LevelSelectUI : MonoBehaviour
{
    public void LoadLevel1()
    {
        SceneLoadSystem.Instance.LoadSceneAsync("Level1");
    }

    public void LoadLevel2()
    {
        SceneLoadSystem.Instance.LoadSceneAsync("Level2");
    }

    public void LoadLevel3()
    {
        SceneLoadSystem.Instance.LoadSceneAsync("Level3");
    }

    public void LoadMainMenu()
    {
        SceneLoadSystem.Instance.LoadSceneAsync("MainScreen");
    }
}
