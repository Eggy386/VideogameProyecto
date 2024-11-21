using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento del personaje
    public Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector3 direction;  
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtener el componente SpriteRenderer
    }

    void Update()
    {
        // Obtener el input de movimiento en los ejes horizontal y vertical
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Crear un vector de movimiento basado en los inputs
        Vector3 move = new Vector3(moveX, moveY, 0f);

        // Aplicar el movimiento al personaje
        transform.position += move * moveSpeed * Time.deltaTime;

        // Activar o desactivar la animación de caminar
        if (move != Vector3.zero)
        {
            animator.SetBool("isWalking", true);

            // Voltear el sprite dependiendo de la dirección en el eje X
            if (moveX > 0)
            {
                spriteRenderer.flipX = false; // Mirando a la derecha
            }
            else if (moveX < 0)
            {
                spriteRenderer.flipX = true; // Mirando a la izquierda
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}
