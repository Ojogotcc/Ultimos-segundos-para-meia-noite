using UnityEngine;

public class TiroProjetil : MonoBehaviour
{
    public ObjetoTiro tiroData;

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if (other.gameObject.CompareTag("Inimigo"))
        {
            other.GetComponent<InimigoControle>().TomarDano(tiroData.dano);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerControle>().TomarDano(tiroData.dano);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
