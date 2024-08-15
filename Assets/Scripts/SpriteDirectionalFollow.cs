using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDirectionalFollow : MonoBehaviour
{
    private void LateUpdate()
    {
        Vector3 camFowardVector = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
        Debug.DrawRay(Camera.main.transform.position, camFowardVector * 5f, Color.blue);
        
    }
}
