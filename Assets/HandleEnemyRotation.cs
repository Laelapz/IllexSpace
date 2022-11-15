using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HandleEnemyRotation : MonoBehaviour
{
    private int direction = 1;
    private Vector3 rotation = Vector3.zero;

    public float speed = 1f;

    private void Start()
    {
        rotation.z = direction;
    }

    private void FixedUpdate()
    {
        transform.Rotate(rotation, direction * speed);
    }

    public void RotateRight()
    {
        direction = 1;
    }

    public void RotateLeft()
    {
        direction = -1;
    }
}
