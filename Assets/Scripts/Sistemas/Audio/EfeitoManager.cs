using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfeitoManager : MonoBehaviour
{
    public static EfeitoManager instance;

    public AudioSource audioSourcePrefab;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Existe mais de um EfeitoManager em cena!");
            return;
        }
        if (instance == null) instance = this;
    }

    public void PlayEfeito(AudioClip clip, Transform local, float volume, float spatial, float pitchrandom)
    {
        AudioSource audioSource = Instantiate(audioSourcePrefab, local.position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.spatialBlend = spatial;
        if (pitchrandom != 0f) audioSource.pitch = 1f + Random.Range(-pitchrandom, pitchrandom);
        audioSource.Play();

        float duracao = audioSource.clip.length;
        Destroy(audioSource.gameObject, duracao);
    }

    public void PlayEfeitos(AudioClip[] clips, Transform local, float volume, float spatial)
    {
        int escolha = Random.Range(0, clips.Length);
        AudioSource audioSource = Instantiate(audioSourcePrefab, local.position, Quaternion.identity);
        audioSource.clip = clips[escolha];
        audioSource.volume = volume;
        audioSource.spatialBlend = spatial;
        audioSource.Play();

        float duracao = audioSource.clip.length;
        Destroy(audioSource.gameObject, duracao);
    }
}
