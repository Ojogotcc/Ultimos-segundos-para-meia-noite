using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private AxisState xAxis;
    [SerializeField] private AxisState yAxis;
    [SerializeField] private Transform lookAt;

    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        lookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
    }
}
