using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutsceneTrigger : MonoBehaviour
{
    public bool jaDisparou = false; 
    public PlayableDirector cutsceneDirector;
    public GameObject player; 
    public GameObject Canvas; 
    public TimelineAsset timelineAsset;

    void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que entrou no trigger é o Player e se a cutscene já foi disparada
        if (other.CompareTag("Player") && !jaDisparou)
        {
            jaDisparou = true;  // Marca que a cutscene já foi disparada

            // Altera o TimelineAsset do PlayableDirector para a nova cutscene
            cutsceneDirector.playableAsset = timelineAsset;

            cutsceneDirector.Play();  // Inicia a cutscene

            Canvas.SetActive(false);

            // Subscribes ao evento que detecta o fim da cutscene
            cutsceneDirector.stopped += OnCutsceneEnd;
        }
    }

    // Método que é chamado quando a cutscene termina
    void OnCutsceneEnd(PlayableDirector director)
    {
        // Verifica se a cutscene que terminou é a nossa
        if (director == cutsceneDirector)
        {
            Canvas.SetActive(true);
            
            // Desinscreve do evento para evitar bugs futuros
            cutsceneDirector.stopped -= OnCutsceneEnd;
        }
    }
}
