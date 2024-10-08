using UnityEngine;

public class ParallaxCanvas : MonoBehaviour
{
    public Camera mainCamera;
    [Range(0.1f, 50f)]
    public float followSpeed = 0.1f;
    [Range(0.1f, 50f)]
    public float maxOffsetX = 5f;
    [Range(0.1f, 50f)]
    public float maxOffsetY = 5f;
    [Range(0.1f, 50f)]
    public float returnSpeed = 0.5f;
    private Vector3 initialPosition;
    private Vector3 lastCameraRotation;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; 
        }

        initialPosition = transform.position;
        lastCameraRotation = mainCamera.transform.eulerAngles;
    }

    void Update()
    {
        Vector3 cameraRotation = mainCamera.transform.eulerAngles;
        Vector3 rotationDifference = cameraRotation - lastCameraRotation;

        if (rotationDifference.magnitude > 0.01f) // Define um limite para detectar movimento
        {
            Vector3 offset = CalculateParallaxOffset(rotationDifference);
            MoveCanvas(offset);
        }
        else
        {
            ReturnToInitialPosition();
        }

        lastCameraRotation = cameraRotation;
    }

    Vector3 CalculateParallaxOffset(Vector3 rotationDifference)
    {
        float horizontalOffset = Mathf.Clamp(rotationDifference.y * maxOffsetX, -maxOffsetX, maxOffsetX);
        float verticalOffset = Mathf.Clamp(rotationDifference.x * maxOffsetY, -maxOffsetY, maxOffsetY);
        return new Vector3(horizontalOffset, verticalOffset, 0);
    }

    void MoveCanvas(Vector3 offset)
    {
        Vector3 targetPosition = initialPosition + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    void ReturnToInitialPosition()
    {
        transform.position = Vector3.Lerp(transform.position, initialPosition, returnSpeed * Time.deltaTime);
    }
}