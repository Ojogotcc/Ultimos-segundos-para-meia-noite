using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] public string Fase1;

    public bool JaJogou = false;
    private int painelAberto = -1;
    public GameObject menuOpcoes;
    public GameObject menuPesquisa;
    public GameObject menuCreditos;    
    public GameObject[] paineis;

    public float delay = 1f;

    private void Start() {
        if (PlayerPrefs.HasKey("JaJogou")) JaJogou = true;
        paineis = new GameObject[] { menuOpcoes, menuPesquisa, menuCreditos };
    }

    public void AbrirJogo()
    {
        if (!JaJogou) SceneManager.LoadScene(Fase1);
    }

    public void FecharJogo()
    {
        Application.Quit();
    }

    public void AbrirPainel(int painel_id)
    {
        GameObject painelAtual = paineis[painel_id];
        painelAberto = painel_id;
        painelAtual.transform.localScale = Vector3.zero;
        painelAtual.SetActive(true);
        painelAtual.transform.LeanScale(Vector3.one, delay);
    }

    public void FecharPainelAtual()
    {
        if (painelAberto == -1) return;

        GameObject painelAtual = paineis[painelAberto];
        painelAberto = -1;
        painelAtual.transform.LeanScale(Vector3.zero, delay).setOnComplete(() => {
            painelAtual.SetActive(false);
        });
    }
}
