using UnityEngine;

public class TiroProjetil : MonoBehaviour{
    public float dano;
    public float velocidade;

    private void OnTriggerEnter(Collider other) {
        if (other == null) return;

        if (other.CompareTag("Inimigo")){
            //other.gameObject.GetComponent<InimigoControle>().TomarDano(dano);
        }
    }

}