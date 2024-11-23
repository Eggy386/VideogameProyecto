using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 2f;          // Velocidad de movimiento
    public float maxDistance = 2f;        // Distancia de cada movimiento (2 unidades por vez)
    public float boundary = 3f;           // L�mite de movimiento en cada direcci�n desde la posici�n inicial
    private Vector3 initialPosition;      // Posici�n inicial para limitar el �rea de movimiento
    private Vector3 targetPosition;       // Posici�n objetivo hacia la que se mover� el NPC
    private bool moving = false;          // Verificar si est� en movimiento

    private Animator animator;
    private SpriteRenderer spriteRenderer; // Nuevo: Referencia al SpriteRenderer

    void Start()
    {
        initialPosition = transform.position;      // Guardar la posici�n inicial
        animator = GetComponent<Animator>();       // Obtener el componente Animator
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtener el componente SpriteRenderer
        SetNewTargetPosition();                    // Elegir una nueva posici�n objetivo
    }

    void Update()
    {
        if (moving)
        {
            // Activar la animaci�n de caminar
            animator.SetBool("isWalking", true);

            // Mover hacia la posici�n objetivo
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Verificar si lleg� a la posici�n objetivo
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                moving = false;
                animator.SetBool("isWalking", false); // Desactivar la animaci�n de caminar
                Invoke("SetNewTargetPosition", 2f); // Esperar un segundo antes de moverse de nuevo
            }
        }
        else
        {
            // Desactivar la animaci�n de caminar si no est� en movimiento
            animator.SetBool("isWalking", false);
        }
    }

    void SetNewTargetPosition()
    {
        // Elegir una direcci�n aleatoria (arriba, abajo, izquierda, derecha)
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

        // Comprobar si la posici�n potencial est� dentro de los l�mites
        if (IsWithinBounds(potentialPosition))
        {
            targetPosition = potentialPosition; // Aceptar el nuevo objetivo si est� dentro de los l�mites
        }
        else
        {
            // Invertir la direcci�n si sale de los l�mites
            switch (direction)
            {
                case 0: // Si se intent� mover arriba, cambiar a abajo
                    targetPosition = transform.position + Vector3.down * maxDistance;
                    break;
                case 1: // Si se intent� mover abajo, cambiar a arriba
                    targetPosition = transform.position + Vector3.up * maxDistance;
                    break;
                case 2: // Si se intent� mover a la derecha, cambiar a izquierda
                    targetPosition = transform.position + Vector3.left * maxDistance;
                    spriteRenderer.flipX = true; // Mirando a la izquierda
                    break;
                case 3: // Si se intent� mover a la izquierda, cambiar a derecha
                    targetPosition = transform.position + Vector3.right * maxDistance;
                    spriteRenderer.flipX = false; // Mirando a la derecha
                    break;
            }
        }

        moving = true; // Iniciar el movimiento
    }

    // Funci�n para verificar si una posici�n est� dentro de los l�mites
    bool IsWithinBounds(Vector3 position)
    {
        return Mathf.Abs(position.x - initialPosition.x) <= boundary && Mathf.Abs(position.y - initialPosition.y) <= boundary;
    }
}
