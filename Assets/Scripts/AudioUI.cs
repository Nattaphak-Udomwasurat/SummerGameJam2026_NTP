using UnityEngine;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    void Start()
    {
        bgmSlider.value = AudioManager.Instance.GetBGMVolume();
        sfxSlider.value = AudioManager.Instance.GetSFXVolume();

        bgmSlider.onValueChanged.AddListener(SetBGM);
        sfxSlider.onValueChanged.AddListener(SetSFX);
    }

    void SetBGM(float value)
    {
        AudioManager.Instance.SetBGMVolume(value);
    }

    void SetSFX(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
    }
}