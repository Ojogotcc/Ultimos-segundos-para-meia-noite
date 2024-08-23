using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class NPC : MonoBehaviour
{
    public DialogueTrigger trigger;
    public GameObject icone;
    public static bool dialogoAtivado = false;

    public GameObject player;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            icone.SetActive(true);
            icone.transform.localScale = Vector3.zero;
            icone.transform.LeanScale(new Vector3(0.3f, 0.3f, 0.3f), 0.2f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && icone.activeSelf && !dialogoAtivado)
        {
            icone.transform.LeanScale(Vector3.zero, 0.2f);
            trigger.IniciarDialogo();
            dialogoAtivado = true;
            player.GetComponent<PlayerControle>().DesabilitarPulo();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            icone.transform.LeanScale(Vector3.zero, 0.2f);
            if (dialogoAtivado)
            {
                trigger.FinalizarDialogo();
                dialogoAtivado = false;
                player.GetComponent<PlayerControle>().HabilitarPulo();
            }
        }
    }
}
