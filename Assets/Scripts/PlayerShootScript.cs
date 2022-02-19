using UnityEngine;

public class PlayerShootScript : MonoBehaviour
{

    public float speed = 1f;

    public string type = "player";
    public int dir = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3 ( 0, speed * Time.deltaTime );
        pos += transform.rotation * velocity;
        transform.position = pos;

        DestroyShoot(pos);
    }

    void OnCollisionEnter2D ( Collision2D collision ) {
        if ( collision.collider.tag == "Enemy" && type == "player" ) {
            DestroyShoot(new Vector3(0, 1000, 0));
        }
        else if ( (collision.collider.tag == "Player" || collision.collider.tag == "Enemy") && type == "enemy" ) {
            DestroyShoot(new Vector3(0, 1000, 0));
        }
    }
    void DestroyShoot (Vector3 pos) {
        if( pos.y > Camera.main.orthographicSize  || pos.y < -Camera.main.orthographicSize) {
            Destroy(gameObject);
        }
    }
}
