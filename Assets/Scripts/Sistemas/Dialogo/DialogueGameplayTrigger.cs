using UnityEngine;

[System.Serializable]
public class DialogueGameplayTrigger : MonoBehaviour
{
    public TextAsset dialogo; 
    public bool dialogoFoiAtivado;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !PlayerControle.instance.estaDialogoNormal && !dialogoFoiAtivado)
        {
            Debug.Log("Acionado DialogueGameplayTrigger");
            DialogueManagerGameplay.instance.AbrirDialogo(dialogo);
            dialogoFoiAtivado = true;
        }
    }
}
