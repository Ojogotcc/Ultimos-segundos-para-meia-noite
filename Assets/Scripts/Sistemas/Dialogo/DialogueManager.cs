using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    public Image charImage0;
    public Image charImage1;
    public TextMeshProUGUI charNome;
    public TextMeshProUGUI mensagemTexto;
    public RectTransform background;

    public GameObject FundoEscolhas;
    public GameObject[] escolhas;
    public TextMeshProUGUI[] escolhasTextos;

    public static bool estaAtivo = false;
    public static bool estaEscolhendo = false;
    public float delayDigitar = 0.01f;

    private Story historiaAtual;
    private string ultimoPersonagemFalante;

    public static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Existe mais de um DialogueManager em cena!");
            return;
        }
        instance = this;
    }

    public void AbrirDialogo(TextAsset inkJSON)
    {
        if (estaAtivo)
        {
            Debug.LogWarning("Um diálogo já está ativo.");
            return;
        }

        historiaAtual = new Story(inkJSON.text);
        estaAtivo = true;
        estaEscolhendo = false;
        ultimoPersonagemFalante = null;
        background.localScale = Vector3.zero;
        ProximaMensagem();
        background.LeanScale(new Vector3(1.94f, 1.94f, 1.94f), 0.3f);
    }

    public void FecharDialogo()
    {
        estaAtivo = false;
        estaEscolhendo = false;
        Debug.Log("Diálogo foi fechado");
        background.LeanScale(Vector3.zero, 0.2f);

        FundoEscolhas.SetActive(false);
        foreach (GameObject escolha in escolhas)
        {
            escolha.SetActive(false);
        }
    }

    void MostrarMensagem()
    {
        StopAllCoroutines();

        // Atualiza o personagem falante baseado nas tags do Ink
        AtualizarPersonagem(historiaAtual.currentTags);

        StartCoroutine(DigitarFrase(historiaAtual.Continue()));
    }

    void AtualizarPersonagem(List<string> tags)
    {
        // Verifica as tags para atualizar a UI do personagem falante
        foreach (string tag in tags)
        {
            if (tag == "Personagem(A)" && ultimoPersonagemFalante != "A")
            {
                charNome.text = "Andróide";
                charImage0.transform.LeanScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
                charImage1.transform.LeanScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
                ultimoPersonagemFalante = "A";
            }
            else if (tag == "Personagem(B)" && ultimoPersonagemFalante != "B")
            {
                charNome.text = "Militar";
                charImage0.transform.LeanScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
                charImage1.transform.LeanScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
                ultimoPersonagemFalante = "B";
            }
        }
    }

    IEnumerator DigitarFrase(string frase)
    {
        Debug.Log(frase);
        mensagemTexto.text = "";
        mensagemTexto.maxVisibleCharacters = 0;
        mensagemTexto.text = frase;

        for (int i = 0; i <= frase.Length; i++)
        {
            mensagemTexto.maxVisibleCharacters = i;
            yield return new WaitForSeconds(delayDigitar);
        }

        MostrarEscolhas();
    }

    public void ProximaMensagem()
    {
        if (historiaAtual.canContinue)
        {
            MostrarMensagem();
        }
        else
        {
            FecharDialogo();
        }
    }

    void Start()
    {
        background.transform.localScale = Vector3.zero;

        escolhasTextos = new TextMeshProUGUI[escolhas.Length];
        int index = 0;
        foreach (GameObject escolha in escolhas)
        {
            escolhasTextos[index] = escolha.GetComponentInChildren<TextMeshProUGUI>();
            escolha.SetActive(false);
            index++;
        }
        FundoEscolhas.SetActive(false);
    }

    void MostrarEscolhas()
    {
        List<Choice> escolhasAtuais = historiaAtual.currentChoices;

        if (escolhasAtuais.Count == 0)
        {
            FundoEscolhas.SetActive(false);
            return;
        }

        estaEscolhendo = true;
        FundoEscolhas.SetActive(true);

        if (escolhasAtuais.Count > escolhas.Length)
        {
            Debug.LogWarning("Mais escolhas do que o UI suporta. Número de escolhas: " + escolhasAtuais.Count);
            return;
        }

        int index = 0;
        foreach (Choice escolha in escolhasAtuais)
        {
            escolhas[index].SetActive(true);
            escolhasTextos[index].text = escolha.text;
            index++;
        }

        for (int i = index; i < escolhas.Length; i++)
        {
            escolhas[i].SetActive(false);
        }
    }

    public void TomarEscolha(int indexEscolha)
    {
        if (indexEscolha >= 0 && indexEscolha < historiaAtual.currentChoices.Count)
        {
            historiaAtual.ChooseChoiceIndex(indexEscolha);
            estaEscolhendo = false;
            ProximaMensagem();
        }
        else
        {
            Debug.LogWarning("Índice de escolha fora do intervalo: " + indexEscolha);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && estaAtivo && !estaEscolhendo)
        {
            ProximaMensagem();
        }
        if (Input.GetKeyUp(KeyCode.Escape) && estaAtivo)
        {
            FecharDialogo();
        }
    }
}
