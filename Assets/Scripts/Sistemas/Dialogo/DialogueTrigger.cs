using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Mensagem[] mensagens; // Array de mensagens do di�logo
    public Char[] chars; // Array de personagens do di�logo

    public void IniciarDialogo() // M�todo para iniciar o di�logo
    {
        // Encontra um objeto do tipo DialogueManager e chama o m�todo AbrirDialogo
        FindAnyObjectByType<DialogueManager>().AbrirDialogo(mensagens, chars);
    }

    public void FinalizarDialogo() // M�todo para finalizar o di�logo
    {
        // Encontra um objeto do tipo DialogueManager e chama o m�todo FecharDialogo
        FindAnyObjectByType<DialogueManager>().FecharDialogo();
    }
}

[System.Serializable]
public class Mensagem // Classe que representa uma mensagem no di�logo
{
    public int charID; // ID do personagem que est� falando
    [TextArea(3, 10)]
    public string mensagem; // Texto da mensagem, com uma �rea de texto ajust�vel no inspector
}

[System.Serializable]
public class Char // Classe que representa um personagem no di�logo
{
    public string charName; // Nome do personagem
    public Sprite sprite; // Sprite do personagem
}
