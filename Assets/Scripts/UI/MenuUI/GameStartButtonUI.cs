using UnityEngine;

public class GameStartButtonUI : MonoBehaviour
{
    
    public void StartCombat()
    {
        CharacterSelectSystem.Instance.UpdateStarterDeck();
        SceneLoadSystem.Instance.heroEnum = CharacterSelectSystem.Instance.SelectedHero.HeroEnum;
        SceneLoadSystem.Instance.LoadScene("Level1");
    }
}