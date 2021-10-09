using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMovement : MonoBehaviour
{
    public float speed;

    public Vector3 direction;

    void Update()
    {
        transform.position += direction * Time.deltaTime * speed;
    }
}