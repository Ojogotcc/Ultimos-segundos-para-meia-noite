using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;
    public CanvasGroup loadingGrupo;
    public GameObject CanvasLoading;
    public Image barraProgresso;
    private float porcentagemProgesso;
    private AsyncOperation cena;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Existe mais de um LoadingManager em cena!");
            Destroy(gameObject);
        }
    }

    private void Start() {
        CanvasLoading.SetActive(false);
    }

    public async void CarregarCena(string nomeCena)
    {
        cena = SceneManager.LoadSceneAsync(nomeCena);
        cena.allowSceneActivation = false;
        barraProgresso.fillAmount = 0f;
        loadingGrupo.alpha = 0f;
        CanvasLoading.SetActive(true);

        LeanTween.value(gameObject, 0f, 1f, .5f).setOnUpdate((float val) => {
            loadingGrupo.alpha = val;
        });

        do
        {
            await Task.Delay(100);
            porcentagemProgesso = cena.progress;
        } while (cena.progress < .9f);
        
        await Task.Delay(1000);
        cena.allowSceneActivation = true;

        LeanTween.value(gameObject, 1f, 0f, 1f).setOnUpdate((float val) => {
            loadingGrupo.alpha = val;
        }).setOnComplete(() => {
            CanvasLoading.SetActive(false);
            cena = null;
            // Destroy(gameObject);
        });       
    }

    void Update()
    {
        if (cena != null)
        {
            LeanTween.value(gameObject, barraProgresso.fillAmount, porcentagemProgesso+.1f, .5f)
            .setOnUpdate((float val) => {
                barraProgresso.fillAmount = val;
            });
        }
    }
}
