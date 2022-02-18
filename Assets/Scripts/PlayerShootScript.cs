using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootScript : MonoBehaviour
{

    public float speed = 1f;
    public int dir = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y += 1 * speed * Time.deltaTime * dir;
        transform.position = pos;

        DestroyShoot(pos);
    }

    void OnCollisionEnter2D ( Collision2D collision ) {
        if ( collision.collider.tag == "Enemy" ) {
            DestroyShoot(new Vector3(0, 1000, 0));
        }
    }
    void DestroyShoot (Vector3 pos) {
        if( pos.y > Camera.main.orthographicSize ) {
            Destroy(gameObject);
        }
    }
}
