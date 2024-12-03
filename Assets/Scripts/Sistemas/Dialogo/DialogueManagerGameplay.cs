using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using UnityEditor.ShaderGraph.Internal;

public class DialogueManagerGameplay : MonoBehaviour
{
    [Header("Dialogos")]
    public Animator charAnimator0; // Substitui Image por Animator para o personagem 0
    public TextMeshProUGUI charNome;
    public TextMeshProUGUI mensagemTexto;
    public RectTransform background;
    
    [Header("Bools")]
    public bool estaAtivo = false;
    public bool estaDigitando = false;

    [Header("Digitacao")]
    public float delayDigitar = 0.2f;
    public float delayFalas = 1f;

    private Story historiaAtual;
    private string Avatar0Inicial = null;
    private string Avatar0AparenciaInicial = null;

    [Header("UI")]
    public GameObject DialogoGameplay_GO;

    [Header("Animação")]
    public float delayAvatares = 1f;
    public float delayTexto = 1f;
    public float delayNome = 1f;
    public RectTransform Vinheta;
    public RectTransform Avatar0;
    public RectTransform Nome;
    public RectTransform Texto;

    public static DialogueManagerGameplay instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Existe mais de um DialogoManagerGameplay em cena!");
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

        if (PlayerControle.instance.estaDialogoNormal) return;

        PlayerControle.instance.estaDialogoGameplay = true;

        historiaAtual = new Story(inkJSON.text);
        estaAtivo = true;
        estaDigitando = false;

        DialogoGameplay_GO.SetActive(true);

        // Animacoes
        Vinheta.localScale = new Vector3(1f, 0f, 1f);
        Vinheta.LeanScale(new Vector3(1f, 1f, 1f), .2f);
        Avatar0.localPosition = new Vector3(-1042f, -540f, -10f);
        Avatar0.LeanMoveLocal(new Vector3(-886f, -540f, -10f), delayAvatares);
        Texto.localPosition = new Vector3(0f, -602f, 0f);
        Texto.LeanMoveLocal(new Vector3(0f, -442f, 0f), delayTexto);
        Nome.localScale = Vector3.zero;
        Nome.LeanScale(new Vector3(1f, .05f, 0f), delayNome/2).setOnComplete(() => {
            Nome.LeanScale(Vector3.one, delayNome/2);
        });


        DefinirConfiguracoesIniciais();
        ProximaMensagem();        
    }

    void DefinirConfiguracoesIniciais()
    {
        Avatar0Inicial = (string) historiaAtual.variablesState["Avatar0Inicial"];
        Avatar0AparenciaInicial = (string) historiaAtual.variablesState["Avatar0AparenciaInicial"];

        Debug.Log("Avatar0Inicial:" + Avatar0Inicial);
        Debug.Log("Avatar0AparenciaInicial:" + Avatar0AparenciaInicial);
        AplicarAparencia(0, Avatar0Inicial, Avatar0AparenciaInicial);
    }

    void AplicarAparencia(int lado, string personagem, string aparencia)
    {
        string animacaoNome = personagem + "_" + aparencia;
        if (lado == 0)
        {
            charAnimator0.Play(animacaoNome);
        }
    }

    void MostrarMensagem()
    {
        StopAllCoroutines();
        estaDigitando = false;

        string[] texto = historiaAtual.Continue().Split(":");
        charNome.text = texto[0];
        ProcessarTags(historiaAtual.currentTags);

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
    }

    IEnumerator DigitarFrase(string frase)
    {
        Debug.Log(frase);
        estaDigitando = true;

        mensagemTexto.text = "";
        mensagemTexto.maxVisibleCharacters = 0;
        mensagemTexto.text = frase;

        for (int i = 0; i <= frase.Length; i++)
        {
            mensagemTexto.maxVisibleCharacters = i;
            yield return new WaitForSeconds(delayDigitar);
        } 

        estaDigitando = false;

        Invoke(nameof(ProximaMensagem), delayFalas);
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
        DialogoGameplay_GO.SetActive(false);
    }

    public void FecharDialogo()
    {
        Nome.LeanScale(new Vector3(1f, .05f, 0f), delayNome/2).setOnComplete(() => {
            Nome.LeanScale(Vector3.zero, delayNome/2);
        });
        Texto.LeanMoveLocal(new Vector3(0f, -602f, 0f), delayTexto);
        Avatar0.LeanMoveLocal(new Vector3(-1042f, -540f, 0f), delayAvatares).setOnComplete(() => {
            Vinheta.LeanScale(new Vector3(1f, 0f, 1f), .2f);
            DialogoGameplay_GO.SetActive(false);
        });        

        estaAtivo = false;
        PlayerControle.instance.estaDialogoGameplay = false;

        Debug.Log("Diálogo foi fechado");  
    }
}
