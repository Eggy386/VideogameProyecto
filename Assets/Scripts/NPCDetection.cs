using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCDetection : MonoBehaviour
{
    public GameObject tasksCanvas; // Canvas de tareas
    public NPCMovement npcMovement; // Referencia al script NPCMovement
    public Button closeButton; // Bot�n de cierre
    public string playerTag = "Player"; // Tag del jugador
    private NPCTasks npcTasks; // Referencia al script NPCTasks del NPC detectado
    public TaskDisplay taskDisplay; // Referencia al TaskDisplay que maneja los slots de tareas

    private void Start()
    {
        // Configura el bot�n para cerrar el Canvas
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
        tasksCanvas.SetActive(true);
    }

    // M�todo para cerrar el Canvas y reanudar el movimiento del NPC
    public void CloseCanvasAndResumeNPC()
    {
        tasksCanvas.SetActive(false); // Cierra el Canvas

        if (npcMovement != null)
        {
            npcMovement.canMove = true; // Reactiva el movimiento del NPC
        }
    }

    private string FormatTasks(System.Collections.Generic.List<NPCTasks.Task> tasks)
    {
        string formattedText = "Tareas del NPC:\n";
        int taskNumber = 1;

        foreach (var task in tasks)
        {
            formattedText += $"Tarea {taskNumber}: {task.taskName}\n";
            formattedText += $"- Requisito: {task.requirement.quantity} {task.requirement.itemName}\n";

            foreach (var reward in task.rewards)
            {
                formattedText += $"- Recompensa: {reward.rewardName} x{reward.rewardAmount}\n";
            }

            formattedText += "\n"; // Espaciado entre tareas
            taskNumber++;
        }

        return formattedText;
    }
}

