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

        if (other.gameObject.CompareTag("Inimigo") && tiroData.atirador == "Player")
        {            
            Destroy(gameObject);
            other.GetComponent<InimigoControle>().TomarDano(tiroData.dano);
        }
        else if (other.gameObject.CompareTag("Player") && tiroData.atirador == "Inimigo")
        {
            Destroy(gameObject);
            other.GetComponent<PlayerControle>().TomarDano(tiroData.dano);            
        }
    }
}
