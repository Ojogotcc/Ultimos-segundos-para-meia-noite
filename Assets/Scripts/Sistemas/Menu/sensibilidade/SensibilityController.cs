using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensibilityController : MonoBehaviour
{
    [Header("Objetos")]
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SensibilidadeMovimentacao(float value)
    {
        float mouseXsensi = player.GetComponent<PlayerControle>().mouseSensibilidadeX;
    }

    public void SensibilidadeCamera(float value)
    {
        float mouseSensibilidadeX = player.GetComponent<PlayerControle>().mouseSensibilidadeX;
        float mouseSensibilidadeY = player.GetComponent<PlayerControle>().mouseSensibilidadeY;

        float newMouseSensibilidadeY = mouseSensibilidadeY * value;
        float newMouseSensibilidadeX = mouseSensibilidadeX * value;

        player.GetComponent<PlayerControle>().mouseSensibilidadeY = newMouseSensibilidadeY;
        player.GetComponent<PlayerControle>().mouseSensibilidadeX = newMouseSensibilidadeX;
    }
}
