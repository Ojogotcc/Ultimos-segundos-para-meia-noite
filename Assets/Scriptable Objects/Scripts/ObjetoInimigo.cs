using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Inimigo", menuName = "Inimigo/Criar Inimigo")]
public class ObjetoInimigo : ScriptableObject
{
    //states
    public string nome;
    public float vida;
    public float velocidade;
    public string tipoInimigo;

    //attack
    public float tempoEntreAtaques;
    public float dano;
    public string tipoAtaque;
    public float distanciaAtaque;
    public float alcanceVisao;
}

public class InimigoLongoAlcance : ObjetoInimigo
{
    public GameObject projetil;
}
