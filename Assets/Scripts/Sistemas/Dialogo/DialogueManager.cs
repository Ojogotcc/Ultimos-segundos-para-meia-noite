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
    public Image charImage0; // Refer�ncia � imagem do personagem 0
    public Image charImage1; // Refer�ncia � imagem do personagem 1
    public TextMeshProUGUI charNome; // Refer�ncia ao componente TextMeshProUGUI para o nome do personagem
    public TextMeshProUGUI mensagemTexto; // Refer�ncia ao componente TextMeshProUGUI para o texto da mensagem
    public RectTransform background; // Refer�ncia ao fundo do di�logo

    Mensagem[] mensagemAtual; // Armazena as mensagens do di�logo atual
    Char[] charAtual; // Armazena os personagens do di�logo atual
    int mensagemAtiva = 0; // �ndice da mensagem ativa
    public static bool estaAtivo = false; // Indica se o di�logo est� ativo
    public float delayDigitar = 0.01f; // Delay entre cada caractere digitado

    public void AbrirDialogo(Mensagem[] mensagens, Char[] chars) // Abre o di�logo com as mensagens e personagens fornecidos
    {
        mensagemAtual = mensagens; // Armazena as mensagens
        charAtual = chars; // Armazena os personagens
        mensagemAtiva = 0; // Reseta o �ndice da mensagem ativa
        estaAtivo = true; // Define que o di�logo est� ativo
        Debug.Log("Iniciou dialogo com: " + chars[0].charName + " e " + chars[1].charName); // Loga os nomes dos personagens no console
        MostrarMensagem(); // Mostra a primeira mensagem
        background.LeanScale(Vector3.one, 0.3f); // Anima a escala do fundo do di�logo para aparecer
    }

    public void FecharDialogo() // Fecha o di�logo
    {
        estaAtivo = false; // Define que o di�logo n�o est� ativo
        Debug.Log("Dialogo foi fechado"); // Loga no console que o di�logo foi fechado
        background.LeanScale(Vector3.zero, 0.2f); // Anima a escala do fundo do di�logo para desaparecer
        mensagemAtiva = 0; // Reseta o �ndice da mensagem ativa
    }

    void MostrarMensagem() // Mostra a mensagem atual junto com o personagem falante e seu nome, depois inicia a digita��o
    {
        StopAllCoroutines(); // Para todas as corrotinas em execu��o
        Mensagem mensagemParaMostrar = mensagemAtual[mensagemAtiva]; // Obt�m a mensagem atual
        Char charParaMostrar = charAtual[mensagemParaMostrar.charID]; // Obt�m o personagem correspondente � mensagem
        charNome.text = charParaMostrar.charName; // Define o nome do personagem no UI
        charImage0.sprite = charAtual[0].sprite; // Define a imagem do personagem 0 no UI
        charImage1.sprite = charAtual[1].sprite; // Define a imagem do personagem 1 no UI

        // Anima a escala das imagens dos personagens para indicar quem est� falando
        if (mensagemParaMostrar.charID == 0)
        {
            charImage0.transform.LeanScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f); // Aumenta a escala do personagem 0
            charImage1.transform.LeanScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f); // Diminui a escala do personagem 1
        }
        else
        {
            charImage1.transform.LeanScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f); // Aumenta a escala do personagem 1
            charImage0.transform.LeanScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f); // Diminui a escala do personagem 0
        }

        StartCoroutine(DigitarFrase(mensagemParaMostrar.mensagem)); // Inicia a corrotina para digitar a mensagem
    }

    IEnumerator DigitarFrase(string frase) // Deixa vis�vel letra a letra (efeito de "digitar") com um pequeno delay
    {
        mensagemTexto.text = frase; // Define o texto completo da mensagem
        mensagemTexto.maxVisibleCharacters = 0; // Define o n�mero de caracteres vis�veis como zero
        for (int i = 0; i <= frase.Length; i++)
        {
            mensagemTexto.maxVisibleCharacters = i; // Incrementa o n�mero de caracteres vis�veis
            yield return new WaitForSeconds(delayDigitar); // Espera o delay definido antes de mostrar o pr�ximo caractere
        }
    }

    public void ProximaMensagem() // Inicia a pr�xima parte do di�logo, se houver, caso contr�rio, desativa o di�logo
    {
        mensagemAtiva++; // Incrementa o �ndice da mensagem ativa
        if (mensagemAtiva < mensagemAtual.Length)
        {
            MostrarMensagem(); // Mostra a pr�xima mensagem
        }
        else
        {
            FecharDialogo(); // Fecha o di�logo se n�o houver mais mensagens
        }
    }

    void Start() // Esconde o canvas do di�logo ao iniciar
    {
        background.transform.localScale = Vector3.zero; // Define a escala do fundo do di�logo como zero
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && estaAtivo == true) // Avan�a para a pr�xima mensagem se a tecla Espa�o for pressionada e o di�logo estiver ativo
        {
            ProximaMensagem();
        }
        if (Input.GetKeyUp(KeyCode.Escape) && estaAtivo == true) // Fecha o di�logo se a tecla Esc for pressionada e o di�logo estiver ativo
        {
            FecharDialogo();
        }
    }
}
