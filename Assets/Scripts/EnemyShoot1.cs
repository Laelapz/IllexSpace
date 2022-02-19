using UnityEngine;

public class EnemyShoot1 : MonoBehaviour
{
    public GameObject ShootEnemy;

    private float ShootTime = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
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
        if ( ShootTime > 1 ){
            Quaternion rot = new Quaternion(0, 0, 5, 0);
            pos.z = -1;
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
