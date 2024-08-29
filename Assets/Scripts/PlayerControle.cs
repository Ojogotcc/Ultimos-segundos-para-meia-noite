using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class PlayerControle : MonoBehaviour
{   
    [Header("Movimentacao")]
    public float velocidade; // Velocidade de movimento do player
    public float multiplicador_gravidade = 5.0f; // Multiplicador da gravidade para ajustar a intensidade
    public float gravidade_valor = -10; // Valor da gravidade
    public bool estaNoChao = false; // Indica se o player esta no chao
    private Rigidbody RB; // Referencia ao componente Rigidbody do player    
    private Vector2 moverInput; // Armazena o input de movimento do player
    private float gravidade_total; // Armazena o valor total da gravidade aplicada ao player    
    private float checkChaoDistancia = 8f; // Distancia para verificar se o player esta no chao
    private bool podePular = true;

    [Header("Ataque")]
    public GameObject playerTiro; // Prefab do tiro
    public GameObject playerTiroPos; // Posicao de onde ira sair o tiro
    public float fireRate; // Intervalo de tiros (quanto menor mais rapido)
    private bool podeAtirar = true; // Verifica se pode atirar
    private bool estaMirando = false; // Verifica se esta com a mira acionada    
    private float miraInput, atirarInput; // Inputs de mira e tiro
    private Vector3 destinoTiro;

    [Header("Animacao")]
    public Animator animator; // Referancia ao componente Animator do player
    private string animacaoAtual = "Player_frente_idle"; // Armazena o estado atual da animacao do player
    
    [Header("FPS")]
    public GameObject GOPrimeiraCamera;
    public float mouseSensibilidadeX;
    public float mouseSensibilidadeY;
    public GameObject miraCanvas;
    private float mouseX;
    private float mouseY;
    private float verticalLookRotation;
    public GameObject arma;
    public AudioSource tiroBox;
    public AudioClip tiroAudio;

    [Header("Cameras")]
    public CinemachineVirtualCamera cameraTerceiraPessoa;
    public CinemachineVirtualCamera cameraPrimeiraPessoa;
    public CinemachineVirtualCamera cameraAnimada;
    public float duracaoAnimacao;
    
    private void OnEnable()
    {
        TrocarCameras.AdicionarCamera(cameraTerceiraPessoa);
        TrocarCameras.AdicionarCamera(cameraPrimeiraPessoa);
        TrocarCameras.AdicionarCamera(cameraAnimada);
    }

    private void OnDisable() {
        TrocarCameras.RemoverCamera(cameraTerceiraPessoa);
        TrocarCameras.RemoverCamera(cameraPrimeiraPessoa);
        TrocarCameras.RemoverCamera(cameraAnimada);
    }

    void Start()
    {
        RB = GetComponent<Rigidbody>();  

        TrocarCameras.estaCameraAtiva(cameraAnimada);    

        StartCoroutine(Animacao());  
    }

    void Update()
    {
        CheckChao(); // Verifica se o player esta no chao
        InputPlayer(); // Recebe os inputs do player
        MoverPlayer(); // Move o player com base nos inputs
        ChecarMiraTiro(); // recebe o input de mirar
        Animacoes(); // Atualiza as animacoes com base nos inputs     
    }

    public void DesabilitarPulo()
    {
        podePular = false;
    }

    public void HabilitarPulo()
    {
        podePular = true;
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
            if (TrocarCameras.estaCameraAtiva(cameraTerceiraPessoa)) TrocarCameras.TrocarCamera(cameraPrimeiraPessoa);
            
            estaMirando = true;
            MirarFPS();

            if (podeAtirar && atirarInput != 0)
            {
                podeAtirar = false;
                AtirarRaio();
                Invoke(nameof(ResetarTiro), fireRate);
            }            
        }
        else
        {
            estaMirando = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            miraCanvas.SetActive(false);
            arma.SetActive(false);

            TrocarCameras.TrocarCamera(cameraTerceiraPessoa);
            transform.localEulerAngles = Vector3.zero;
        }        
    }

    private void MirarFPS()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        miraCanvas.SetActive(true);
        arma.SetActive(true);

        transform.Rotate(Vector3.up * mouseX* mouseSensibilidadeX);
        verticalLookRotation += mouseY * mouseSensibilidadeY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
        GOPrimeiraCamera.transform.localEulerAngles = Vector3.left * verticalLookRotation;

        playerTiroPos.transform.Rotate(Vector3.up * mouseX* mouseSensibilidadeX);
        playerTiroPos.transform.localEulerAngles = Vector3.left * verticalLookRotation;
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
        tiro.GetComponent<Rigidbody>().velocity = (destinoTiro - transform.position).normalized * playerTiro.GetComponent<TiroProjetil>().tiroData.velocidade;
        tiroBox.PlayOneShot(tiroAudio);
    }

    private void MoverPlayer() // Movimentacao do player
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
                gravidade_total += 50; // Aplica impulso para pular
            }
        }

        Vector3 movimento = transform.TransformDirection(new Vector3(moverInput.x * velocidade, 0, moverInput.y * velocidade));
        RB.velocity = new Vector3(movimento.x, gravidade_total, movimento.z);
    }

    private void Animacoes() // Animacoes do player
    {    
        if(estaMirando == true) 
        {
            // if (moverInput.x == 0 && moverInput.y == 0)
            // {
            //     MudarEstadoAnimacao("Player_costa_idle_aim");        
            // }
            // else
            // {
            //     MudarEstadoAnimacao("Player_costa_run_aim"); // Altera para animacao de corrida esquerda
            // }

            MudarEstadoAnimacao("Player_costa_idle_aim");
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

    public IEnumerator Animacao()
    {
        yield return new WaitForSeconds(duracaoAnimacao);
        TrocarCameras.TrocarCamera(cameraTerceiraPessoa);
    }

    private void MudarEstadoAnimacao(string animacaoNova) // Funcao para alternar as animacoes do player
    {
        if (animacaoAtual == animacaoNova) return; // Se o estado atual == o novo estado, mantem o msm

        animator.Play(animacaoNova); // Reproduz a nova animacao
        animacaoAtual = animacaoNova; // Atualiza o estado atual
    }
}