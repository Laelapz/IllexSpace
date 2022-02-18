using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int PlayerDamage;
    public float speed = 1f;
    public int life = 1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y -= 1 * speed * Time.deltaTime;
        transform.position = pos;
        DestroyEnemy(pos);

    }

    void OnCollisionEnter2D( Collision2D collision ) {
        if( collision.collider.tag == "Player" || collision.collider.tag == "MyBullet" ) {
            Debug.Log(PlayerDamage);
            DamageEnemy(1);
        }
    }

    void DamageEnemy ( int damage ) {
        life -= damage;

        if ( life <= 0 ) {
            DestroyEnemy(new Vector3(0, -1000, 0));
        }
    }
    void DestroyEnemy (Vector3 pos) {
        if ( pos.y < -Camera.main.orthographicSize - 1 ) {
            Destroy(gameObject);
        }
    }
}
