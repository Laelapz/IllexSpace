using UnityEngine;

public class PlayerShootScript : MonoBehaviour
{

    public float speed = 1f;
    public string type = "player";
    public SpriteRenderer shootColor;
    public ParticleSystem shootParticleColor;
    public int dir = 1;

    void Update() {
        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3 ( 0, speed * Time.deltaTime );
        pos += transform.rotation * velocity;
        transform.position = pos;

        DestroyShoot(pos);
    }

    void OnTriggerEnter2D () {
        //Ao colidir com alguém simula como se a bala tivesse saido da tela
        DestroyShoot(new Vector3(0, 1000, 0));
    }
    void DestroyShoot (Vector3 pos) {
        //A função é chamada a todo frame caso a bala esteja no final da tela ela é destruida

        if( pos.y > Camera.main.orthographicSize  || pos.y < -Camera.main.orthographicSize) {
            Destroy(gameObject);
        }
    }
}
