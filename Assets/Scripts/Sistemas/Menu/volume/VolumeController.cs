using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [Header("Objetos")]
    [SerializeField] private AudioMixer Main;
    [SerializeField] private Slider VolumeMasterSlider;
    [SerializeField] private Slider VolumeSFXSlider;
    [SerializeField] private Slider VolumeMusicaSlider;

    private void Start()
    {
        LoadVolumeMasterValue();
        LoadVolumeSFXValue();
        LoadVolumeMusicaValue();
    }

    public void SetVolumeMaster(float sliderValue)
    {
        Main.SetFloat("VolumeMaster", sliderValue);
        PlayerPrefs.SetFloat("MasterValue", sliderValue);
    }

    public void SetVolumeSFX(float sliderValue)
    {
        Main.SetFloat("VolumeSFX", sliderValue);
        PlayerPrefs.SetFloat("SFXValue", sliderValue);
    }

    public void SetVolumeMusica(float sliderValue)
    {
        Main.SetFloat("VolumeMusica", sliderValue);
        PlayerPrefs.SetFloat("MusicaValue", sliderValue);
    }
    public void LoadVolumeMasterValue()
    {
        float volume_master_value = PlayerPrefs.GetFloat("MasterValue");
        VolumeMasterSlider.value = volume_master_value;
        SetVolumeMaster(volume_master_value);
    }

    public void LoadVolumeSFXValue()
    {
        float volume_sfx_value = PlayerPrefs.GetFloat("SFXValue");
        VolumeSFXSlider.value = volume_sfx_value;
        SetVolumeMaster(volume_sfx_value);
    }

    public void LoadVolumeMusicaValue()
    {
        float volume_musica_value = PlayerPrefs.GetFloat("MusicaValue");
        VolumeMusicaSlider.value = volume_musica_value;
        SetVolumeMaster(volume_musica_value);
    }
}
