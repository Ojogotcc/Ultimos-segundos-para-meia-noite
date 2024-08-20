using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] public string fase;
    [SerializeField] public string menuPrincipal;

    public GameObject menuConfig;
    public GameObject menuPausa;

    private bool gameplayAtivada;

    private void Pausar()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuPausa.SetActive(true);
            Time.timeScale = 0f;
            gameplayAtivada = false;
        }
    }

    public void Despausar()
    {
        menuPausa.SetActive(false);
        gameplayAtivada=true;
        Time.timeScale = 1;
    }

    public void AbrirConfig()
    {
        menuPausa.SetActive(false);
        menuConfig.SetActive(true);
    }

    public void FecharConfig()
    {
        menuPausa.SetActive(true);
        menuConfig.SetActive(false);
    }

    public void IrParaMenuPrinc()
    {
        SceneManager.LoadScene(menuPrincipal);
        Time.timeScale = 1;
    }

    public void SairJogo()
    {
        Debug.Log("Sair do jogo");
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameplayAtivada = true;
    }

    // Update is called once per frame
    void Update()
    {
        Pausar();
    }
}
