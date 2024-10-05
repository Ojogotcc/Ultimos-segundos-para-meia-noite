using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public string Fase1;
    private int painelAberto = -1;
    private Image fundoImage;
    public bool JaJogou = false;    
    public GameObject fundoGO;
    public GameObject menuOpcoes;
    public GameObject menuPesquisa;
    public GameObject menuCreditos;    
    public GameObject[] paineis;
    public float delay = 1f;

    [Header("Efeitos")]
    public AudioClip abrirClip;
    public AudioClip fecharClip;

    private void Start() {
        if (PlayerPrefs.HasKey("JaJogou")) JaJogou = true;
        paineis = new GameObject[] { menuOpcoes, menuPesquisa, menuCreditos };
        fundoImage = fundoGO.GetComponent<Image>();
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
        EfeitoManager.instance.PlayEfeito(abrirClip, transform, 1f, 0f, 0f);
        GameObject painelAtual = paineis[painel_id];
        painelAberto = painel_id;
        painelAtual.transform.localScale = Vector3.zero;
        painelAtual.SetActive(true);

        Color color = fundoImage.color;
        color.a = 0f;
        fundoImage.color = color;
        fundoGO.SetActive(true);

        LeanTween.value(gameObject, 0f, 0.941176f, 0.1f).setOnUpdate((float val) => {
            Color newColor = fundoImage.color;
            newColor.a = val;
            fundoImage.color = newColor;
        });

        painelAtual.transform.LeanScale(new Vector3(1f, 0.05f, 0f), delay).setOnComplete(() => {
            painelAtual.transform.LeanScale(Vector3.one, delay);
        });
    }

    public void FecharPainelAtual()
    {
        if (painelAberto == -1) return;

        EfeitoManager.instance.PlayEfeito(fecharClip, transform, 1f, 0f, 0f);

        GameObject painelAtual = paineis[painelAberto];
        painelAberto = -1;

        painelAtual.transform.LeanScale(new Vector3(1f, 0.05f, 0f), delay).setOnComplete(() => {
            painelAtual.transform.LeanScale(Vector3.zero, delay).setOnComplete(() => {
                painelAtual.SetActive(false);

                LeanTween.value(gameObject, 1f, 0f, 0.1f).setOnUpdate((float val) => {
                    Color newColor = fundoImage.color;
                    newColor.a = val;
                    fundoImage.color = newColor;
                }).setOnComplete(() => {fundoGO.SetActive(false);});
            });
        });        
    }
}
