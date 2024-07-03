using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Mensagem[] mensagens; // Array de mensagens do diálogo
    public Char[] chars; // Array de personagens do diálogo

    public void IniciarDialogo() // Método para iniciar o diálogo
    {
        // Encontra um objeto do tipo DialogueManager e chama o método AbrirDialogo
        FindAnyObjectByType<DialogueManager>().AbrirDialogo(mensagens, chars);
    }

    public void FinalizarDialogo() // Método para finalizar o diálogo
    {
        // Encontra um objeto do tipo DialogueManager e chama o método FecharDialogo
        FindAnyObjectByType<DialogueManager>().FecharDialogo();
    }
}

[System.Serializable]
public class Mensagem // Classe que representa uma mensagem no diálogo
{
    public int charID; // ID do personagem que está falando
    [TextArea(3, 10)]
    public string mensagem; // Texto da mensagem, com uma área de texto ajustável no inspector
}

[System.Serializable]
public class Char // Classe que representa um personagem no diálogo
{
    public string charName; // Nome do personagem
    public Sprite sprite; // Sprite do personagem
}
