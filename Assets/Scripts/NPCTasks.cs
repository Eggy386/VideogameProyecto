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

    public GameObject carrotSeedPrefab; // Prefab para las semillas de zanahoria
    public GameObject tomatoSeedPrefab;

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

    public bool CanCompleteTask(NPCTasks.Task task)
    {
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();

        int itemCount = inventoryManager.GetItemCount("Toolbar", task.requirement.itemName);

        if (itemCount >= task.requirement.quantity)
        {
            Debug.Log($"Puedes completar la tarea: {task.taskName}");
            return true; // Tarea completable
        }

        Debug.Log($"No tienes suficientes {task.requirement.itemName} para completar la tarea: {task.taskName}");
        return false; // No puedes completar la tarea
    }

    public void CheckTaskRequirements()
    {
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();

        int carrotCount = inventoryManager.GetItemCount("Toolbar", "Carrot");
        int tomatoCount = inventoryManager.GetItemCount("Toolbar", "Tomato");

        Debug.Log($"Tienes {carrotCount} zanahorias y {tomatoCount} tomates en el Toolbar.");
    }

    public void UpdateTaskProgressBasedOnInventory()
    {
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();

        foreach (var task in tasks)
        {
            // Obtén la cantidad actual del ítem requerido en el inventario
            int itemCount = inventoryManager.GetItemCount("Toolbar", task.requirement.itemName);

            // Actualiza el progreso actual de la tarea
            task.currentAmount = itemCount;

            // Marca la tarea como completada si se cumple el requisito
            if (task.currentAmount >= task.requirement.quantity)
            {
                task.isCompleted = true;
            }
            else
            {
                task.isCompleted = false;
            }
        }
    }

    public void GrantTaskRewards(Task task)
    {
        if (!task.isCompleted)
        {
            Debug.Log($"La tarea {task.taskName} aún no está completada. No se pueden otorgar recompensas.");
            return;
        }

        foreach (var reward in task.rewards)
        {
            GameObject rewardPrefab = null;

            // Determina el prefab correspondiente a la recompensa
            if (reward.rewardName == "CarrotSeeds")
                rewardPrefab = carrotSeedPrefab;
            else if (reward.rewardName == "TomatoSeeds")
                rewardPrefab = tomatoSeedPrefab;

            if (rewardPrefab != null)
            {
                // Genera las recompensas en posiciones aleatorias cercanas al NPC
                for (int i = 0; i < reward.rewardAmount; i++)
                {
                    Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
                    Instantiate(rewardPrefab, spawnPosition, Quaternion.identity);
                }

                Debug.Log($"Recompensa otorgada: {reward.rewardAmount} x {reward.rewardName}");
            }
            else
            {
                Debug.LogWarning($"Prefab no encontrado para la recompensa: {reward.rewardName}");
            }
        }
    }

    public void CompleteTask(string taskName)
    {
        foreach (var task in tasks)
        {
            if (task.taskName == taskName && task.isCompleted)
            {
                GrantTaskRewards(task);
                Debug.Log($"Recompensas otorgadas para la tarea: {taskName}");
                return;
            }
        }

        Debug.LogWarning($"No se encontró la tarea completada con el nombre: {taskName}");
    }
}
