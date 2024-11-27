using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCDetection : MonoBehaviour
{
    public GameObject tasksCanvas; // Canvas de tareas
    public NPCMovement npcMovement; // Referencia al script NPCMovement
    public Button closeButton; // Botón de cierre
    public string playerTag = "Player"; // Tag del jugador
    private NPCTasks npcTasks; // Referencia al script NPCTasks del NPC detectado
    public TaskDisplay taskDisplay; // Referencia al TaskDisplay que maneja los slots de tareas

    private void Start()
    {
        // Configura el botón para cerrar el Canvas
        closeButton.onClick.AddListener(CloseCanvasAndResumeNPC);
    }

    public void OpenCanvas(NPCTasks detectedNpcTasks)
    {
        if (detectedNpcTasks == null)
        {
            Debug.LogError("NPC Tasks is null!");
            return;
        }

        npcTasks = detectedNpcTasks;

        // Detener movimiento del NPC
        if (npcMovement != null)
        {
            npcMovement.canMove = false;
        }

        // Mostrar las tareas en el Canvas usando TaskDisplay
        taskDisplay.DisplayTasks(npcTasks.tasks);
        npcTasks.UpdateTaskProgressBasedOnInventory();
        tasksCanvas.SetActive(true);
    }

    // Método para cerrar el Canvas y reanudar el movimiento del NPC
    public void CloseCanvasAndResumeNPC()
    {
        tasksCanvas.SetActive(false); // Cierra el Canvas

        if (npcMovement != null)
        {
            npcMovement.canMove = true; // Reactiva el movimiento del NPC
        }
    }
}

