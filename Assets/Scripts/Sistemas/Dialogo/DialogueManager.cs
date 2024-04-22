using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Image charImage0;
    public Image charImage1;
    public TextMeshProUGUI charNome;
    public TextMeshProUGUI mensagemTexto;
    public RectTransform background;

    Mensagem[] mensagemAtual;
    Char[] charAtual;
    int mensagemAtiva = 0;
    public static bool estaAtivo = false;
    public float delayDigitar = 0.01f;

    public void AbrirDialogo(Mensagem[] mensagens, Char[] chars)
    {
        mensagemAtual = mensagens;
        charAtual = chars;
        mensagemAtiva = 0;
        estaAtivo = true;
        Debug.Log("Iniciou dialogo com: " + chars[0].charName + " e "+ chars[1].charName);
        MostrarMensagem();
        background.LeanScale(Vector3.one, 0.3f);
    }

    public void FecharDialogo()
    {
        estaAtivo = false;
        Debug.Log("Dialogo foi fechado");
        background.LeanScale(Vector3.zero, 0.2f);
        mensagemAtiva = 0;
    }

    void MostrarMensagem() // Mostra a frase atual junto com o personagem falante e o seu nome, por ultimo inicia a digitação
    {
        StopAllCoroutines();
        Mensagem mensagemParaMostrar = mensagemAtual[mensagemAtiva];
        Char charParaMostrar = charAtual[mensagemParaMostrar.charID];
        charNome.text = charParaMostrar.charName;
        charImage0.sprite = charAtual[0].sprite;
        charImage1.sprite = charAtual[1].sprite;

        if (mensagemParaMostrar.charID == 0)
        {
            charImage0.transform.LeanScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
            charImage1.transform.LeanScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        } else
        {
            charImage1.transform.LeanScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
            charImage0.transform.LeanScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        }
                
        StartCoroutine(DigitarFrase(mensagemParaMostrar.mensagem));
    }
        
    IEnumerator DigitarFrase (string frase) // Deixa visivel letra a letra (efeito de "digitar") com uns segundos de delay
    {
        mensagemTexto.text = frase;
        mensagemTexto.maxVisibleCharacters = 0;
        for (int i = 0; i<= frase.Length; i++)
        {
            mensagemTexto.maxVisibleCharacters = i;
            yield return new WaitForSeconds(delayDigitar);
        }
    }

    public void ProximaMensagem() // Inicia a proxima parte do dialogo, caso tenha terminado ele desativa
    {
        mensagemAtiva++;
        if (mensagemAtiva < mensagemAtual.Length)
        {
            MostrarMensagem();
        }
        else
        {
            FecharDialogo();
        }
    }

    void Start() // Esconde o canvas do dialogo
    {
        background.transform.localScale = Vector3.zero;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && estaAtivo == true) // Verifica se já acabou de digitar para ir pro próximo 
        {
            ProximaMensagem();
        }
        if (Input.GetKeyUp(KeyCode.Escape) && estaAtivo == true) // Fecha o dialogo quando o player aperta o Esc
        {
            FecharDialogo();
        }
    }
}
