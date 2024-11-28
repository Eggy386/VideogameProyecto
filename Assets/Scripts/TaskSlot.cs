using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI taskNameText;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private GameObject requirementIcon; // Ícono del requisito
    [SerializeField] private GameObject rewardIcon; // Ícono de la recompensa
    [SerializeField] private TextMeshProUGUI requirementText; // Texto del requisito
    [SerializeField] private Button completeTaskButton;

    private NPCTasks.Task currentTask;

    public void Setup(NPCTasks.Task task)
    {
        currentTask = task; // Guarda la referencia de la tarea actual
        taskNameText.text = task.taskName;
        requirementText.text = $"{task.currentAmount}/{task.requirement.quantity}";

        requirementText.color = task.isCompleted ? Color.green : Color.red;

        if (task.icon != null)
        {
            requirementIcon.GetComponent<Image>().sprite = task.icon.GetComponent<SpriteRenderer>().sprite;
        }

        string rewardsSummary = "";
        foreach (var reward in task.rewards)
        {
            rewardsSummary += $"{reward.rewardAmount}\n";
            if (reward.rewardIcon != null)
            {
                rewardIcon.GetComponent<Image>().sprite = reward.rewardIcon.GetComponent<SpriteRenderer>().sprite;
            }
        }
        rewardText.text = $"x{rewardsSummary}";

        if (completeTaskButton != null)
        {
            completeTaskButton.onClick.AddListener(CompleteTask); // Asocia el evento al botón
        }
        else
        {
            Debug.LogError("CompleteTaskButton no está asignado en TaskSlot.");
        }
    }

    private void CompleteTask()
    {
        if (currentTask.isCompleted)
        {
            UI_Manager uiManager = FindObjectOfType<UI_Manager>();
            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
            NPCTasks npcTasks = FindObjectOfType<NPCTasks>();

            // Eliminar la cantidad requerida de la Toolbar
            inventoryManager.RemoveItem("Toolbar", currentTask.requirement.itemName, currentTask.requirement.quantity);

            // Procesar las recompensas
            foreach (var reward in currentTask.rewards)
            {
                // Obtener el inventario de la Toolbar
                Inventory toolbar = inventoryManager.GetInventoryByName("Toolbar");

                // Buscar un slot existente con el mismo ítem
                bool itemAdded = false;
                foreach (var slot in toolbar.slots)
                {
                    if (slot.itemName == reward.rewardName)
                    {
                        // Si el ítem ya existe, incrementar la cantidad
                        slot.count += reward.rewardAmount;
                        Debug.Log($"Recompensa añadida al slot existente: {reward.rewardName} x{reward.rewardAmount}");
                        itemAdded = true;
                        break; // Salimos del bucle si ya se añadió
                    }
                }

                // Si no se encontró el ítem, buscar un slot vacío
                if (!itemAdded)
                {
                    foreach (var slot in toolbar.slots)
                    {
                        if (slot.IsEmpty) // Slot vacío
                        {
                            slot.itemName = reward.rewardName; // Asignar el nombre del ítem
                            slot.count = reward.rewardAmount; // Asignar la cantidad

                            // Verificar si reward.rewardIcon no es nulo
                            if (reward.rewardIcon != null)
                            {
                                // Asignar el sprite directamente al componente Image
                                slot.icon = reward.rewardIcon.GetComponent<SpriteRenderer>().sprite;
                            }
                            else
                            {
                                // Usar un sprite por defecto o buscar uno alternativo
                                slot.icon = Resources.Load<Sprite>("DefaultItemIcon");
                            }

                            Debug.Log($"Recompensa añadida a un nuevo slot: {reward.rewardName} x{reward.rewardAmount}");
                            break; // Salimos del bucle al añadir el ítem
                        }
                    }
                }
            }

            // Completar la tarea en NPCTasks
            npcTasks?.CompleteTask(currentTask.taskName);

            // Actualizar el inventario visual
            uiManager.RefreshInventoryUI("Toolbar");

            // Actualizar la información del slot de la tarea
            taskNameText.color = Color.green;
            requirementText.color = Color.green;
            requirementText.text = $"{currentTask.requirement.quantity}/{currentTask.requirement.quantity}";

            Debug.Log($"Tarea completada: {currentTask.taskName}");
        }
    }
}
