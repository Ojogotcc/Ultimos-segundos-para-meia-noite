using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] public string fase;
    [SerializeField] public string menuPrincipal;

    public GameObject menuConfig;
    public GameObject menuPausa;
    public PlayerControle player;

    private void Awake() {
        player = GameObject.Find("Player").GetComponent<PlayerControle>();
    }

    private void Pausar()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7"))
        {
            player.estaAtivadoMenu = true;
            menuPausa.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Despausar()
    {
        player.estaAtivadoMenu = false;
        menuPausa.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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

    void Update()
    {
        Pausar();
    }
}
