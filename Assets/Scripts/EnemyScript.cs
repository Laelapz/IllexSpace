using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int playerDamage;

    public WorldManager worldManager;
    public float speed = 1f;
    public int xpBase = 1;
    public int life = 1;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y -= 1 * speed * Time.deltaTime;
        transform.position = pos;
        DestroyEnemy(pos);

    }

    void OnCollisionEnter2D( Collision2D collision ) {
        if( collision.collider.tag == "Player" || collision.collider.tag == "MyBullet" || collision.collider.tag == "EnemyBullet" ) {
            DamageEnemy(playerDamage);
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
            worldManager.IncreasePoints(xpBase);
            int num = Random.Range(0, 4);
            worldManager.ActivatePower(num);
        }
    }
}
