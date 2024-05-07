using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControle : MonoBehaviour
{
    public Rigidbody RB;
    public SpriteRenderer Sprite;
    public float velocidade;
    private Vector2 moverInput;

    // Gravidade
    private float gravidade_total;
    public float multiplicador_gravidade = 5.0f;
    public float gravidade_valor = -10;
    public bool estaNoChao = false;
    private float checkChaoDistancia = 8f;

    // Animações
    public Animator animator;
    private string estadoAtual;

    // Sprites em diversas direcoes
    const string PLAYER_FRENTE_IDLE = "Player_frente_idle";
    const string PLAYER_ESQUERDA_IDLE = "Player_esquerda_idle";
    const string PLAYER_DIREITA_IDLE = "Player_direita_idle";
    const string PLAYER_COSTA_IDLE = "Player_costa_idle";
    const string PLAYER_ESQUERDA = "Player_esquerda_run";
    const string PLAYER_DIREITA = "Player_direita_run";
    const string PLAYER_FRENTE = "Player_frente_run";
    const string PLAYER_COSTA = "Player_costa_run";

    void Start()
    {
    }

    void Update()
    {
        CheckChao();
        InputPlayer();
        MoverPlayer(moverInput);
        Animacoes(moverInput);     
    }

    private void CheckChao() // Verifica se o player está no chão
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

    private void InputPlayer() // Inputs
    {
        moverInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moverInput.Normalize();
    }

    private void MoverPlayer(Vector2 moverInput) // Movimentação do player
    {
        if (!estaNoChao)
        {
            gravidade_total += gravidade_valor * multiplicador_gravidade * Time.deltaTime;
        }
        else
        {
            gravidade_total = 0.0f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                gravidade_total += 50 ;
            }
        }
        
        RB.velocity = new Vector3(moverInput.x * velocidade, gravidade_total, moverInput.y * velocidade);
    }
    private void Animacoes(Vector2 moverInput) // Animações do player
    {
        // Anim Idle
        if (moverInput.x == 0 && moverInput.y == 0)
        {
            if (estadoAtual == PLAYER_FRENTE)
            {
                MudarEstadoAnimacao(PLAYER_FRENTE_IDLE);
            }
            else if (estadoAtual == PLAYER_ESQUERDA)
            {
                MudarEstadoAnimacao(PLAYER_ESQUERDA_IDLE);
            }
            else if (estadoAtual == PLAYER_DIREITA)
            {
                MudarEstadoAnimacao(PLAYER_DIREITA_IDLE);
            }
            else if (estadoAtual == PLAYER_COSTA)
            {
                MudarEstadoAnimacao(PLAYER_COSTA_IDLE);
            }
        }
        // Anim Direita
        if (moverInput.x > 0 && moverInput.y == 0)
        {
            MudarEstadoAnimacao(PLAYER_DIREITA);
        }
        // Anim Esqueda
        if (moverInput.x < 0 && moverInput.y == 0)
        {
            MudarEstadoAnimacao(PLAYER_ESQUERDA);
        }
        // Anim Frente normal
        if (moverInput.x == 0 && moverInput.y < 0)
        {
            MudarEstadoAnimacao(PLAYER_FRENTE);
        }
        // Anim Costa normal
        if (moverInput.x == 0 && moverInput.y > 0)
        {
            MudarEstadoAnimacao(PLAYER_COSTA);
        }
        // Anim Frente diagonais
        if (moverInput.x > 0 && moverInput.y < 0)
        {
            MudarEstadoAnimacao(PLAYER_DIREITA);
        }
        if (moverInput.x < 0 && moverInput.y < 0)
        {
            MudarEstadoAnimacao(PLAYER_ESQUERDA);
        }
        // Anim Costa diagonais
        if (moverInput.x > 0 && moverInput.y > 0)
        {
            MudarEstadoAnimacao(PLAYER_DIREITA);
        }
        if (moverInput.x < 0 && moverInput.y > 0)
        {
            MudarEstadoAnimacao(PLAYER_ESQUERDA);
        }
    }

    private void MudarEstadoAnimacao(string novoEstado) // Função para alternar as animações do player
    {
        if (estadoAtual == novoEstado) return;

        animator.Play(novoEstado);
        estadoAtual = novoEstado;
    }
}
