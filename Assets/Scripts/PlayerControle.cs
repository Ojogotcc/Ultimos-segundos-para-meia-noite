using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControle : MonoBehaviour
{   
    public static PlayerControle instance;
    public Rigidbody RB; // Refer�ncia ao componente Rigidbody do player
    public SpriteRenderer Sprite; // Refer�ncia ao componente SpriteRenderer do player
    public float velocidade; // Velocidade de movimento do player
    private Vector2 moverInput; // Armazena o input de movimento do player

    // Gravidade
    private float gravidade_total; // Armazena o valor total da gravidade aplicada ao player
    public float multiplicador_gravidade = 5.0f; // Multiplicador da gravidade para ajustar a intensidade
    public float gravidade_valor = -10; // Valor da gravidade
    public bool estaNoChao = false; // Indica se o player est� no ch�o
    private float checkChaoDistancia = 8f; // Dist�ncia para verificar se o player est� no ch�o

    // Anima��es
    public Animator animator; // Refer�ncia ao componente Animator do player
    private string estadoAtual; // Armazena o estado atual da anima��o do player

    // Sprites em diversas dire��es
    const string PLAYER_FRENTE_IDLE = "Player_frente_idle"; // Estado de idle na frente
    const string PLAYER_ESQUERDA_IDLE = "Player_esquerda_idle"; // Estado de idle � esquerda
    const string PLAYER_DIREITA_IDLE = "Player_direita_idle"; // Estado de idle � direita
    const string PLAYER_COSTA_IDLE = "Player_costa_idle"; // Estado de idle de costas
    const string PLAYER_ESQUERDA = "Player_esquerda_run"; // Estado de corrida � esquerda
    const string PLAYER_DIREITA = "Player_direita_run"; // Estado de corrida � direita
    const string PLAYER_FRENTE = "Player_frente_run"; // Estado de corrida na frente
    const string PLAYER_COSTA = "Player_costa_run"; // Estado de corrida de costas
    const string PLAYER_DIREITA_TIRO = "Player_direita_tiro";
    const string PLAYER_ESQUERDA_TIRO = "Player_esquerda_tiro";
    const string PLAYER_COSTA_IDLE_AIM = "Player_costa_idle_aim";
    const string PLAYER_COSTA_RUN_AIM = "Player_costa_run_aim";

    public bool canShot; // verifica se pode atirar
    public float inputShot, fireRate, inputAim; //cria o botao de tiro, o espaco entre eles e a mira
    public Transform[] playerAim; //cria a mira
    public GameObject playerShot; // cria um gameobject para atirar
    public bool IsAim;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Inicializa��o do script
    }

    void Update()
    {
        CheckChao(); // Verifica se o player est� no ch�o
        InputPlayer(); // Recebe os inputs do player
        MoverPlayer(moverInput); // Move o player com base nos inputs
        ChecarMira(); // recebe o input de mirar
        Animacoes(moverInput); // Atualiza as anima��es com base nos inputs     
    }

    private void CheckChao() // Verifica se o player est� no ch�o
    {
        if (Physics.Raycast(transform.position, Vector3.down, checkChaoDistancia))
        {
            estaNoChao = true; // Player est� no ch�o
        }
        else
        {
            estaNoChao = false; // Player n�o est� no ch�o
        }
    }

    private void InputPlayer() // Inputs do player
    {
        moverInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // Obt�m os inputs de movimento
        moverInput.Normalize(); // Normaliza os inputs para garantir movimento consistente
        inputAim = Input.GetAxis("Fire2");
    }

    private void ChecarMira() // verifica se o personagem está mirando
    {   
        if(inputAim != 0)
        {
            IsAim = true;
            Mirar();
            
        }
        else
        {
            IsAim = false;
        }
    }

    private void Mirar()
    {
    }

    private void MoverPlayer(Vector2 moverInput) // Movimenta��o do player
    {
        if (!estaNoChao)
        {
            gravidade_total += gravidade_valor * multiplicador_gravidade * Time.deltaTime; // Aplica a gravidade se n�o estiver no ch�o
        }
        else
        {
            gravidade_total = 0.0f; // Reseta a gravidade quando est� no ch�o

            if (Input.GetKeyDown(KeyCode.Space))
            {
                gravidade_total += 50; // Aplica impulso para pular
            }
        }

        RB.velocity = new Vector3(moverInput.x * velocidade, gravidade_total, moverInput.y * velocidade); // Aplica movimento ao Rigidbody do player
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

        }
        else 
        {
            if (moverInput.x == 0 && moverInput.y == 0)
            {
                if (estadoAtual == PLAYER_FRENTE)
                {
                    MudarEstadoAnimacao(PLAYER_FRENTE_IDLE); // Altera para anima��o idle frente
                }
                else if(estadoAtual == PLAYER_COSTA_IDLE)
                {
                    MudarEstadoAnimacao(PLAYER_FRENTE);
                }
                else if(estadoAtual == PLAYER_COSTA_RUN_AIM)
                {
                    MudarEstadoAnimacao(PLAYER_FRENTE);
                }
                else if (estadoAtual == PLAYER_ESQUERDA)
                {
                    MudarEstadoAnimacao(PLAYER_ESQUERDA_IDLE); // Altera para anima��o idle esquerda
                }
                else if (estadoAtual == PLAYER_DIREITA)
                {
                    MudarEstadoAnimacao(PLAYER_DIREITA_IDLE); // Altera para anima��o idle direita
                }
                else if (estadoAtual == PLAYER_COSTA)
                {
                    MudarEstadoAnimacao(PLAYER_COSTA_IDLE); // Altera para anima��o idle costas
                }
            }

            // Anima��o Direita
            if (moverInput.x > 0 && moverInput.y == 0)
            {
                MudarEstadoAnimacao(PLAYER_DIREITA); // Altera para anima��o de corrida � direita
            }
            // Anima��o Esquerda
            if (moverInput.x < 0 && moverInput.y == 0)
            {
                MudarEstadoAnimacao(PLAYER_ESQUERDA); // Altera para anima��o de corrida � esquerda
            }
            // Anima��o Frente
            if (moverInput.x == 0 && moverInput.y < 0)
            {
                MudarEstadoAnimacao(PLAYER_FRENTE); // Altera para anima��o de corrida para frente
            }
            // Anima��o Costas
            if (moverInput.x == 0 && moverInput.y > 0)
            {
                MudarEstadoAnimacao(PLAYER_COSTA); // Altera para anima��o de corrida para tr�s
            }
            // Anima��o Frente diagonal direita
            if (moverInput.x > 0 && moverInput.y < 0)
            {
                MudarEstadoAnimacao(PLAYER_DIREITA); // Altera para anima��o de corrida � direita
            }
            // Anima��o Frente diagonal esquerda
            if (moverInput.x < 0 && moverInput.y < 0)
            {
                MudarEstadoAnimacao(PLAYER_ESQUERDA); // Altera para anima��o de corrida � esquerda
            }
            // Anima��o Costas diagonal direita
            if (moverInput.x > 0 && moverInput.y > 0)
            {
                MudarEstadoAnimacao(PLAYER_DIREITA); // Altera para anima��o de corrida � direita
            }
            // Anima��o Costas diagonal esquerda
            if (moverInput.x < 0 && moverInput.y > 0)
            {
                MudarEstadoAnimacao(PLAYER_ESQUERDA); // Altera para anima��o de corrida � esquerda
            }
        }
    }


    private void MudarEstadoAnimacao(string novoEstado) // Fun��o para alternar as anima��es do player
    {
        if (estadoAtual == novoEstado) return; // Se o estado atual j� � o novo estado, n�o faz nada

        animator.Play(novoEstado); // Reproduz a nova anima��o
        estadoAtual = novoEstado; // Atualiza o estado atual
    }
}
