using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalManager : MonoBehaviour
{
    public static FinalManager instance;

    void Awake()
    {
        instance = this;
    }

    public GameObject Final_GO;
    public GameObject Parallax_GO;
    public void Acabou()
    {
        Time.timeScale = 0f;
        Parallax_GO.SetActive(false);
        Final_GO.SetActive(true);
    }
}
