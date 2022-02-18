using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootScript : MonoBehaviour
{

    public float speed = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y += 1 * speed * Time.deltaTime;
        transform.position = pos;

        DestroyShoot(pos);
    }

    void DestroyShoot (Vector3 pos) {
        if( pos.y > Camera.main.orthographicSize ) {
            Destroy(gameObject);
        }
    }
}
