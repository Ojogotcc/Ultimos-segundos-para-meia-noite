using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControle : MonoBehaviour
{   
    public static PlayerControle instance;

    private Rigidbody RB; // Referencia ao componente Rigidbody do player
    public float velocidade; // Velocidade de movimento do player
    private Vector2 moverInput; // Armazena o input de movimento do player

    // Gravidade
    private float gravidade_total; // Armazena o valor total da gravidade aplicada ao player
    public float multiplicador_gravidade = 5.0f; // Multiplicador da gravidade para ajustar a intensidade
    public float gravidade_valor = -10; // Valor da gravidade
    public bool estaNoChao = false; // Indica se o player esta no chao
    private float checkChaoDistancia = 8f; // Distancia para verificar se o player esta no chao
    private bool podePular = true;

    // Mira e Tiro
    private bool podeAtirar = true; // Verifica se pode atirar
    private bool estaMirando = false; // Verifica se esta com a mira acionada
    public float fireRate; // Intervalo de tiros (quanto menor mais rapido)
    private float miraInput, atirarInput; // Inputs de mira e tiro
    public GameObject playerTiro; // Prefab do tiro
    public GameObject playerTiroPos; // Posicao de onde ira sair o tiro

    // Animacao
    public Animator animator; // Referancia ao componente Animator do player
    private string animacaoAtual = "Player_frente_idle"; // Armazena o estado atual da animacao do player
    
    // Primeira Pessoa - Combate
    public Camera PrimeiraCamera;
    private Transform TransformPrimeiraCamera;
    private float mouseX;
    private float mouseY;
    public float mouseSensibilidadeX;
    public float mouseSensibilidadeY;
    private float verticalLookRotation;

    void Start()
    {
        RB = GetComponent<Rigidbody>();      
    }

    void Update()
    {
        CheckChao(); // Verifica se o player esta no chao
        InputPlayer(); // Recebe os inputs do player
        MoverPlayer(moverInput); // Move o player com base nos inputs
        ChecarMiraTiro(); // recebe o input de mirar
        Animacoes(moverInput); // Atualiza as animacoes com base nos inputs     
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
            estaMirando = true;
            MirarFPS();

            if (podeAtirar && atirarInput != 0)
            {
                podeAtirar = false;
                Atirar();
                Invoke(nameof(ResetarTiro), fireRate);
            }
        }
        else
        {
            estaMirando = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void MirarFPS()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensibilidadeX);
        verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensibilidadeY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
        TransformPrimeiraCamera.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    private void ResetarTiro()
    {
        podeAtirar = true;
    }
    private void Atirar()
    {
        Instantiate(playerTiro, playerTiroPos.transform.position, transform.rotation);
    }

    private void MoverPlayer(Vector2 moverInput) // Movimentacao do player
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

        RB.velocity = new Vector3(moverInput.x * velocidade, gravidade_total, moverInput.y * velocidade);
    }

    private void Animacoes(Vector2 moverInput) // Animacoes do player
    {    
        if(estaMirando == true) 
        {
            if (moverInput.x == 0 && moverInput.y == 0)
            {
                MudarEstadoAnimacao("Player_costa_idle_aim");        
            }
            else
            {
                MudarEstadoAnimacao("Player_costa_run_aim"); // Altera para animacao de corrida esquerda
            }
        }
        else 
        {
            // Se ficar parado troca a animacao atual para idle
            if (moverInput.x == 0 && moverInput.y == 0 && animacaoAtual.Contains("run"))
            {
                string novaAnimacao = animacaoAtual.Replace("run", "idle");
                MudarEstadoAnimacao(novaAnimacao);
            }
            
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
}
