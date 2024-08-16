using UnityEngine;
using UnityEngine.AI;

public class InimgoControle : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask chaoLayer, playerLayer;

    public float vida;

    //Patrulha
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float intervaloEntreAtaques;
    bool jaAtacou;
    public GameObject projectile;

    public float visaoRange, ataqueRange;
    public bool playerEmVisaoRange, playerEmAtaqueRange;

    // Animacao
    public Animator animator;
    private string animacaoAtual;

    public Vector3 direcao;
    public float offsetAnimacao = 4.0f;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerEmVisaoRange = Physics.CheckSphere(transform.position, visaoRange, playerLayer);
        playerEmAtaqueRange = Physics.CheckSphere(transform.position, ataqueRange, playerLayer);

        if (!playerEmVisaoRange && !playerEmAtaqueRange) Patrulhar();
        if (playerEmVisaoRange && !playerEmAtaqueRange) PerseguirPlayer();
        if (playerEmAtaqueRange && playerEmVisaoRange) AtacarPlayer();
        Animacoes();
    }

    private void Animacoes(){
        direcao = agent.desiredVelocity;

        if (direcao.x == 0 && direcao.z == 0) MudarEstadoAnimacao("IA_frente_idle");

        if ((direcao.x > 0 && direcao.z == 0) || (direcao.x > 0 && direcao.z < 0) || (direcao.x > 0 && direcao.z > 0))
        {
            MudarEstadoAnimacao("IA_direita_run"); // Altera para animacao de corrida direita
        }
        // Animacao Esquerda; Frente diagonal esquerda; Costas diagonal esquerda
        if ((direcao.x < 0 && direcao.z == 0) || (direcao.x < 0 && direcao.z < 0) || (direcao.x < 0 && direcao.z > 0))
        {
            MudarEstadoAnimacao("IA_esquerda_run"); // Altera para animacao de corrida esquerda
        }
         // Animacao Frente
        if (-offsetAnimacao <= direcao.x && direcao.x <= +offsetAnimacao && direcao.z < 0)
        {
            MudarEstadoAnimacao("IA_frente_run"); // Altera para animacao de corrida para frente
        }
        // Animacao Costas
        if (direcao.x == 0 && direcao.z > 0)
        {
            MudarEstadoAnimacao("IA_costa_run"); // Altera para animacao de corrida para tras
        }
    }

    private void Patrulhar()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, chaoLayer))
            walkPointSet = true;
    }

    private void PerseguirPlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AtacarPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!jaAtacou)
        {
            jaAtacou = true;
            Invoke(nameof(ResetarAtaque), intervaloEntreAtaques);
        }
    }
    private void ResetarAtaque()
    {
        jaAtacou = false;
    }

    public void TomarDano(int dano)
    {
        vida -= dano;

        if (vida <= 0) Invoke(nameof(DestruirInimigo), 0.5f);
    }
    private void DestruirInimigo()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ataqueRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visaoRange);
    }

    private void MudarEstadoAnimacao(string animacaoNova) // Funcao para alternar as animacoes do player
    {
        if (animacaoAtual == animacaoNova) return; // Se o estado atual == o novo estado, mantem o msm

        animator.Play(animacaoNova); // Reproduz a nova animacao
        animacaoAtual = animacaoNova; // Atualiza o estado atual
    }
}
