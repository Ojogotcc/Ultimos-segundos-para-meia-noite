using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class AutoSaveCena : EditorWindow
{
    static float saveTime = 150.0f; // Intervalo de tempo para auto salvar (em segundos)
    static double nextSave = 0;

    [MenuItem("Ferramentas/AutoSave")] // Menu para ativar a janela de AutoSave
    static void Init()
    {
        AutoSaveCena window = (AutoSaveCena)GetWindowWithRect(
            typeof(AutoSaveCena),
            new Rect(0, 0, 160, 60));
        window.Show();
    }

    void OnGUI()
    {
        EditorGUI.LabelField(new Rect(10, 10, 90, 20), "Save a cada:");
        EditorGUI.LabelField(new Rect(90, 10, 80, 20), saveTime + " secs");

        double timeToSave = nextSave - EditorApplication.timeSinceStartup;

        EditorGUI.LabelField(new Rect(10, 30, 100, 20), "Proximo Save:");
        EditorGUI.LabelField(new Rect(100, 30, 80, 20), timeToSave.ToString("N1") + " secs");

        this.Repaint();

        // Salva a cena quando o tempo chegar a zero
        if (EditorApplication.timeSinceStartup > nextSave)
        {
            Scene scene = SceneManager.GetActiveScene();

            // Verifica se a cena já está salva em um caminho válido
            if (scene.isDirty) // Se a cena foi modificada
            {
                string scenePath = scene.path; // Obtém o caminho da cena atual

                // Se o caminho da cena estiver válido, salva a cena sobrescrevendo o arquivo
                if (!string.IsNullOrEmpty(scenePath))
                {
                    EditorSceneManager.SaveScene(scene, scenePath); // Sobrescreve a cena atual
                    Debug.Log("Cena salvada automaticamente em: " + scene.name);
                }
                else
                {
                    // Se a cena ainda não foi salva (cena nova), abre o dialogo de salvar
                    string newPath = EditorUtility.SaveFilePanelInProject("Salvar Cena", scene.name, "unity", "Coloque o nome da cena para ser salva:");
                    if (!string.IsNullOrEmpty(newPath))
                    {
                        EditorSceneManager.SaveScene(scene, newPath);
                        Debug.Log("Cena salvada em: " + newPath);
                    }
                }
            }

            nextSave = EditorApplication.timeSinceStartup + saveTime; // Atualiza o próximo tempo de auto salvar
        }
    }
}
