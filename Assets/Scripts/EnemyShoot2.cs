using UnityEngine;

public class EnemyShoot2 : MonoBehaviour
{
    public GameObject ShootEnemy;
    public float ReloadShoot = 3f;
    private float ShootTime = 0f;

    void FixedUpdate() {
        ShootOnPlayer(transform.position);
        
    }

    void ShootOnPlayer (Vector3 pos) {
        if ( ShootTime > ReloadShoot ){

            for (int rot_z = -30; rot_z <= 30; rot_z+=15){
                Quaternion rot = new Quaternion(0, 0, 5, 0);
                pos.z = -1;
                var shoot = Instantiate(ShootEnemy, pos, rot);
                shoot.transform.Rotate(new Vector3(0, 0, rot_z));
                shoot.GetComponent<PlayerShootScript>().speed = 2;
            }
            
            ShootTime = 0f;
        }
        else {
            ShootTime += Time.deltaTime;
        }
        
    }
}
