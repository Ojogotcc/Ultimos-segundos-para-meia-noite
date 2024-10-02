using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] public string Fase1;

    public GameObject menuPrincipal;
    public GameObject menuOpcoes;
    public GameObject menuCreditos;
    public GameObject menuPesquisa;

    public void AbrirJogo()
    {
        SceneManager.LoadScene(Fase1);
    }

    public void FecharJogo()
    {
        Application.Quit();
    }

    public void AbrirOpcoes()
    {
        menuPrincipal.SetActive(true);
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
