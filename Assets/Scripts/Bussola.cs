using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bussola : MonoBehaviour
{
    public RawImage compass;
    public Transform player;

    
    void Update()
    {
        compass.uvRect = new Rect(player.localEulerAngles.y / 360f, 0f, 1f, 1f);
    }
}
