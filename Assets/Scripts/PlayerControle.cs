using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControle : MonoBehaviour
{
    public Rigidbody RB; // Referência ao componente Rigidbody do player
    public SpriteRenderer Sprite; // Referência ao componente SpriteRenderer do player
    public float velocidade; // Velocidade de movimento do player
    private Vector2 moverInput; // Armazena o input de movimento do player

    // Gravidade
    private float gravidade_total; // Armazena o valor total da gravidade aplicada ao player
    public float multiplicador_gravidade = 5.0f; // Multiplicador da gravidade para ajustar a intensidade
    public float gravidade_valor = -10; // Valor da gravidade
    public bool estaNoChao = false; // Indica se o player está no chão
    private float checkChaoDistancia = 8f; // Distância para verificar se o player está no chão

    // Animações
    public Animator animator; // Referência ao componente Animator do player
    private string estadoAtual; // Armazena o estado atual da animação do player

    // Sprites em diversas direções
    const string PLAYER_FRENTE_IDLE = "Player_frente_idle"; // Estado de idle na frente
    const string PLAYER_ESQUERDA_IDLE = "Player_esquerda_idle"; // Estado de idle à esquerda
    const string PLAYER_DIREITA_IDLE = "Player_direita_idle"; // Estado de idle à direita
    const string PLAYER_COSTA_IDLE = "Player_costa_idle"; // Estado de idle de costas
    const string PLAYER_ESQUERDA = "Player_esquerda_run"; // Estado de corrida à esquerda
    const string PLAYER_DIREITA = "Player_direita_run"; // Estado de corrida à direita
    const string PLAYER_FRENTE = "Player_frente_run"; // Estado de corrida na frente
    const string PLAYER_COSTA = "Player_costa_run"; // Estado de corrida de costas

    void Start()
    {
        // Inicialização do script
    }

    void Update()
    {
        CheckChao(); // Verifica se o player está no chão
        InputPlayer(); // Recebe os inputs do player
        MoverPlayer(moverInput); // Move o player com base nos inputs
        Animacoes(moverInput); // Atualiza as animações com base nos inputs     
    }

    private void CheckChao() // Verifica se o player está no chão
    {
        if (Physics.Raycast(transform.position, Vector3.down, checkChaoDistancia))
        {
            estaNoChao = true; // Player está no chão
        }
        else
        {
            estaNoChao = false; // Player não está no chão
        }
    }

    private void InputPlayer() // Inputs do player
    {
        moverInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // Obtém os inputs de movimento
        moverInput.Normalize(); // Normaliza os inputs para garantir movimento consistente
    }

    private void MoverPlayer(Vector2 moverInput) // Movimentação do player
    {
        if (!estaNoChao)
        {
            gravidade_total += gravidade_valor * multiplicador_gravidade * Time.deltaTime; // Aplica a gravidade se não estiver no chão
        }
        else
        {
            gravidade_total = 0.0f; // Reseta a gravidade quando está no chão

            if (Input.GetKeyDown(KeyCode.Space))
            {
                gravidade_total += 50; // Aplica impulso para pular
            }
        }

        RB.velocity = new Vector3(moverInput.x * velocidade, gravidade_total, moverInput.y * velocidade); // Aplica movimento ao Rigidbody do player
    }

    private void Animacoes(Vector2 moverInput) // Animações do player
    {
        // Animação Idle (parado)
        if (moverInput.x == 0 && moverInput.y == 0)
        {
            if (estadoAtual == PLAYER_FRENTE)
            {
                MudarEstadoAnimacao(PLAYER_FRENTE_IDLE); // Altera para animação idle frente
            }
            else if (estadoAtual == PLAYER_ESQUERDA)
            {
                MudarEstadoAnimacao(PLAYER_ESQUERDA_IDLE); // Altera para animação idle esquerda
            }
            else if (estadoAtual == PLAYER_DIREITA)
            {
                MudarEstadoAnimacao(PLAYER_DIREITA_IDLE); // Altera para animação idle direita
            }
            else if (estadoAtual == PLAYER_COSTA)
            {
                MudarEstadoAnimacao(PLAYER_COSTA_IDLE); // Altera para animação idle costas
            }
        }
        // Animação Direita
        if (moverInput.x > 0 && moverInput.y == 0)
        {
            MudarEstadoAnimacao(PLAYER_DIREITA); // Altera para animação de corrida à direita
        }
        // Animação Esquerda
        if (moverInput.x < 0 && moverInput.y == 0)
        {
            MudarEstadoAnimacao(PLAYER_ESQUERDA); // Altera para animação de corrida à esquerda
        }
        // Animação Frente
        if (moverInput.x == 0 && moverInput.y < 0)
        {
            MudarEstadoAnimacao(PLAYER_FRENTE); // Altera para animação de corrida para frente
        }
        // Animação Costas
        if (moverInput.x == 0 && moverInput.y > 0)
        {
            MudarEstadoAnimacao(PLAYER_COSTA); // Altera para animação de corrida para trás
        }
        // Animação Frente diagonal direita
        if (moverInput.x > 0 && moverInput.y < 0)
        {
            MudarEstadoAnimacao(PLAYER_DIREITA); // Altera para animação de corrida à direita
        }
        // Animação Frente diagonal esquerda
        if (moverInput.x < 0 && moverInput.y < 0)
        {
            MudarEstadoAnimacao(PLAYER_ESQUERDA); // Altera para animação de corrida à esquerda
        }
        // Animação Costas diagonal direita
        if (moverInput.x > 0 && moverInput.y > 0)
        {
            MudarEstadoAnimacao(PLAYER_DIREITA); // Altera para animação de corrida à direita
        }
        // Animação Costas diagonal esquerda
        if (moverInput.x < 0 && moverInput.y > 0)
        {
            MudarEstadoAnimacao(PLAYER_ESQUERDA); // Altera para animação de corrida à esquerda
        }
    }

    private void MudarEstadoAnimacao(string novoEstado) // Função para alternar as animações do player
    {
        if (estadoAtual == novoEstado) return; // Se o estado atual já é o novo estado, não faz nada

        animator.Play(novoEstado); // Reproduz a nova animação
        estadoAtual = novoEstado; // Atualiza o estado atual
    }
}
