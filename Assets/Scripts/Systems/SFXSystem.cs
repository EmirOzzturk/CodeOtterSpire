using UnityEngine;
using UnityEngine.Audio;

[AddComponentMenu("Audio/SFX System")]
public class SFXSystem: PersistentSingleton<SFXSystem>
{
    [Header("Kütüphaneler")]
    [SerializeField] private SfxLibrary library;

    [Header("Mixer")]
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("Havuz")]
    [SerializeField, Range(4, 32)] private int poolSize = 8;
    private AudioSource[] pool;
    private int nextIdx;

    private void Start()
    {
        pool = new AudioSource[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.outputAudioMixerGroup = sfxGroup;
            src.spatialBlend = 0f;           // 2D ses
            src.playOnAwake  = false;
            pool[i] = src;
        }
    }

    /*―――――――――― PUBLIC API ――――――――――*/

    /// <summary>Enum + otomatik varyasyon + pitch.</summary>
    public void Play(SFXType type, float volumeMul = 1f)
    {
        var data = library?.Get(type);
        if (data == null) return;
        InnerPlay(data, volumeMul);
    }

    /// <summary>Doğrudan SFXData gönder.</summary>
    public void Play(SFXData data, float volumeMul = 1f) =>
        InnerPlay(data, volumeMul);

    /// <summary>Klasik AudioClip çalar (pitch varyasyonu yok).</summary>
    public void PlayClip(AudioClip clip, float volumeMul = 1f)
    {
        if (!clip) return;
        var src = NextSource();
        src.pitch = 1f;
        src.PlayOneShot(clip, volumeMul);
    }

    /*――――――――――  INTERNAL ――――――――――*/

    private void InnerPlay(SFXData data, float volumeMul)
    {
        var clip = data.GetRandomClip();
        if (!clip) return;

        var src = NextSource();
        src.pitch = Random.Range(1f - data.PitchVariation,
                                 1f + data.PitchVariation);
        src.PlayOneShot(clip, data.BaseVolume * volumeMul);
    }

    private AudioSource NextSource()
    {
        var src = pool[nextIdx];
        nextIdx = (nextIdx + 1) % poolSize;
        return src;
    }
}
