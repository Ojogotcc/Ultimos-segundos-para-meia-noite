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
    public Image charImage0; // Referência à imagem do personagem 0
    public Image charImage1; // Referência à imagem do personagem 1
    public TextMeshProUGUI charNome; // Referência ao componente TextMeshProUGUI para o nome do personagem
    public TextMeshProUGUI mensagemTexto; // Referência ao componente TextMeshProUGUI para o texto da mensagem
    public RectTransform background; // Referência ao fundo do diálogo

    Mensagem[] mensagemAtual; // Armazena as mensagens do diálogo atual
    Char[] charAtual; // Armazena os personagens do diálogo atual
    int mensagemAtiva = 0; // Índice da mensagem ativa
    public static bool estaAtivo = false; // Indica se o diálogo está ativo
    public float delayDigitar = 0.01f; // Delay entre cada caractere digitado

    public void AbrirDialogo(Mensagem[] mensagens, Char[] chars) // Abre o diálogo com as mensagens e personagens fornecidos
    {
        mensagemAtual = mensagens; // Armazena as mensagens
        charAtual = chars; // Armazena os personagens
        mensagemAtiva = 0; // Reseta o índice da mensagem ativa
        estaAtivo = true; // Define que o diálogo está ativo
        Debug.Log("Iniciou dialogo com: " + chars[0].charName + " e " + chars[1].charName); // Loga os nomes dos personagens no console
        MostrarMensagem(); // Mostra a primeira mensagem
        background.LeanScale(Vector3.one, 0.3f); // Anima a escala do fundo do diálogo para aparecer
    }

    public void FecharDialogo() // Fecha o diálogo
    {
        estaAtivo = false; // Define que o diálogo não está ativo
        Debug.Log("Dialogo foi fechado"); // Loga no console que o diálogo foi fechado
        background.LeanScale(Vector3.zero, 0.2f); // Anima a escala do fundo do diálogo para desaparecer
        mensagemAtiva = 0; // Reseta o índice da mensagem ativa
    }

    void MostrarMensagem() // Mostra a mensagem atual junto com o personagem falante e seu nome, depois inicia a digitação
    {
        StopAllCoroutines(); // Para todas as corrotinas em execução
        Mensagem mensagemParaMostrar = mensagemAtual[mensagemAtiva]; // Obtém a mensagem atual
        Char charParaMostrar = charAtual[mensagemParaMostrar.charID]; // Obtém o personagem correspondente à mensagem
        charNome.text = charParaMostrar.charName; // Define o nome do personagem no UI
        charImage0.sprite = charAtual[0].sprite; // Define a imagem do personagem 0 no UI
        charImage1.sprite = charAtual[1].sprite; // Define a imagem do personagem 1 no UI

        // Anima a escala das imagens dos personagens para indicar quem está falando
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

    IEnumerator DigitarFrase(string frase) // Deixa visível letra a letra (efeito de "digitar") com um pequeno delay
    {
        mensagemTexto.text = frase; // Define o texto completo da mensagem
        mensagemTexto.maxVisibleCharacters = 0; // Define o número de caracteres visíveis como zero
        for (int i = 0; i <= frase.Length; i++)
        {
            mensagemTexto.maxVisibleCharacters = i; // Incrementa o número de caracteres visíveis
            yield return new WaitForSeconds(delayDigitar); // Espera o delay definido antes de mostrar o próximo caractere
        }
    }

    public void ProximaMensagem() // Inicia a próxima parte do diálogo, se houver, caso contrário, desativa o diálogo
    {
        mensagemAtiva++; // Incrementa o índice da mensagem ativa
        if (mensagemAtiva < mensagemAtual.Length)
        {
            MostrarMensagem(); // Mostra a próxima mensagem
        }
        else
        {
            FecharDialogo(); // Fecha o diálogo se não houver mais mensagens
        }
    }

    void Start() // Esconde o canvas do diálogo ao iniciar
    {
        background.transform.localScale = Vector3.zero; // Define a escala do fundo do diálogo como zero
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && estaAtivo == true) // Avança para a próxima mensagem se a tecla Espaço for pressionada e o diálogo estiver ativo
        {
            ProximaMensagem();
        }
        if (Input.GetKeyUp(KeyCode.Escape) && estaAtivo == true) // Fecha o diálogo se a tecla Esc for pressionada e o diálogo estiver ativo
        {
            FecharDialogo();
        }
    }
}
