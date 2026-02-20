using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("Mixer")]
    public AudioMixer mixer;
    public string bgmParam = "BGM_Volume";
    public string sfxParam = "SFX_Volume";

    [Header("Sliders (0~1)")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    private const float MinSliderValue = 0.0001f; // 防止log10(0)

    void Start()
    {
        // 给 slider 一个安全的最小值
        if (bgmSlider != null) bgmSlider.minValue = MinSliderValue;
        if (sfxSlider != null) sfxSlider.minValue = MinSliderValue;

        // 读取并同步当前 Mixer 值到 Slider（可选但很推荐）
        SyncSliderFromMixer(bgmSlider, bgmParam);
        SyncSliderFromMixer(sfxSlider, sfxParam);

        // 绑定事件：滑动就改音量
        if (bgmSlider != null) bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(SetSfxVolume);
    }

    public void SetBgmVolume(float value01) => SetMixerVolume(bgmParam, value01);
    public void SetSfxVolume(float value01) => SetMixerVolume(sfxParam, value01);

    private void SetMixerVolume(string paramName, float value01)
    {
        // 0~1 转 dB：dB = 20 * log10(value)
        float db = Mathf.Log10(Mathf.Clamp(value01, MinSliderValue, 1f)) * 20f;
        mixer.SetFloat(paramName, db);
    }

    private void SyncSliderFromMixer(Slider slider, string paramName)
    {
        if (slider == null) return;
        if (mixer.GetFloat(paramName, out float db))
        {
            // dB 转 0~1：value = 10^(dB/20)
            float value01 = Mathf.Pow(10f, db / 20f);
            slider.value = Mathf.Clamp(value01, MinSliderValue, 1f);
        }
    }
}