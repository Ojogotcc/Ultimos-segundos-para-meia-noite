using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] public string fase;

    public GameObject menuPrincipal;
    public GameObject menuOpcoes;
    public GameObject menuCreditos;
    public GameObject menuPesquisa;
    public void AbrirJogo()
    {
        SceneManager.LoadScene(fase);
    }

    public void FecharJogo()
    {
        Debug.Log("Sair do jogo");
        Application.Quit();
    }

    public void AbrirOpcoes()
    {
        menuPrincipal.SetActive(false);
        menuOpcoes.SetActive(true);
    }

    public void FecharOpcoes()
    {
        menuOpcoes.SetActive(false);
        menuPrincipal.SetActive(true);
    }

    public void AbrirCreditos()
    {
        menuPrincipal.SetActive(false);
        menuCreditos.SetActive(true);
    }

    public void FecharCreditos()
    {
        menuCreditos.SetActive(false);
        menuPrincipal.SetActive(true);
    }

    public void AbrirPesquisa()
    {
        menuPrincipal.SetActive(false);
        menuPesquisa.SetActive(true);
    }

    public void FecharPesquisa()
    {
        menuPesquisa.SetActive(false);
        menuPrincipal.SetActive(true);
    }
}
