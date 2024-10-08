using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogos")]
    public Animator charAnimator0; // Substitui Image por Animator para o personagem 0
    public Animator charAnimator1; // Substitui Image por Animator para o personagem 1
    public TextMeshProUGUI charNome;
    public TextMeshProUGUI mensagemTexto;
    public RectTransform background;
    public GameObject proximoUI;

    [Header("Escolhas")]
    public GameObject FundoEscolhas;
    public GameObject[] escolhas;
    public TextMeshProUGUI[] escolhasTextos;
    
    [Header("Bools")]
    public static bool estaAtivo = false;
    public static bool estaDigitando = false;
    public static bool estaEscolhendo = false;

    [Header("Digitacao")]
    public float delayDigitar = 0.1f;

    private Story historiaAtual;
    private string Avatar0Inicial = null;
    private string Avatar1Inicial = null;
    private string Avatar0AparenciaInicial = null;
    private string Avatar1AparenciaInicial = null;

    private string ultimoPersonagemFalante;

    public static DialogueManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Existe mais de um DialogoManager em cena!");
            Destroy(gameObject);
        }
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
        estaDigitando = false;

        background.localScale = Vector3.zero;

        DefinirConfiguracoesIniciais();

        ProximaMensagem();
        background.LeanScale(new Vector3(1.94f, 1.94f, 1.94f), 0.3f);
    }

    void DefinirConfiguracoesIniciais()
    {
        Avatar0Inicial = (string) historiaAtual.variablesState["Avatar0Inicial"];
        Avatar0AparenciaInicial = (string) historiaAtual.variablesState["Avatar0AparenciaInicial"];

        Debug.Log("Avatar0Inicial:" + Avatar0Inicial);
        Debug.Log("Avatar0AparenciaInicial:" + Avatar0AparenciaInicial);
        AplicarAparencia(0, Avatar0Inicial, Avatar0AparenciaInicial);

        Avatar1Inicial = (string) historiaAtual.variablesState["Avatar1Inicial"];
        Avatar1AparenciaInicial = (string) historiaAtual.variablesState["Avatar1AparenciaInicial"];

        Debug.Log("Avatar1Inicial:" + Avatar1Inicial);
        Debug.Log("Avatar1AparenciaInicial:" + Avatar1AparenciaInicial);
        AplicarAparencia(1, Avatar1Inicial, Avatar1AparenciaInicial);
    }

    void AplicarAparencia(int lado, string personagem, string aparencia)
    {
        string animacaoNome = personagem + "_" + aparencia;
        if (lado == 0)
        {
            charAnimator0.Play(animacaoNome);
        }
        else if (lado == 1)
        {
            charAnimator1.Play(animacaoNome);
        }
    }

    void MostrarMensagem()
    {
        StopAllCoroutines();
        proximoUI.SetActive(false);
        FundoEscolhas.SetActive(false);
        estaDigitando = false;
        estaEscolhendo = false;

        string[] texto = historiaAtual.Continue().Split(":");
        charNome.text = texto[0];
        ProcessarTags(historiaAtual.currentTags);

        if (texto[0] == Avatar0Inicial)
        {
            charAnimator0.transform.LeanScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
            charAnimator1.transform.LeanScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        }
        else if (texto[0] == Avatar1Inicial)
        {
            charAnimator0.transform.LeanScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
            charAnimator1.transform.LeanScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
        }             

        StartCoroutine(DigitarFrase(texto[1].Trim()));
    }

    void ProcessarTags(List<string> tags)
    {   
        if (tags.Count > 0)
        {
            foreach (string tag in tags)
            {
                if (tag.Contains("Aparencia"))
                {
                    if (tag.Contains("L0"))
                    {
                        AplicarAparencia(0, Avatar0Inicial, tag.Replace("AparenciaL0:", "").Trim());
                    }
                    else if (tag.Contains("L1"))
                    {
                        Debug.Log(tag.Replace("AparenciaL1:", "").Trim());
                        AplicarAparencia(1, Avatar1Inicial, tag.Replace("AparenciaL1:", "").Trim());
                    }
                }
            }
        }
        else
        {
            ReverterAparenciaInicial();
        }
    }

    void ReverterAparenciaInicial()
    {
        AplicarAparencia(0, Avatar0Inicial, Avatar0AparenciaInicial);
        AplicarAparencia(1, Avatar1Inicial, Avatar1AparenciaInicial);
    }

    IEnumerator DigitarFrase(string frase)
    {
        Debug.Log(frase);
        estaDigitando = true;
        proximoUI.SetActive(false);

        mensagemTexto.text = "";
        mensagemTexto.maxVisibleCharacters = 0;
        mensagemTexto.text = frase;

        for (int i = 0; i <= frase.Length; i++)
        {
            mensagemTexto.maxVisibleCharacters = i;
            yield return new WaitForSeconds(delayDigitar);
        } 

        estaDigitando = false;
        proximoUI.SetActive(true);

        MostrarEscolhas();
    }

    private void PularDigitacao()
    {
        StopAllCoroutines();
        estaDigitando = false;
        proximoUI.SetActive(true);
        mensagemTexto.maxVisibleCharacters = 1000;
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

        proximoUI.SetActive(false);
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
        Debug.Log(escolhasAtuais);
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
        if (indexEscolha >= 0 && indexEscolha <= historiaAtual.currentChoices.Count)
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && estaAtivo && !estaEscolhendo)
        {
            if (estaDigitando)
            {
                PularDigitacao();
            }
            else
            {
                ProximaMensagem();
            }            
        }
        if (Input.GetKeyUp(KeyCode.Escape) && estaAtivo)
        {
            FecharDialogo();
        }
    }
}
