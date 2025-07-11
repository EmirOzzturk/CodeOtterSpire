using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[AddComponentMenu("Audio/Music System")]
public class MusicSystem : PersistentSingleton<MusicSystem>
{
    [Header("Kaynaklar")]
    [SerializeField] private MusicSceneMap sceneMusicMap;
    [SerializeField] private AudioMixerGroup musicMixerGroup;

    [Header("Fade Ayarı")]
    [SerializeField, Range(0.1f, 5f)] private float fadeDuration = 1f;

    private AudioSource activeSource;
    private AudioSource fadeSource;

    private void Start()
    {
        activeSource = gameObject.AddComponent<AudioSource>();
        fadeSource   = gameObject.AddComponent<AudioSource>();

        foreach (var src in new[] { activeSource, fadeSource })
        {
            src.loop                = true;
            src.playOnAwake         = false;
            src.outputAudioMixerGroup = musicMixerGroup;
        }

        SceneLoadSystem.Instance.OnSceneLoadStarted += NotifySceneChanged;
        
        // İlk sahnenin adı otamatik olarka veriliyor!!!!
        NotifySceneChanged("MainScreen");
    }


    /// <summary>
    /// Haricî sahne sistemi burayı çağırdığında, gerektiğinde parça değiştirir.
    /// </summary>
    public void NotifySceneChanged(string sceneName)
    {
        if (!sceneMusicMap) return;
        var clip = sceneMusicMap.GetClipForScene(sceneName);
        if (!clip || clip == activeSource.clip) return;

        fadeSource.clip   = clip;
        fadeSource.volume = 0;
        fadeSource.Play();

        StopAllCoroutines();
        StartCoroutine(CrossFade());
    }

    private IEnumerator CrossFade()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float k = t / fadeDuration;
            fadeSource.volume   = k;
            activeSource.volume = 1f - k;
            yield return null;
        }

        // Kaynakları takas et
        (activeSource, fadeSource) = (fadeSource, activeSource);
        fadeSource.Stop();
    }
}
