using UnityEngine;

public class TiroProjetil : MonoBehaviour
{
    public float dano;
    public float velocidade;
    public ObjetoTiro tiroData;

    private void Awake()
    {
        dano = tiroData.dano;
        velocidade = tiroData.velocidade;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * velocidade * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if (other.CompareTag("Inimigo"))
        {
            //other.GetComponent<InimigoControle>().TomarDano(dano);
            Destroy(gameObject);
        }
    }
}
