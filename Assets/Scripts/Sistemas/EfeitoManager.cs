using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfeitoManager : MonoBehaviour
{
    public static EfeitoManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Existe mais de um EfeitoManager em cena!");
            return;
        }
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
