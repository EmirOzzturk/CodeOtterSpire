using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioData/SFX Data")]
public class SFXData : ScriptableObject
{
    [Header("Temel Ayarlar")]
    [Tooltip("Birden fazla clip koyarsan rastgele seçilir.")]
    [SerializeField] private List<AudioClip> clips = new List<AudioClip>();

    [Tooltip("0-1 arası temel volüm (master volümle çarpılır).")]
    [Range(0f, 1f)]
    [SerializeField] private float baseVolume = 1f;

    [Header("Rastgele Pitch")]
    [Tooltip("Pitch varyasyonu (%). 0 = sabit pitch")]
    [Range(0f, 1f)]
    [SerializeField] private float pitchVariation = 0.1f;

    public AudioClip GetRandomClip() =>
        clips.Count == 0 ? null : clips[Random.Range(0, clips.Count)];

    public float BaseVolume => baseVolume;
    public float PitchVariation => pitchVariation;
}