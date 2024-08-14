using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControle : MonoBehaviour
{
    public Rigidbody RB; // Refer�ncia ao componente Rigidbody do player
    public SpriteRenderer Sprite; // Refer�ncia ao componente SpriteRenderer do player
    public float velocidade; // Velocidade de movimento do player
    private Vector2 moverInput; // Armazena o input de movimento do player

    // Gravidade
    public float multiplicador_gravidade = 5.0f; 
    public float gravidade_valor = -10;
    private float gravidade_total; // multiplicador * valor
    // Pulo
    public bool estaNoChao = false; // Indica se o player esta no chao
    private float checkChaoDistancia = 8f; // Distancia para verificar se o player esta no chao

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

    void Start()
    {
        // Inicializa��o do script
    }

    void Update()
    {
        CheckChao(); // Verifica se o player esta no chao
        InputPlayer(); // Recebe os inputs do player
        MoverPlayer(moverInput); // Move o player com base nos inputs
        Animacoes(moverInput); // Atualiza as anima��es com base nos inputs     
    }

    private void CheckChao()
    {
        if (Physics.Raycast(transform.position, Vector3.down, checkChaoDistancia))
        {
            estaNoChao = true;
        }
        else
        {
            estaNoChao = false;
        }
    }

    private void InputPlayer()
    {
        moverInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // Obt�m os inputs de movimento
        moverInput.Normalize(); // Normaliza os inputs para garantir movimento consistente
    }

    private void MoverPlayer(Vector2 moverInput)
    {
        if (!estaNoChao)
        {
            gravidade_total += gravidade_valor * multiplicador_gravidade * Time.deltaTime; // Aplica a gravidade se nao estiver no chao
        }
        else
        {
            gravidade_total = 0.0f; // Reseta a gravidade quando esta no chao

            if (Input.GetKeyDown(KeyCode.Space))
            {
                gravidade_total += 50; // Aplica impulso para pular
            }
        }

        RB.velocity = new Vector3(moverInput.x * velocidade, gravidade_total, moverInput.y * velocidade);
    }

    private void Animacoes(Vector2 moverInput) // Anima��es do player
    {
        // Anima��o Idle (parado)
        if (moverInput.x == 0 && moverInput.y == 0)
        {
            if (estadoAtual == PLAYER_FRENTE)
            {
                MudarEstadoAnimacao(PLAYER_FRENTE_IDLE); // Altera para anima��o idle frente
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

    private void MudarEstadoAnimacao(string animacaoNova) // Funcao para alternar as animacoes do player
    {
        if (animacaoAtual == animacaoNova) return; // Se a animacaoAtual == animacaoNova entao continua

        animator.Play(animacaoNova); // Reproduz a nova Animacaoo
        animacaoAtual = animacaoNova; // Atualiza o estado atual
    }
}
