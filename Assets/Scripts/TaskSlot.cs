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

    public void Setup(NPCTasks.Task task)
    {
        Debug.Log("Configurando TaskSlot para: " + task.taskName);

        // Actualizando el nombre de la tarea y el progreso
        taskNameText.text = task.taskName;
        progressText.text = $"{task.currentAmount}/{task.totalAmount}";
        requirementText.text = $"{task.requirement.quantity}"; // Mostrar el nombre del requisito

        // Asignando el ícono del requisito
        if (task.icon != null)
        {
            Debug.Log("Asignando ícono del requisito: " + task.icon.name);
            requirementIcon.GetComponent<Image>().sprite = task.icon.GetComponent<SpriteRenderer>().sprite;
        }

        // Asignando el texto y los iconos de recompensa
        string rewardsSummary = "";
        foreach (var reward in task.rewards)
        {
            rewardsSummary += $"{reward.rewardAmount}\n"; // Añadir el nombre y cantidad de recompensa al texto
            if (reward.rewardIcon != null)
            {
                // Asignar el icono de recompensa si está disponible
                rewardIcon.GetComponent<Image>().sprite = reward.rewardIcon.GetComponent<SpriteRenderer>().sprite;
            }
        }
        rewardText.text = rewardsSummary; // Mostrar la lista de recompensas
        Debug.Log("Recompensas asignadas: " + rewardsSummary);
    }
}
