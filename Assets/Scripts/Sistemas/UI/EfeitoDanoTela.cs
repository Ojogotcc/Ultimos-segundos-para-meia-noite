using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfeitoDanoTela : MonoBehaviour
{
    public static EfeitoDanoTela instance;
    public Material material;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Existe mais de um EfeitoDanoTela em cena!");
            return;
        }
        if (instance == null) instance = this;
    }

    public string floatPropertyName = "_Intensidade"; 
    public float speed = 1.0f;

    public float minValue = 0.0f;
    public float maxValue = 1.0f;

    void Update()
    {
        if (material != null && material.HasProperty(floatPropertyName))
        {
            float pingPongValue = Mathf.PingPong(Time.time * speed, maxValue - minValue) + minValue;
            material.SetFloat(floatPropertyName, pingPongValue);
        }
        else
        {
            Debug.LogWarning("Material ou propriedade de float não encontrada.");
        }
    }
}
