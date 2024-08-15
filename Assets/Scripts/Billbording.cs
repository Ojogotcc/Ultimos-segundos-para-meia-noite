using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billbording : MonoBehaviour
{

    [SerializeField] bool freezeXZ;
    void Update()
    {
        if(freezeXZ)
        {
            transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        }
        else
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
