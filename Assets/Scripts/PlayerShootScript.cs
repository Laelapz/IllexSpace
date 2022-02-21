using UnityEngine;

public class PlayerShootScript : MonoBehaviour
{

    public float speed = 1f;
    public string type = "player";
    public int dir = 1;
    void Start()
    {
        // transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
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

    void OnTriggerEnter2D () {
        DestroyShoot(new Vector3(0, 1000, 0));
    }
    void DestroyShoot (Vector3 pos) {
        if( pos.y > Camera.main.orthographicSize  || pos.y < -Camera.main.orthographicSize) {
            Destroy(gameObject);
        }
    }
}
