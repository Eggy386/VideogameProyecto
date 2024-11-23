using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 2f;          // Velocidad de movimiento
    public float maxDistance = 2f;        // Distancia de cada movimiento (2 unidades por vez)
    public float boundary = 3f;           // Límite de movimiento en cada dirección desde la posición inicial
    private Vector3 initialPosition;      // Posición inicial para limitar el área de movimiento
    private Vector3 targetPosition;       // Posición objetivo hacia la que se moverá el NPC
    private bool moving = false;          // Verificar si está en movimiento

    private Animator animator;
    private SpriteRenderer spriteRenderer; // Nuevo: Referencia al SpriteRenderer

    void Start()
    {
        initialPosition = transform.position;      // Guardar la posición inicial
        animator = GetComponent<Animator>();       // Obtener el componente Animator
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtener el componente SpriteRenderer
        SetNewTargetPosition();                    // Elegir una nueva posición objetivo
    }

    void Update()
    {
        if (moving)
        {
            // Activar la animación de caminar
            animator.SetBool("isWalking", true);

            // Mover hacia la posición objetivo
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Verificar si llegó a la posición objetivo
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                moving = false;
                animator.SetBool("isWalking", false); // Desactivar la animación de caminar
                Invoke("SetNewTargetPosition", 2f); // Esperar un segundo antes de moverse de nuevo
            }
        }
        else
        {
            // Desactivar la animación de caminar si no está en movimiento
            animator.SetBool("isWalking", false);
        }
    }

    void SetNewTargetPosition()
    {
        // Elegir una dirección aleatoria (arriba, abajo, izquierda, derecha)
        int direction = Random.Range(0, 4);
        Vector3 potentialPosition = transform.position;

        switch (direction)
        {
            case 0: // Arriba
                potentialPosition = transform.position + Vector3.up * maxDistance;
                break;
            case 1: // Abajo
                potentialPosition = transform.position + Vector3.down * maxDistance;
                break;
            case 2: // Derecha
                potentialPosition = transform.position + Vector3.right * maxDistance;
                spriteRenderer.flipX = false; // Mirando a la derecha
                break;
            case 3: // Izquierda
                potentialPosition = transform.position + Vector3.left * maxDistance;
                spriteRenderer.flipX = true; // Mirando a la izquierda
                break;
        }

        // Comprobar si la posición potencial está dentro de los límites
        if (IsWithinBounds(potentialPosition))
        {
            targetPosition = potentialPosition; // Aceptar el nuevo objetivo si está dentro de los límites
        }
        else
        {
            // Invertir la dirección si sale de los límites
            switch (direction)
            {
                case 0: // Si se intentó mover arriba, cambiar a abajo
                    targetPosition = transform.position + Vector3.down * maxDistance;
                    break;
                case 1: // Si se intentó mover abajo, cambiar a arriba
                    targetPosition = transform.position + Vector3.up * maxDistance;
                    break;
                case 2: // Si se intentó mover a la derecha, cambiar a izquierda
                    targetPosition = transform.position + Vector3.left * maxDistance;
                    spriteRenderer.flipX = true; // Mirando a la izquierda
                    break;
                case 3: // Si se intentó mover a la izquierda, cambiar a derecha
                    targetPosition = transform.position + Vector3.right * maxDistance;
                    spriteRenderer.flipX = false; // Mirando a la derecha
                    break;
            }
        }

        moving = true; // Iniciar el movimiento
    }

    // Función para verificar si una posición está dentro de los límites
    bool IsWithinBounds(Vector3 position)
    {
        return Mathf.Abs(position.x - initialPosition.x) <= boundary && Mathf.Abs(position.y - initialPosition.y) <= boundary;
    }
}
