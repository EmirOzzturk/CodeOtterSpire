using System;
using System.Collections;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InCombatUISystem : Singleton<InCombatUISystem>
{
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Button retryOrNextLevelButton;

    public void ShowResultPanel(Boolean winOrLose)
    {
        Time.timeScale = 0f;
        
        resultText.text = winOrLose ? CombatEndText.Win : CombatEndText.Lose;
        retryOrNextLevelButton.GetComponentInChildren<TMP_Text>().text = winOrLose ? CombatEndText.NextLevel : CombatEndText.Retry;

        if (winOrLose)
        {
            string nextSceneName = SceneLoadSystem.Instance.GetNextSceneName();
            if ( nextSceneName == null)
            {
                retryOrNextLevelButton.enabled = false;
                resultText.text = CombatEndText.WinThemAll;
                retryOrNextLevelButton.GetComponentInChildren<TMP_Text>().text = CombatEndText.AllDone;
            }
            else
            {
                retryOrNextLevelButton.onClick.RemoveAllListeners();
                retryOrNextLevelButton.onClick.AddListener(() => LoadScene(nextSceneName));
            }   
        }
        else
        {
            retryOrNextLevelButton.onClick.RemoveAllListeners();
            retryOrNextLevelButton.onClick.AddListener(ReloadCurrentLevel);
        }
        resultPanel.SetActive(true);
    }

    public void HideResultPanel()
    {
        resultPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    
    private void ReloadCurrentLevel()
    {
        HideResultPanel();
        var currentSceneName = SceneLoadSystem.Instance.GetCurrentSceneName();
        SceneLoadSystem.Instance.LoadScene(currentSceneName);
    }

    private void LoadScene(string sceneName)
    {
        HideResultPanel();
        SceneLoadSystem.Instance.LoadScene(sceneName);
    }
    
}
