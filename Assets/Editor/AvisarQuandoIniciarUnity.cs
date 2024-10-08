using UnityEngine;
using UnityEditor;

[InitializeOnLoad] // Isso faz com que o código seja executado quando o editor iniciar
public class AvisarQuandoIniciarUnity
{
    static AvisarQuandoIniciarUnity()
    {
        if (!SessionState.GetBool("FirstInitDone", false))
        {
            EditorUtility.DisplayDialog(
                "Avisos", // Título do diálogo
                "Caso queira usar a cutscene (video) terá que adicionar manualmente, o github não aceita arquivos grandes no repo. Ass:Filipe", // Mensagem
                "Blz" // Texto do botão
            );
          
            SessionState.SetBool("FirstInitDone", true);
        }        
    }
}
