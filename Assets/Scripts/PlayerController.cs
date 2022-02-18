using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D Player;
    public GameObject Shoot;
    public float speed;

    private float PlayerBoundary = 0.5f;
    void Update()
    {
        Vector3 pos = transform.position;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        pos.y += Input.GetAxis("Vertical") * speed * Time.deltaTime;
        pos.x += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        pos = ScreenBoundary(pos);
        transform.position = pos;

        PlayerShoot(pos, transform.rotation.z);
        
    }

    void OnCollisionEnter2D (Collision2D collision) {
        if ( collision.collider.tag != "MyBullet" ) {
            Dead();
        }
    }
    void Dead () {
        transform.position = new Vector3(0, -4, 0);
    }

    void PlayerShoot (Vector3 pos, float z_rot) {

        if ( Input.GetKeyDown(KeyCode.Mouse0 ) ) {

            Quaternion rot = new Quaternion(0, 0, z_rot, 0);
            var shoot = Instantiate(Shoot, pos, rot);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shoot.GetComponent<Collider2D>());
            
        }

    }
    Vector3 ScreenBoundary (Vector3 pos) {

        //Se a posição do player for maior ou menor que os valores limites da camera força a posição a se encaixar
        if ( pos.y + PlayerBoundary > Camera.main.orthographicSize){
            pos.y = Camera.main.orthographicSize - PlayerBoundary;
        }
        else if ( pos.y - PlayerBoundary < -Camera.main.orthographicSize ) {
            pos.y = -Camera.main.orthographicSize + PlayerBoundary;
        }


        //Encontra a proporção da tela e usa a mesma lógica dos limites de camera
        float ScreenRatio = (float)Screen.width / (float)Screen.height;
        float ScreenOrtho = Camera.main.orthographicSize * ScreenRatio;  
        if ( pos.x + PlayerBoundary > ScreenOrtho ) {
            pos.x = ScreenOrtho - PlayerBoundary;
        }
        else if ( pos.x - PlayerBoundary < -ScreenOrtho ) {
            pos.x = -ScreenOrtho + PlayerBoundary;
        }

        return pos;
    }
}
