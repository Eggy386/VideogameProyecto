using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TaskDisplay : MonoBehaviour
{
    [SerializeField] private GameObject taskSlotPrefab; // Prefab del Slot
    [SerializeField] private Transform taskSlotContainer; // Contenedor del GridLayoutGroup
    [SerializeField] private List<GameObject> taskSlots = new List<GameObject>(); // Lista de Slots visuales

    public void DisplayTasks(List<NPCTasks.Task> tasks)
    {
        int taskCount = tasks.Count;

        // Si hay más slots de los que necesitamos, desactiva los extras
        while (taskSlots.Count > taskCount)
        {
            taskSlots[taskSlots.Count - 1].SetActive(false); // Desactivar slots adicionales
            taskSlots.RemoveAt(taskSlots.Count - 1);
        }

        // Si hay menos slots de los que necesitamos, crea nuevos
        while (taskSlots.Count < taskCount)
        {
            GameObject newTaskSlot = Instantiate(taskSlotPrefab, taskSlotContainer);
            taskSlots.Add(newTaskSlot);
        }

        // Actualiza los textos y otros datos de los slots existentes
        for (int i = 0; i < taskCount; i++)
        {
            GameObject taskSlot = taskSlots[i];
            TaskSlot taskSlotComponent = taskSlot.GetComponent<TaskSlot>();

            if (taskSlotComponent != null)
            {
                // Configura el slot con la tarea actual
                taskSlotComponent.Setup(tasks[i]);
                taskSlot.SetActive(true); // Asegúrate de que el slot esté activo
                Debug.Log("TaskSlot actualizado con la tarea: " + tasks[i].taskName);
            }
            else
            {
                Debug.LogError("No se pudo obtener el componente TaskSlot del prefab.");
            }
        }
    }
}
