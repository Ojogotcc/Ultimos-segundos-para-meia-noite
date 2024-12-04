using UnityEngine;

public class MusicaManager : MonoBehaviour
{
    public static MusicaManager instance;
    public AudioSource audioSourcePrefab;
    public AudioClip musicaPorradas;
    public AudioClip musicaAmbiente;
    public bool tocandoPorradas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Existe mais de um MusicaManager em cena!");
            Destroy(gameObject);
        }
    }

    public void VoltarAoNormal()
    {
        audioSourcePrefab.clip = musicaAmbiente;
        audioSourcePrefab.Play();
        tocandoPorradas = false;
    }

    public void TocarMusicaPorrada()
    {
        if (!tocandoPorradas)
        {
            audioSourcePrefab.clip = musicaPorradas;
            audioSourcePrefab.Play();
            tocandoPorradas = true;
            Invoke(nameof(VoltarAoNormal), 30f);
        }
    }
}
