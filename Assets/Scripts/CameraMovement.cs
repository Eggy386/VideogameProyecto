using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public GameObject follow;
    public float cameraSpeed;
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 direction = follow.transform.position - transform.position;
        direction = new Vector3(direction.x, direction.y, 0);
        transform.position += direction.normalized * cameraSpeed * Time.deltaTime;
        //transform.position = follow.transform.position;

    }
}
