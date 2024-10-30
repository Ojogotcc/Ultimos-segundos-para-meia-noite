using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public static class TrocarCameras
{   
    static List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();
    public static CinemachineVirtualCamera CameraAtual = null;

    public static bool estaCameraAtiva(CinemachineVirtualCamera camera)
    {
        return camera == CameraAtual;
    }

    public static void TrocarCamera(CinemachineVirtualCamera camera)
    {
        camera.Priority = 10;
        CameraAtual = camera;

        foreach (CinemachineVirtualCamera c in cameras){
            if (c != camera && c.Priority != 0){
                c.Priority = 0;
            }
        }
    }

    public static void AdicionarCamera(CinemachineVirtualCamera camera){
        cameras.Add(camera);
    }

    public static void RemoverCamera(CinemachineVirtualCamera camera){
        cameras.Remove(camera);
    }
}
    