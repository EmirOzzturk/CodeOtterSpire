
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioData/Scene Music Map")]
public class MusicSceneMap : ScriptableObject
{
    [System.Serializable]
    public class SceneMusicPair
    {
        [Tooltip("Build Settings’teki Scene adı (tam yazın)")]
        public string sceneName;
        public AudioClip musicClip;
    }

    [Header("Sahne → Müzik Eşlemesi")]
    [SerializeField] private List<SceneMusicPair> map = new List<SceneMusicPair>();

    public AudioClip GetClipForScene(string sceneName)
    {
        foreach (var pair in map)
        {
            if (pair.sceneName == sceneName)
                return pair.musicClip;
        }
        return null;
    }
}
