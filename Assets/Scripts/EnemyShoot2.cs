using UnityEngine;

public class EnemyShoot2 : MonoBehaviour
{
    public GameObject ShootEnemy;
    private float ShootTime = 0f;
    public float ReloadShoot = 3f;

    void FixedUpdate() {
        ShootOnPlayer(transform.position);
        
    }

    void ShootOnPlayer (Vector3 pos) {
        //Alterna os efeitos das cores para combinar com o inimigo e gerencia o tempo de espera entre os disparos
        //Este inimigo atira em leque

        if ( ShootTime > ReloadShoot ){

            for (int rot_z = -30; rot_z <= 30; rot_z+=15){
                Quaternion rot = new Quaternion(0, 0, 5, 0);
                pos.z = -1;
                var shoot = Instantiate(ShootEnemy, pos, rot);
                var playerShootScript = shoot.GetComponent<PlayerShootScript>();
                playerShootScript.timeToDestroy = 20f;
                playerShootScript.shootColor.color = new Color(1, 1, 0, 1);
                shoot.transform.Rotate(new Vector3(0, 0, rot_z));
                playerShootScript.speed = 2;
            }
            
            ShootTime = 0f;
        }
        else {
            ShootTime += Time.deltaTime;
        }
        
    }
}
