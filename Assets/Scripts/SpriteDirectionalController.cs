using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpriteDirectionalController : MonoBehaviour
{
    [SerializeField] Transform mainTransform;
    [SerializeField] float backAngle = 65f;
    [SerializeField] float sideAngle = 155f;    
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer SpriteRenderer;
    private void LateUpdate()
    {
        Vector3 camFowardVector = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
        Debug.DrawRay(Camera.main.transform.position, camFowardVector * 5f, Color.blue);

        float signedAngle = Vector3.SignedAngle(mainTransform.forward, camFowardVector, Vector3.up);

        Vector2 animationDirection = new Vector2(0f, -1f);

        float angle = Mathf.Abs(signedAngle);

        if(angle < backAngle)
        {
            animationDirection = new Vector2(0f, -1f);      
        }
        else if(angle < sideAngle)
        {
            animationDirection = new Vector2(1f, 0f);
        }
        else
        {
            animationDirection = new Vector2(0f, 1f);
        }

        animator.SetFloat("moveX", animationDirection.x);
        animator.SetFloat("moveY", animationDirection.y);
    }
}