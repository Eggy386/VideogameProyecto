using System.Collections.Generic;
using UnityEngine;

public class NPCTasks : MonoBehaviour
{
    [System.Serializable]
    public class TaskRequirement
    {
        public string itemName; // Nombre del objeto (ejemplo: zanahoria)
        public int quantity;    // Cantidad requerida
    }

    [System.Serializable]
    public class TaskReward
    {
        public string rewardName; // Nombre de la recompensa
        public int rewardAmount;  // Cantidad de la recompensa
        public GameObject rewardIcon; // Ícono de la recompensa (opcional)
    }

    [System.Serializable]
    public class Task
    {
        public GameObject icon;                    // Ícono de la tarea
        public string taskName;                // Nombre de la tarea
        public TaskRequirement requirement;    // Único requisito
        public List<TaskReward> rewards;       // Lista de recompensas
        public int currentAmount;              // Progreso actual
        public int totalAmount;                // Total necesario
        public bool isCompleted;               // Estado de la tarea
    }

    public List<Task> tasks = new List<Task>();

    public void UpdateTaskProgress(string taskName, int amount)
    {
        foreach (var task in tasks)
        {
            if (task.taskName == taskName)
            {
                task.currentAmount += amount;

                if (task.currentAmount >= task.totalAmount)
                {
                    task.currentAmount = task.totalAmount;
                    task.isCompleted = true;
                    Debug.Log($"Tarea completada: {taskName}");
                }
                return;
            }
        }
    }

    public List<TaskReward> GetTaskRewards(string taskName)
    {
        foreach (var task in tasks)
        {
            if (task.taskName == taskName)
            {
                if (task.isCompleted)
                    return task.rewards;
                else
                    Debug.Log($"La tarea {taskName} aún no está completada.");
            }
        }
        Debug.Log($"Tarea {taskName} no encontrada.");
        return null;
    }
}
