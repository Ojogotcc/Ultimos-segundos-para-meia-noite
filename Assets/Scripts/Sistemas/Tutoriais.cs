using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutoriais : MonoBehaviour
{
    [Header("GameObjects")]

    public GameObject canva;
    public TMP_Text textoTutorial;
    public string[] tutorial;
    private int numero;

    void Start()
    {
        canva.SetActive(true);

        textoTutorial.text = tutorial[numero];

        if(Input.GetKeyDown(KeyCode.Return))
        {
            numero+=1;
        }
    }

    
    


}
