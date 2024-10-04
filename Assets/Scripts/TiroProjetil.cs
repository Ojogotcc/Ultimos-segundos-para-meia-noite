using UnityEngine;

public class TiroProjetil : MonoBehaviour
{
    public ObjetoTiro tiroData;

    private void Start()
    {
        Destroy(gameObject, tiroData.duracao);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;                   

        if (tiroData.atirador == "Player")
        {
            Destroy(gameObject);
            if (tiroData.efeitoImpacto) Instantiate(tiroData.efeitoImpacto, transform.position, Quaternion.identity);

            if (other.gameObject.CompareTag("Inimigo"))
            {                       
                other.GetComponent<InimigoControle>().TomarDano(tiroData.dano);
            }
        }
        if (other.gameObject.CompareTag("Player") && tiroData.atirador == "Inimigo")
        {
            other.GetComponent<PlayerControle>().TomarDano(tiroData.dano);            
        }
    }
}
