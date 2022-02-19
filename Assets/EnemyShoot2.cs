using UnityEngine;

public class EnemyShoot2 : MonoBehaviour
{
    public GameObject ShootEnemy;
    public float ReloadShoot = 1f;

    private float ShootTime = 0f;

    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector3(pos.x, pos.y - 1, pos.z), -Vector2.up);

        foreach ( RaycastHit2D hit in hits ) {
            if (hit.collider.tag == "Player" ){
                ShootOnPlayer(transform.position);
            }
        }
        
    }

    void ShootOnPlayer (Vector3 pos) {
        if ( ShootTime > ReloadShoot ){
            Quaternion rot = new Quaternion(0, 0, 0, 0);
            var shoot = Instantiate(ShootEnemy, pos, rot);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shoot.GetComponent<Collider2D>());
            shoot.tag = "EnemyBullet";
            shoot.GetComponent<PlayerShootScript>().speed = 2;
            shoot.GetComponent<PlayerShootScript>().type = "enemy";
            ShootTime = 0f;
        }
        else {
            ShootTime += Time.deltaTime;
        }
        
    }
}
