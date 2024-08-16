using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ColisaoChuva : MonoBehaviour
{
    // Start is called before the first frame update    
    public GameObject Chuva;

    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            Debug.Log("Chuva");
            Chuva.SetActive(true);
        }
    }
    void OnTriggerExit(Collider collision)
    {
        Chuva.SetActive(false);
    }
}
