using UnityEngine;

public class NPCDetection : MonoBehaviour
{
    public GameObject tasksCanvas; // Asigna tu Canvas de tareas desde el Inspector
    public NPCMovement npcMovement; // Referencia al script NPCMovement
    public string playerTag = "Player"; // Tag del jugador

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            Debug.Log("Hay un NPC cerca: NPC1");
            tasksCanvas.gameObject.SetActive(true); // Activa el Canvas
            npcMovement.canMove = false; // Detiene al NPC
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            Debug.Log("El jugador se alejó del NPC");
            tasksCanvas.gameObject.SetActive(false); // Desactiva el Canvas
            npcMovement.canMove = true; // Reactiva el movimiento del NPC
        }
    }
}
