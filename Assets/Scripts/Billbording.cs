using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class Billbording : MonoBehaviour
{     
    [SerializeField] bool freezeXZ;
    private void Update()
    {
        if(freezeXZ)
        {
            transform.rotation = quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        }
        else{
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
