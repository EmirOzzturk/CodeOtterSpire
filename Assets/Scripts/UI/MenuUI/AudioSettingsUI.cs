using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    // Slider 0-1 arası value gönderir
    public void OnMusicSlider(float value) =>
        mixer.SetFloat("MusicVolume", Mathf.Lerp(-80f, 0f, value));

    public void OnSFXSlider(float value) =>
        mixer.SetFloat("SFXVolume", Mathf.Lerp(-80f, 0f, value));
}