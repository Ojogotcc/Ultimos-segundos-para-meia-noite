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

        Destroy(gameObject);

        if (other.gameObject.CompareTag("Inimigo") && tiroData.atirador == "Player")
        {            
            other.GetComponent<InimigoControle>().TomarDano(tiroData.dano);
        }
        else if (other.gameObject.CompareTag("Player") && tiroData.atirador == "Inimigo")
        {
            other.GetComponent<PlayerControle>().TomarDano(tiroData.dano);            
        }
    }
}
