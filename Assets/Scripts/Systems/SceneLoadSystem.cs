using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class SceneLoadSystem : PersistentSingleton<SceneLoadSystem>
{
    public Action<string> OnSceneLoadStarted;
    public Action<string> OnSceneLoadCompleted;
    
    /// <summary>
    /// Sahneyi senkron şekilde yükler
    /// </summary>
    public void LoadScene(string sceneName)
    {
        OnSceneLoadStarted?.Invoke(sceneName);
        SceneManager.LoadScene(sceneName);
        OnSceneLoadCompleted?.Invoke(sceneName);
    }

    /// <summary>
    /// Sahneyi asenkron şekilde yükler
    /// </summary>
    public void LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName, mode));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, LoadSceneMode mode)
    {
        OnSceneLoadStarted?.Invoke(sceneName);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);

        while (!asyncLoad.isDone)
        {
            // Gerekirse asyncLoad.progress ile ilerleme takibi yapılabilir
            yield return null;
        }

        OnSceneLoadCompleted?.Invoke(sceneName);
    }

    /// <summary>
    /// Aktif sahnenin adını döner
    /// </summary>
    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}