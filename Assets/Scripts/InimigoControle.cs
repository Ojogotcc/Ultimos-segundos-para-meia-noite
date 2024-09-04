using UnityEngine;
using UnityEngine.AI;

public class InimigoControle : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask chaoLayer, playerLayer;

    // CAracteristicas
    public float vida;

    // Patrulha
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Ataque
    public float intervaloEntreAtaques;
    bool jaAtacou;
    public GameObject projectile;

    public float visaoRange, ataqueRange;
    public bool playerEmVisaoRange, playerEmAtaqueRange;

    // Animacao
    public Animator animator;
    private string animacaoAtual = "IA_frente_idle";
    public Vector3 direcao;
    public float offsetAnimacao = 20.0f;
    private Vector3 destinoTiro;
    public ObjetoInimigo inimigoData;
    public GameObject efeitoMorte;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = inimigoData.velocidade;
        agent.acceleration = inimigoData.aceleracao;
        vida = inimigoData.vida;
        intervaloEntreAtaques = inimigoData.tempoEntreAtaques;        
        visaoRange = inimigoData.alcanceVisao;
        ataqueRange = inimigoData.distanciaAtaque;
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

        // Se ficar parado troca a animacao atual para idle
        if (direcao.x == 0 && direcao.y == 0 && animacaoAtual.Contains("run")) MudarEstadoAnimacao(animacaoAtual.Replace("run", "idle"));
            
        // Animacao Frente
        if (-offsetAnimacao <= direcao.x && direcao.x <= offsetAnimacao && direcao.z < 0)
        {
            MudarEstadoAnimacao("IA_frente_run"); // Altera para animacao de corrida para frente
        }
        // Animacao Costas
        else if (-offsetAnimacao <= direcao.x && direcao.x <= offsetAnimacao && direcao.z > 0)
        {
            MudarEstadoAnimacao("IA_costa_run"); // Altera para animacao de corrida para tras
        }
        // Animacao Direita
        else if (direcao.x > offsetAnimacao)
        {
            MudarEstadoAnimacao("IA_direita_run"); // Altera para animacao de corrida direita
        }
        // Animacao Esquerda
        else if (direcao.x < -offsetAnimacao)
        {
            MudarEstadoAnimacao("IA_esquerda_run"); // Altera para animacao de corrida esquerda
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
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!jaAtacou)
        {
            jaAtacou = true;

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

            GameObject tiro = Instantiate (projectile, transform.position, transform.rotation);
            tiro.GetComponent<Rigidbody>().velocity = (destinoTiro - transform.position).normalized * projectile.GetComponent<TiroProjetil>().tiroData.velocidade;
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
        MudarEstadoAnimacao("IA_frente_hit");

        if (vida <= 0) Invoke(nameof(DestruirInimigo), 0.5f);
    }
    private void DestruirInimigo()
    {
        Instantiate(efeitoMorte, transform.position, Quaternion.identity);
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
