using UnityEngine;
using UnityEngine.AI;

public class InimigoControle : MonoBehaviour
{
    [Header("Agentes")]
    public NavMeshAgent agent;
    private Transform player;
    public LayerMask chaoLayer, playerLayer;
    public float vida;

    [Header("Patrulha")]
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    [Header("Ataque")]
    public float intervaloEntreAtaques;
    bool jaAtacou;
    public GameObject projectile;

    public float visaoRange, ataqueRange;
    public bool playerEmVisaoRange, playerEmAtaqueRange;

    [Header("Animacao")]
    public Animator animator;
    private string animacaoAtual = "IA_frente_idle";
    public Vector3 direcao;
    public float offsetAnimacao = 30.0f;
    private Vector3 destinoTiro;
    // public ObjetoInimigo inimigoData;
    public GameObject efeitoMorte;

    [Header("Efeitos")]
    public AudioClip hitClip;
    public AudioClip ataqueClip;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = ataqueRange;
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
        if (-offsetAnimacao <= direcao.x && direcao.x <= offsetAnimacao && direcao.z > 0)
        {
            MudarEstadoAnimacao("IA_frente_run"); // Altera para animacao de corrida para frente
        }
        // Animacao Costas
        else if (-offsetAnimacao <= direcao.x && direcao.x <= offsetAnimacao && direcao.z < 0)
        {
            MudarEstadoAnimacao("IA_costa_run"); // Altera para animacao de corrida para tras
        }
        // Animacao Direita
        else if (direcao.x > offsetAnimacao)
        {
            MudarEstadoAnimacao("IA_esquerda_run"); // Altera para animacao de corrida direita
        }
        // Animacao Esquerda
        else if (direcao.x < -offsetAnimacao)
        {
            MudarEstadoAnimacao("IA_direita_run"); // Altera para animacao de corrida esquerda
        }        
    }

    private void Patrulhar()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        
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
        //agent.SetDestination(transform.position);
        
        Vector3 direcaoParaPlayer = (player.position - transform.position).normalized;
        Quaternion olharRotacao = Quaternion.LookRotation(new Vector3(direcaoParaPlayer.x, 0, direcaoParaPlayer.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, olharRotacao, Time.deltaTime * 5f);       

        if (!jaAtacou)
        {
            //MudarEstadoAnimacao("IA_frente_charge");

            AnimatorStateInfo estadoAnimacao = animator.GetCurrentAnimatorStateInfo(0);

            if (estadoAnimacao.normalizedTime >= 1f)
            {
                jaAtacou = true;

                Ray ray = new Ray(transform.position, (player.position - transform.position).normalized);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    destinoTiro = hit.point;
                }
                else
                {
                    destinoTiro = player.position;
                }

                GameObject tiro = Instantiate (projectile, transform.position, transform.rotation);
                EfeitoManager.instance.PlayEfeitoNoLocal(ataqueClip, transform, 0.5f);
                tiro.GetComponent<Rigidbody>().velocity = (destinoTiro - transform.position).normalized * projectile.GetComponent<TiroProjetil>().tiroData.velocidade;
                Invoke(nameof(ResetarAtaque), intervaloEntreAtaques);
            }            
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

        EfeitoManager.instance.PlayEfeitoNoLocal(hitClip, transform, 1f);

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
