using UnityEngine;

public class EnemyShoot1 : MonoBehaviour
{
    public GameObject ShootEnemy;

    private float ShootTime = 0f;
    public float ReloadShoot = 1.5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate() {
        ShootOnPlayer(transform.position);
        
    }

    void ShootOnPlayer (Vector3 pos) {
        //Alterna os efeitos das cores para combinar com o inimigo e gerencia o tempo de espera entre os disparos
        //O tiro normal do jogo

        if ( ShootTime > ReloadShoot ){
            Quaternion rot = new Quaternion(0, 0, 5, 0);
            pos.z = -1;
            var shoot = Instantiate(ShootEnemy, pos, rot);
            var playerShootScript = shoot.GetComponent<PlayerShootScript>();
            playerShootScript.timeToDestroy = 20f;
            playerShootScript.shootColor.color = new Color(0, 1, 0, 1);
            ShootTime = 0f;
            playerShootScript.speed = 2;
        }
        else {
            ShootTime += Time.deltaTime;
        }
        
    }
}
