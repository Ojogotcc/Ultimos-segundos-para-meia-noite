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

    public void PlayEfeitoNoLocal(AudioClip clip, Transform local, float volume)
    {
        AudioSource audioSource = Instantiate(audioSourcePrefab, local.position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        float duracao = audioSource.clip.length;
        Destroy(audioSource.gameObject, duracao);
    }

    public void PlayEfeitosNoLocal(AudioClip[] clips, Transform local, float volume)
    {
        int escolha = Random.Range(0, clips.Length);
        AudioSource audioSource = Instantiate(audioSourcePrefab, local.position, Quaternion.identity);
        audioSource.clip = clips[escolha];
        audioSource.volume = volume;
        audioSource.Play();

        float duracao = audioSource.clip.length;
        Destroy(audioSource.gameObject, duracao);
    }
}
