using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public string Fase1;
    private int painelAberto = -1;
    // private Image fundoImage;
    public TMP_Text textoJogar;
    public bool JaJogou = false;    
    public CanvasGroup fundo;
    public GameObject menuOpcoes;
    public GameObject menuPesquisa;
    public GameObject menuCreditos;    
    public GameObject[] paineis;
    public float delay = 1f;

    [Header("Efeitos")]
    public AudioClip abrirClip;
    public AudioClip fecharClip;
    [Header("Video")]
    public GameObject videoGO;
    public VideoPlayer videoPlayer;
    public GameObject pularBotao;
    public GameObject MusicaGO;

    private void Start() {
        if (PlayerPrefs.HasKey("JaJogou"))
        {
            JaJogou = true;
            textoJogar.text = "Continuar";
        }
        paineis = new GameObject[] { menuOpcoes, menuPesquisa, menuCreditos };
    }

    void Update()
    {
        if (videoPlayer.isPlaying && Input.anyKeyDown) pularBotao.SetActive(true);
    }

    public void AcabarCutscene()
    {
        PlayerPrefs.SetInt("JaJogou", 1);
        LoadingManager.instance.CarregarCena(Fase1);  
    }

    public void AbrirJogo()
    {
        EfeitoManager.instance.PlayEfeito(abrirClip, transform, 1f, 0f, 0f);
        if (JaJogou)
        {
            LoadingManager.instance.CarregarCena(Fase1);
        }
        else
        {
            MusicaGO.SetActive(false);
            videoGO.SetActive(true);
            videoPlayer.Play();
            videoPlayer.loopPointReached += VideoAcabou;
        }
    }

    void VideoAcabou(VideoPlayer vp)
    {
        PlayerPrefs.SetInt("JaJogou", 1);
        LoadingManager.instance.CarregarCena(Fase1);        
    }

    public void FecharJogo()
    {
        EfeitoManager.instance.PlayEfeito(fecharClip, transform, 1f, 0f, 0f);
        Application.Quit();
    }

    public void AbrirPainel(int painel_id)
    {
        EfeitoManager.instance.PlayEfeito(abrirClip, transform, 1f, 0f, 0f);
        GameObject painelAtual = paineis[painel_id];
        painelAberto = painel_id;
        painelAtual.transform.localScale = Vector3.zero;
        painelAtual.SetActive(true);

        fundo.alpha = 0f;
        fundo.gameObject.SetActive(true);

        LeanTween.value(gameObject, 0f, 1f, .5f).setOnUpdate((float val) => {
            fundo.alpha = val;
        });

        painelAtual.transform.LeanScale(new Vector3(1f, .05f, 0f), delay).setOnComplete(() => {
            painelAtual.transform.LeanScale(Vector3.one, delay);
        });
    }

    public void FecharPainelAtual()
    {
        if (painelAberto == -1) return;

        EfeitoManager.instance.PlayEfeito(fecharClip, transform, 1f, 0f, 0f);

        GameObject painelAtual = paineis[painelAberto];
        painelAberto = -1;

        painelAtual.transform.LeanScale(new Vector3(1f, .05f, 0f), delay).setOnComplete(() => {
            painelAtual.transform.LeanScale(Vector3.zero, delay).setOnComplete(() => {
                painelAtual.SetActive(false);

                LeanTween.value(gameObject, 1f, 0f, .5f).setOnUpdate((float val) => {
                    fundo.alpha = val;
                }).setOnComplete(() => {fundo.gameObject.SetActive(false);});
            });
        });        
    }
}
