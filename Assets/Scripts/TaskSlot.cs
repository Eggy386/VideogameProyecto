using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI taskNameText;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private GameObject requirementIcon; // �cono del requisito
    [SerializeField] private GameObject rewardIcon; // �cono de la recompensa
    [SerializeField] private TextMeshProUGUI requirementText; // Texto del requisito

    public void Setup(NPCTasks.Task task)
    {
        // Actualizando el nombre de la tarea y el progreso
        taskNameText.text = task.taskName;

        requirementText.text = $"{task.currentAmount}/{task.requirement.quantity}"; // Mostrar el nombre del requisito

        if(task.isCompleted == true)
        {
            requirementText.color = Color.green;
        }

        // Asignando el �cono del requisito
        if (task.icon != null)
        {
            requirementIcon.GetComponent<Image>().sprite = task.icon.GetComponent<SpriteRenderer>().sprite;
        }

        // Asignando el texto y los iconos de recompensa
        string rewardsSummary = "";
        foreach (var reward in task.rewards)
        {
            rewardsSummary += $"{reward.rewardAmount}\n"; // A�adir el nombre y cantidad de recompensa al texto
            if (reward.rewardIcon != null)
            {
                // Asignar el icono de recompensa si est� disponible
                rewardIcon.GetComponent<Image>().sprite = reward.rewardIcon.GetComponent<SpriteRenderer>().sprite;
            }
        }
        rewardText.text = $"x{rewardsSummary}"; // Mostrar la lista de recompensas
    }
}
