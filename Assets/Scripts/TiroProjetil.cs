using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Rendering;

public class TiroProjetil : MonoBehaviour
{
    [SerializeField] private int dano;
    [SerializeField] private float velocidade;
    [SerializeField] private float duracao;
    public ObjetoTiro tiroData;

    private float duracaoAtual;

    private void Awake()
    {
        if (tiroData == null) return;
        dano = tiroData.dano;
        velocidade = tiroData.velocidade;
        duracao = tiroData.duracao;
    }

    private void Update()
    {
        duracaoAtual += Time.deltaTime;

        transform.Translate(transform.forward * velocidade * Time.deltaTime);

        if (duracaoAtual > duracao) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if (other.gameObject.CompareTag("Inimigo"))
        {
            other.gameObject.GetComponent<InimigoControle>().TomarDano(dano);
        }
    }
}
