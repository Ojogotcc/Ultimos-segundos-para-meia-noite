using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Mensagem[] mensagens; // Array de mensagens do dialogo
    public Char[] chars; // Array de personagens do dialogo

    public void IniciarDialogo() // Metodo para iniciar o dialogo
    {
        // Encontra um objeto do tipo DialogueManager e chama o metodo AbrirDialogo
        FindAnyObjectByType<DialogueManager>().AbrirDialogo(mensagens, chars);
    }

    public void FinalizarDialogo() // Metodo para finalizar o dialogo
    {
        // Encontra um objeto do tipo DialogueManager e chama o Metodo FecharDialogo
        FindAnyObjectByType<DialogueManager>().FecharDialogo();
    }
}

[System.Serializable]
public class Mensagem // Classe que representa uma mensagem no dialogo
{
    public int charID; // ID do personagem que esta falando
    [TextArea(3, 10)]
    public string mensagem; // Texto da mensagem, com uma area de texto ajustavel no inspector
}

[System.Serializable]
public class Char // Classe que representa um personagem no dialogo
{
    public string charName; // Nome do personagem
    public Sprite sprite; // Sprite do personagem
}
