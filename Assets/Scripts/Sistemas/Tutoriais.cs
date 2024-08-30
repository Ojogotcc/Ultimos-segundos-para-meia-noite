using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutoriais : MonoBehaviour
{
    [Header("GameObjects")]

    public GameObject canva;
    public TMP_Text textoTutorial;
    public string[] tutorial;
    public CinemachineVirtualCamera cameraTerceiraPessoa;
    public int actualTips;


    void Start()
    {

    }

    void Initialize()
    {
        nextTips(actualTips);
    }

     void Update()
    {
         if(Input.GetKeyDown(KeyCode.Return))
        {
            checkEnter();
        }
    }


    public void nextTips(int numero)
    {
        textoTutorial.text = tutorial[numero];
        actualTips = numero;
    }

    private void checkEnter()
    {
        StartCoroutine(PassarTutorial());
    }
    
    public IEnumerator PassarTutorial()
    {
        actualTips++;

        yield return new WaitForSeconds(0.1f);
    }
    
}
