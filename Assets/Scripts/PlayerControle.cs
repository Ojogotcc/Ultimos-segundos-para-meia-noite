using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControle : MonoBehaviour
{   
    [Header("Movimentacao")]
    public float velocidade_andando; // Velocidade de movimento do player
    public float velocidade_correndo; // Velocidade de movimento do player
    public float velocidade_mirando;
    private float velocidade_atual; // Velocidade de movimento do player
    public float multiplicador_gravidade = 5f; // Multiplicador da gravidade para ajustar a intensidade
    public float forca_pulo = 20f;
    public float gravidade_valor = -10; // Valor da gravidade
    public bool estaNoChao = false; // Indica se o player esta no chao
    private Rigidbody RB; // Referencia ao componente Rigidbody do player    
    private Vector2 moverInput; // Armazena o input de movimento do player
    private float gravidade_total; // Armazena o valor total da gravidade aplicada ao player    
    public float checkChaoDistancia = 8f; // Distancia para verificar se o player esta no chao
    private bool podePular = true;

    [Header("Ataque")]
    public GameObject playerTiro; // Prefab do tiro
    public GameObject playerTiroPos; // Posicao de onde ira sair o tiro
    public float fireRate; // Intervalo de tiros (quanto menor mais rapido)
    private bool podeAtirar = true; // Verifica se pode atirar
    public bool podeMover = true; // Verifica se pode atirar
    private bool estaMirando = false; // Verifica se esta com a mira acionada    
    private float miraInput, atirarInput; // Inputs de mira e tiro
    private Vector3 destinoTiro;

    [Header("Animacao")]
    public Animator animator; // Referancia ao componente Animator do player
    private string animacaoAtual = "Player_frente_idle"; // Armazena o estado atual da animacao do player
    
    [Header("FPS")]
    public GameObject GOTerceiraCamera;
    public GameObject GOMiraCamera;
    public float mouseSensibilidadeX;
    public float mouseSensibilidadeY;
    public GameObject miraCanvas;
    private float mouseX;
    private float mouseY;
    private float verticalLookRotation;

    [Header("UI")]
    public float vidaMaxima = 100;
    public float vidaAtual;
    public float energiaMaxima = 100;
    public float energiaAtual;
    public int gastoportiro = 5;
    [SerializeField] public Image vidadelay;
    [SerializeField] public Image vida;
    [SerializeField] public Image energia;
    [SerializeField] public Image energiadelay;

    [Header("Cameras")]
    public CinemachineVirtualCamera cameraTerceiraPessoa;
    public CinemachineVirtualCamera cameraMiraPessoa;

    public bool estaAtivadoMenu = false;

    [Header("Efeitos")]
    public AudioClip hitClip;
    public AudioClip ataqueClip;
    public GameObject efeitodanotela;

    private void OnEnable()
    {
        TrocarCameras.AdicionarCamera(cameraTerceiraPessoa);
        TrocarCameras.AdicionarCamera(cameraMiraPessoa);
    }

    private void OnDisable()
    {
        TrocarCameras.RemoverCamera(cameraTerceiraPessoa);
        TrocarCameras.RemoverCamera(cameraMiraPessoa);
    }

    void Start()
    {
        RB = GetComponent<Rigidbody>();        
        vidaAtual = vidaMaxima;
        energiaAtual = energiaMaxima;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {             
        CheckChao(); // Verifica se o player esta no chao
        InputPlayer(); // Recebe os inputs do player
        MoverPlayer(); // Move o player com base nos inputs
        ChecarMiraTiro(); // recebe o input de mirar
        MovimentacaoCamera();
        Animacoes(); // Atualiza as animacoes com base nos inputs     
    }
    public void MudarSensibilidadeCamera(float value)
    {
        mouseSensibilidadeY = value;
        mouseSensibilidadeX = value;
    }

    public void MudarSensibilidadeMovimento(float value)
    {
        velocidade_atual = velocidade_atual * value;
        mouseSensibilidadeY = 1;
        mouseSensibilidadeX = 1;
    }

    private void MoverPlayer() // Movimentacao do player
    {
        if (podeMover)
        {
            if (!estaNoChao)
            {
                gravidade_total += gravidade_valor * multiplicador_gravidade * Time.deltaTime; // Aplica a gravidade se nao estiver no chao
            }
            else
            {
                gravidade_total = 0.0f; // Reseta a gravidade quando esta no chao

                if (Input.GetKeyDown(KeyCode.Space) && podePular == true)
                {
                    gravidade_total += forca_pulo; // Aplica impulso para pular
                }
            }

            if (estaMirando)
                velocidade_atual = velocidade_mirando;

            else
                velocidade_atual = velocidade_andando;

            Vector3 movimento = transform.TransformDirection(new Vector3(moverInput.x * velocidade_atual, 0, moverInput.y * velocidade_atual));

            RB.velocity = new Vector3(movimento.x, gravidade_total, movimento.z);
        }
    }

    public void DesabilitarPulo()
    {
        podePular = false;
    }

    public void HabilitarPulo()
    {
        podePular = true;
    }

    public void DesabilitarMovimento()
    {
        podeMover = false;
    }

    public void HabilitarMovimento()
    {
        podeMover = true;
    }

    private void CheckChao() // Verifica se o player esta no chao
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, checkChaoDistancia))
        {
            if (hit.collider.CompareTag("Chao"))
            {
                estaNoChao = true;    
            }
            else
            {
                estaNoChao = false;
            }
        }
        else
        {
            estaNoChao = false;
        }
    }   

    private void InputPlayer()
    {
        moverInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moverInput.Normalize(); // Normaliza os inputs para garantir movimento consistente
        atirarInput = Input.GetAxis("Fire1");
        miraInput = Input.GetAxis("Fire2");      
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");        
    }

    private void ChecarMiraTiro() // Verifica se o personagem está mirando
    {   
        if(miraInput != 0)
        {
            if (TrocarCameras.estaCameraAtiva(cameraTerceiraPessoa)) TrocarCameras.TrocarCamera(cameraMiraPessoa);
            
            estaMirando = true;
            miraCanvas.SetActive(true);

            if (podeAtirar && atirarInput != 0 && energiaAtual > gastoportiro)
            {
                podeAtirar = false;
                AtirarRaio();
                Invoke(nameof(ResetarTiro), fireRate);
            }            
        }
        else
        {
            estaMirando = false;
            // Cursor.visible = true;
            // Cursor.lockState = CursorLockMode.None;
            miraCanvas.SetActive(false);

            TrocarCameras.TrocarCamera(cameraTerceiraPessoa);
            // transform.localEulerAngles = Vector3.zero;
        }        
    }

    private void MovimentacaoCamera()
    {
        if (!estaAtivadoMenu)
        {
            transform.Rotate(Vector3.up * mouseX * mouseSensibilidadeX);
            
            if (estaMirando)
            {
                verticalLookRotation += mouseY * mouseSensibilidadeY;
                verticalLookRotation = Mathf.Clamp(verticalLookRotation, -30, 45);


                GOMiraCamera.transform.localEulerAngles = Vector3.left * verticalLookRotation;

                playerTiroPos.transform.Rotate(Vector3.up * mouseX* mouseSensibilidadeX);
                playerTiroPos.transform.localEulerAngles = Vector3.left * verticalLookRotation;
            }
            else
            {
                GOTerceiraCamera.transform.localEulerAngles = Vector3.left * verticalLookRotation;
            }
        }       
    }

    private void ResetarTiro()
    {
        podeAtirar = true;
    }

    private void AtirarRaio()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            destinoTiro = hit.point;
        }
        else
        {
            destinoTiro = ray.GetPoint(200);
        }

        GameObject tiro = Instantiate (playerTiro, playerTiroPos.transform.position, playerTiroPos.transform.rotation);
        EfeitoManager.instance.PlayEfeito(ataqueClip, transform, .25f, 0f);
        tiro.GetComponent<Rigidbody>().velocity = (destinoTiro - transform.position).normalized * playerTiro.GetComponent<TiroProjetil>().tiroData.velocidade;
        
        energiaAtual -= gastoportiro;   
        energia.fillAmount = (energiaAtual / energiaMaxima);
        StartCoroutine(DelayBarras(energiadelay, energia, 1f));
    }

    private void Animacoes() // Animacoes do player
    {    
        if (estaMirando)
        {
            if (moverInput.x == 0 && moverInput.y == 0) MudarEstadoAnimacao("Player_costa_idle_aim"); else if (moverInput.x != 0 && moverInput.y != 0) MudarEstadoAnimacao("Player_costa_run_aim"); 
        }
        else
        {
            // Se ficar parado troca a animacao atual para idle
            if (moverInput.x == 0 && moverInput.y == 0 && animacaoAtual.Contains("run")) MudarEstadoAnimacao(animacaoAtual.Replace("run", "idle"));
            
            // Animacao Direita; Frente diagonal direita; Costas diagonal direita
            if ((moverInput.x > 0 && moverInput.y == 0) || (moverInput.x > 0 && moverInput.y < 0) || (moverInput.x > 0 && moverInput.y > 0))
            {
                MudarEstadoAnimacao("Player_direita_run"); // Altera para animacao de corrida direita
            }
            // Animacao Esquerda; Frente diagonal esquerda; Costas diagonal esquerda
            if ((moverInput.x < 0 && moverInput.y == 0) || (moverInput.x < 0 && moverInput.y < 0) || (moverInput.x < 0 && moverInput.y > 0))
            {
                MudarEstadoAnimacao("Player_esquerda_run"); // Altera para animacao de corrida esquerda
            }
            // Animacao Frente
            if (moverInput.x == 0 && moverInput.y < 0)
            {
                MudarEstadoAnimacao("Player_frente_run"); // Altera para animacao de corrida para frente
            }
            // Animacao Costas
            if (moverInput.x == 0 && moverInput.y > 0)
            {
                MudarEstadoAnimacao("Player_costa_run"); // Altera para animacao de corrida para tras
            }
        }        
    }

    private void MudarEstadoAnimacao(string animacaoNova) // Funcao para alternar as animacoes do player
    {
        if (animacaoAtual == animacaoNova) return; // Se o estado atual == o novo estado, mantem o msm

        animator.Play(animacaoNova); // Reproduz a nova animacao
        animacaoAtual = animacaoNova; // Atualiza o estado atual
    }

    public void TomarDano(int dano)
    {
        vidaAtual -= dano;

        EfeitoManager.instance.PlayEfeito(hitClip, transform, 1f, 0f);

        vida.fillAmount = (vidaAtual / vidaMaxima);
        StartCoroutine(DelayBarras(vidadelay, vida, 1f));

        if (vidaAtual < 30f) efeitodanotela.SetActive(true); else efeitodanotela.SetActive(false);

        if (vidaAtual <= 0)
        {
            SceneManager.LoadScene("Fase1");
            // Destroy(gameObject);
        }
    }

    private IEnumerator DelayBarras(Image delay, Image normal, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        LeanTween.value(delay.fillAmount, normal.fillAmount, .5f).setOnUpdate((float val) =>
        {
            delay.fillAmount = val;
        });
    }
}