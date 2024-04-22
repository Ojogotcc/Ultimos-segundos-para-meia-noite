using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Mensagem[] mensagens;
    public Char[] chars;

    public void IniciarDialogo()
    {
        FindAnyObjectByType<DialogueManager>().AbrirDialogo(mensagens, chars);
    }

    public void FinalizarDialogo()
    {
        FindAnyObjectByType<DialogueManager>().FecharDialogo();
    }

}

[System.Serializable]
public class Mensagem
{
    public int charID;
    [TextArea(3, 10)]
    public string mensagem;
}

[System.Serializable]
public class Char
{
    public string charName;
    public Sprite sprite;
}