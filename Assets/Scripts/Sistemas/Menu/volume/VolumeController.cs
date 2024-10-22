using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [Header("Objetos")]
    [SerializeField] private AudioMixer Main;
    [SerializeField] private Slider VolumeSlider;

    private void Start()
    {
        LoadVolumeMasterValue();
        LoadVolumeSFXValue();
        LoadVolumeMusicaValue();
    }

    public void SetVolumeMaster(float sliderValue)
    {
        Main.SetFloat("VolumeMaster", sliderValue);
        SaveVolumeMaster();
    }

    public void SetVolumeSFX(float sliderValue)
    {
        Main.SetFloat("VolumeSFX", sliderValue);
        SaveVolumeSFX();
    }

    public void SetVolumeMusica(float sliderValue)
    {
        Main.SetFloat("VolumeMusica", sliderValue);
        SaveVolumeMusica();
    }

    public void SaveVolumeMaster()
    {
        float volume_master_value = VolumeSlider.value;
        PlayerPrefs.SetFloat("MasterValue", volume_master_value);
    }

   public void SaveVolumeSFX()
    {
        float volume_sfx_value = VolumeSlider.value;
        PlayerPrefs.SetFloat("SFXValue", volume_sfx_value);
    }

   public void SaveVolumeMusica()
    {
        float volume_musica_value = VolumeSlider.value;
        PlayerPrefs.SetFloat("MusicaValue", volume_musica_value);
    }

    public void LoadVolumeMasterValue()
    {
        float volume_master_value = PlayerPrefs.GetFloat("MasterValue");
        VolumeSlider.value = volume_master_value;
        SetVolumeMaster(volume_master_value);
    }

    public void LoadVolumeSFXValue()
    {
        float volume_sfx_value = PlayerPrefs.GetFloat("SFXValue");
        VolumeSlider.value = volume_sfx_value;
        SetVolumeMaster(volume_sfx_value);
    }

    public void LoadVolumeMusicaValue()
    {
        float volume_musica_value = PlayerPrefs.GetFloat("MusicaValue");
        VolumeSlider.value = volume_musica_value;
        SetVolumeMaster(volume_musica_value);
    }
}
