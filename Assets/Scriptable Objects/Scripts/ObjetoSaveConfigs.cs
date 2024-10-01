using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Save", menuName = "Save/Configs")]
public class ObjetoSaveConfigs : ScriptableObject
{
    public float volume_musica;
    public float volume_sfx;
    public float sensibilidadeX;
    public float sensibilidadeY;
}