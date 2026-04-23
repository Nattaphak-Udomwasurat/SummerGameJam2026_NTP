using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private List<AudioSource> sfxSources;


    private float bgmVolume = 1f;
    private float sfxVolume = 1f;

    private int currentIndex = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // โหลดค่าเสียง
        bgmVolume = PlayerPrefs.GetFloat("BGM", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFX", 1f);

        ApplyVolume();
    }

    private void ApplyVolume()
    {
        bgmSource.volume = bgmVolume;

        // 🔥 วน set ทุก SFX source
        foreach (var source in sfxSources)
        {
            source.volume = sfxVolume;
        }
    }

    //  เล่น BGM
    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip) return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    //  เล่น SFX
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSources.Count == 0) return;

        AudioSource source = sfxSources[currentIndex];
        source.PlayOneShot(clip, sfxVolume);

        currentIndex++;
        if (currentIndex >= sfxSources.Count)
            currentIndex = 0;
    }

    // ปรับเสียง
    public void SetBGMVolume(float value)
    {
        bgmVolume = value;
        PlayerPrefs.SetFloat("BGM", value);
        ApplyVolume();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        PlayerPrefs.SetFloat("SFX", value);
        ApplyVolume();
    }

    public float GetBGMVolume() => bgmVolume;
    public float GetSFXVolume() => sfxVolume;
}