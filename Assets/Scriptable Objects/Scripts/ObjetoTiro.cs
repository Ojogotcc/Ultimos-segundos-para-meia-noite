using UnityEngine;

[CreateAssetMenu(fileName = "Tiro", menuName = "Tiros/Criar Tiro")]
public class ObjetoTiro : ScriptableObject
{
    public float velocidade;
    public int dano;
    public float duracao;
    public Sprite sprite;
}
