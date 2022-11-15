using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : MonoBehaviour
{
    public float amplitude = 1f;
    public float speed = 1f;

    private Camera _mainCamera;
    [SerializeField] private HandleEnemyRotation _enemyRotation;
    void Start()
    {
        var startDirection = Random.Range(0, 2);
        speed *= startDirection == 0 ? 1 : -1;
        _enemyRotation.speed = 3f;
        _mainCamera = Camera.main;

        if(startDirection == 0)
        {
            _enemyRotation.RotateRight();
            return;
        }

        _enemyRotation.RotateLeft();
    }

    void Update()
    {
        Vector3 pos = transform.position;

        if (pos.x < -_mainCamera.orthographicSize * 2)
        {
            _enemyRotation.RotateRight();
            speed *= -1;
        }

        if (pos.x > _mainCamera.orthographicSize * 2)
        {
            _enemyRotation.RotateLeft();
            speed *= -1;
        }

        pos.x += (Time.deltaTime * amplitude) * speed;
        transform.position = pos;

    }
}
