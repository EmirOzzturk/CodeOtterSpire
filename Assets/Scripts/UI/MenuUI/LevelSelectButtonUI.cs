using UnityEngine;

public class LevelSelectUI : MonoBehaviour
{
    public void LoadLevel1()
    {
        SceneLoadSystem.Instance.LoadScene("Level1");
    }

    public void LoadLevel2()
    {
        SceneLoadSystem.Instance.LoadScene("Level2");
    }

    public void LoadLevel3()
    {
        SceneLoadSystem.Instance.LoadScene("Level3");
    }

    public void LoadCharacterChooseScene()
    {
        SceneLoadSystem.Instance.LoadScene("CharacterChooseScene");
    }

    public void LoadMainMenu()
    {
        SceneLoadSystem.Instance.LoadScene("MainScreen");
    }

    public void LoadMainMenuInResultPanel()
    {
        InCombatUISystem.Instance.HideResultPanel();
        LoadMainMenu();
    }


    
}
