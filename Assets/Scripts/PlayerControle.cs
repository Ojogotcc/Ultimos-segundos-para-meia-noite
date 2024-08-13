using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControle : MonoBehaviour
{
    private Rigidbody RB;
    private SpriteRenderer Sprite;
    public float velocidade; // Velocidade de movimento do player
    private Vector2 moverInput; // Armazena o input de movimento do player

    // Gravidade
    public float multiplicador_gravidade = 5.0f; 
    public float gravidade_valor = -10;
    private float gravidade_total; // multiplicador * valor
    // Pulo
    public bool estaNoChao = false; // Indica se o player esta no chao
    private float checkChaoDistancia = 8f; // Distancia para verificar se o player esta no chao

    // Animacoes
    public Animator animator; // Referencia ao componente Animator do player
    private string animacaoAtual; 

    void Start()
    {
        RB = GetComponent<Rigidbody>();
        Sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        CheckChao(); // Verifica se o player esta no chao
        InputPlayer(); // Recebe os inputs do player
        MoverPlayer(moverInput); // Move o player com base nos inputs
        Animacoes(moverInput); // Atualiza as animacoes com base nos inputs     
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
        moverInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moverInput.Normalize(); // Normaliza os inputs para garantir movimento ideal
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

    private void Animacoes(Vector2 moverInput) // Animacaoes do player
    {
        // Animacaoo Idle (parado) ele retem as animações
        if (moverInput.x == 0 && moverInput.y == 0)
        {
            if (animacaoAtual == "Player_frente_idle")
            {
                MudarEstadoAnimacao("Player_frente_idle"); // Altera para animacao idle frente
            }
            else if (animacaoAtual == "Player_esquerda_idle")
            {
                MudarEstadoAnimacao("Player_esquerda_idle"); // Altera para animacao idle esquerda
            }
            else if (animacaoAtual == "Player_direita_idle")
            {
                MudarEstadoAnimacao("Player_direita_idle"); // Altera para animacao idle direita
            }
            else if (animacaoAtual == "Player_costa_idle")
            {
                MudarEstadoAnimacao("Player_costa_idle"); // Altera para animacao idle costas
            }
        }
        // Animacaoo Direita
        if (moverInput.x > 0 && moverInput.y == 0)
        {
            MudarEstadoAnimacao("Player_direita_run"); // Altera para animacao de corrida direita
        }
        // Animacaoo Esquerda
        if (moverInput.x < 0 && moverInput.y == 0)
        {
            MudarEstadoAnimacao("Player_esquerda_run"); // Altera para animacao de corrida esquerda
        }
        // Animacaoo Frente
        if (moverInput.x == 0 && moverInput.y < 0)
        {
            MudarEstadoAnimacao("Player_frente_run"); // Altera para animacao de corrida para frente
        }
        // Animacaoo Costas
        if (moverInput.x == 0 && moverInput.y > 0)
        {
            MudarEstadoAnimacao("Player_costa_run"); // Altera para animacao de corrida para tras
        }
        // Animacaoo Frente diagonal direita
        if (moverInput.x > 0 && moverInput.y < 0)
        {
            MudarEstadoAnimacao("Player_direita_run"); // Altera para animacao de corrida direita
        }
        // Animacaoo Frente diagonal esquerda
        if (moverInput.x < 0 && moverInput.y < 0)
        {
            MudarEstadoAnimacao("Player_esquerda_run"); // Altera para animacao de corrida esquerda
        }
        // Animacaoo Costas diagonal direita
        if (moverInput.x > 0 && moverInput.y > 0)
        {
            MudarEstadoAnimacao("Player_direita_run"); // Altera para animacao de corrida direita
        }
        // Animacaoo Costas diagonal esquerda
        if (moverInput.x < 0 && moverInput.y > 0)
        {
            MudarEstadoAnimacao("Player_esquerda_run"); // Altera para animacao de corrida esquerda
        }
    }

    private void MudarEstadoAnimacao(string animacaoNova) // Funcao para alternar as animacoes do player
    {
        if (animacaoAtual == animacaoNova) return; // Se a animacaoAtual == animacaoNova entao continua

        animator.Play(animacaoNova); // Reproduz a nova Animacaoo
        animacaoAtual = animacaoNova; // Atualiza o estado atual
    }
}
