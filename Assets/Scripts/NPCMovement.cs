using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float maxDistance = 2f;
    public float boundary = 3f;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool moving = false;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public bool canMove = true; // Controla si el NPC puede moverse

    void Start()
    {
        initialPosition = transform.position;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetNewTargetPosition();
    }

    void Update()
    {
        if (!canMove)
        {
            // Si el NPC no puede moverse, desactiva la animación
            animator.SetBool("isWalking", false);
            return;
        }

        if (moving)
        {
            animator.SetBool("isWalking", true);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                moving = false;
                animator.SetBool("isWalking", false);
                Invoke("SetNewTargetPosition", 2f);
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    void SetNewTargetPosition()
    {
        if (!canMove) return; // Si no puede moverse, no elige nueva posición

        int direction = Random.Range(0, 4);
        Vector3 potentialPosition = transform.position;

        switch (direction)
        {
            case 0:
                potentialPosition = transform.position + Vector3.up * maxDistance;
                break;
            case 1:
                potentialPosition = transform.position + Vector3.down * maxDistance;
                break;
            case 2:
                potentialPosition = transform.position + Vector3.right * maxDistance;
                spriteRenderer.flipX = false;
                break;
            case 3:
                potentialPosition = transform.position + Vector3.left * maxDistance;
                spriteRenderer.flipX = true;
                break;
        }

        if (IsWithinBounds(potentialPosition))
        {
            targetPosition = potentialPosition;
        }
        else
        {
            switch (direction)
            {
                case 0:
                    targetPosition = transform.position + Vector3.down * maxDistance;
                    break;
                case 1:
                    targetPosition = transform.position + Vector3.up * maxDistance;
                    break;
                case 2:
                    targetPosition = transform.position + Vector3.left * maxDistance;
                    spriteRenderer.flipX = true;
                    break;
                case 3:
                    targetPosition = transform.position + Vector3.right * maxDistance;
                    spriteRenderer.flipX = false;
                    break;
            }
        }

        moving = true;
    }

    bool IsWithinBounds(Vector3 position)
    {
        return Mathf.Abs(position.x - initialPosition.x) <= boundary && Mathf.Abs(position.y - initialPosition.y) <= boundary;
    }
}
