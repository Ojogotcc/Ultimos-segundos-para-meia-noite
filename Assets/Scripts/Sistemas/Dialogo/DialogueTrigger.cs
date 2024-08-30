using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public TextAsset dialogo; // Array de mensagens do dialog
    public void IniciarDialogo() // Metodo para iniciar o dialogo
    {
        // Encontra um objeto do tipo DialogueManager e chama o metodo AbrirDialogo
        FindAnyObjectByType<DialogueManager>().AbrirDialogo(dialogo);
    }

    public void FinalizarDialogo() // Metodo para finalizar o dialogo
    {
        // Encontra um objeto do tipo DialogueManager e chama o Metodo FecharDialogo
        FindAnyObjectByType<DialogueManager>().FecharDialogo();
    }
}