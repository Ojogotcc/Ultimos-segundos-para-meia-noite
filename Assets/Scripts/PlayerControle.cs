using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControle : MonoBehaviour

{   
    public static PlayerControle instance;

    private Rigidbody RB; // Referencia ao componente Rigidbody do player
    private SpriteRenderer Sprite; // Referencia ao componente SpriteRenderer do player
    public float velocidade; // Velocidade de movimento do player
    private Vector2 moverInput; // Armazena o input de movimento do player

    // Gravidade
    private float gravidade_total; // Armazena o valor total da gravidade aplicada ao player
    public float multiplicador_gravidade = 5.0f; // Multiplicador da gravidade para ajustar a intensidade
    public float gravidade_valor = -10; // Valor da gravidade
    public bool estaNoChao = false; // Indica se o player esta no chao
    private float checkChaoDistancia = 8f; // Distancia para verificar se o player est� no chao

    private Animator animator; // Referancia ao componente Animator do player
    private string animacaoAtual; // Armazena o estado atual da animacao do player

    // Mira
    public bool canShot; // verifica se pode atirar
    public float inputShot, fireRate, inputAim; //cria o botao de tiro, o espaco entre eles e a mira
    public Transform[] playerAim; //cria a mira
    public GameObject playerShot; // cria um gameobject para atirar
    public bool IsAim;
    public GameObject cameraMira;
    public GameObject mira;

    
    void Start()
    {
        RB = GetComponent<Rigidbody>();
        Sprite = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();

        cameraMira.SetActive(false);
        mira.SetActive(false);
    }

    void Update()
    {
        CheckChao(); // Verifica se o player esta no chao
        InputPlayer(); // Recebe os inputs do player
        MoverPlayer(moverInput); // Move o player com base nos inputs
        ChecarMira(); // recebe o input de mirar
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
        inputAim = Input.GetAxis("Fire2");
    }

    private void ChecarMira() // Verifica se o personagem está mirando
    {   
        if(inputAim != 0)
        {
            IsAim = true;
            Mirar();
            
        }
        else
        {
            IsAim = false;
            cameraMira.SetActive(false);
            mira.SetActive(false);
        }
    }

    private void Mirar()
    {
        if(IsAim)
        {
            cameraMira.SetActive(true);
            mira.SetActive(true);
        }
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

    private void Animacoes(Vector2 moverInput) // Anima��es do player
    {    
        if(IsAim == true) 
        {
            if (moverInput.x == 0 && moverInput.y == 0)
            {
                MudarEstadoAnimacao(PLAYER_COSTA_IDLE_AIM);        
            }
            if (moverInput.x == 0 && moverInput.y > 0)
            {
                MudarEstadoAnimacao(PLAYER_COSTA_RUN_AIM);
            }
            if (moverInput.x == 0 && moverInput.y < 0)
            {
                MudarEstadoAnimacao(PLAYER_COSTA_RUN_AIM);
            }
            if (moverInput.x > 0 && moverInput.y == 0)
            {
                MudarEstadoAnimacao(PLAYER_COSTA_RUN_AIM); // Altera para animacao de corrida � direita
            }
            if (moverInput.x < 0 && moverInput.y == 0)
            {
                MudarEstadoAnimacao(PLAYER_COSTA_RUN_AIM); // Altera para animacao de corrida � esquerda
            }
        }
        else 
        {
            if (moverInput.x == 0 && moverInput.y == 0)
            {
                if (animacaoAtual == "Player_frente_run" || animacaoAtual == "Player_costa_run_aim")
                {
                    MudarEstadoAnimacao("Player_frente_idle"); // Altera para animacao idle frente
                }
                else if (animacaoAtual == "Player_costa_run")
                {
                    MudarEstadoAnimacao("Player_costa_idle"); // Altera para animacao idle costas
                }
                else if (animacaoAtual == "Player_esquerda_run")
                {
                    MudarEstadoAnimacao("Player_esquerda_idle"); // Altera para animacao idle esquerda
                }
                else if (animacaoAtual == "Player_direita_run")
                {
                    MudarEstadoAnimacao("Player_direita_idle"); // Altera para animacao idle direita
                }
            }

            // Animacao Direita
            if (moverInput.x > 0 && moverInput.y == 0)
            {
                MudarEstadoAnimacao("Player_direita_run"); // Altera para animacao de corrida direita
            }
            // Animacao Esquerda
            if (moverInput.x < 0 && moverInput.y == 0)
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
            // Animacao Frente diagonal direita
            if (moverInput.x > 0 && moverInput.y < 0)
            {
                MudarEstadoAnimacao("Player_direita_run"); // Altera para animacao de corrida direita
            }
            // Animacao Frente diagonal esquerda
            if (moverInput.x < 0 && moverInput.y < 0)
            {
                MudarEstadoAnimacao("Player_esquerda_run"); // Altera para animacao de corrida esquerda
            }
            // Animacao Costas diagonal direita
            if (moverInput.x > 0 && moverInput.y > 0)
            {
                MudarEstadoAnimacao("Player_direita_run"); // Altera para animacao de corrida direita
            }
            // Animacao Costas diagonal esquerda
            if (moverInput.x < 0 && moverInput.y > 0)
            {
                MudarEstadoAnimacao("Player_esquerda_run"); // Altera para animacao de corrida esquerda
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
