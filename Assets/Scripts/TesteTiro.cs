using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteTiro : MonoBehaviour
{
    public float rayLength = 5.0f;
    // Cor do raio no Gizmos
    public Color rayColor = Color.red;

    void OnDrawGizmos()
    {
        // Defina a cor do Gizmo
        Gizmos.color = rayColor;

        // Posição de origem do raio (posição do objeto)
        Vector3 rayOrigin = transform.position;

        // Direção do raio (neste caso, para frente do objeto)
        Vector3 rayDirection = transform.forward;

        // Desenha o raio
        Gizmos.DrawRay(rayOrigin, rayDirection * rayLength);
    }
}
