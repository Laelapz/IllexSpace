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
        if ( ShootTime > ReloadShoot ){
            Quaternion rot = new Quaternion(0, 0, 5, 0);
            pos.z = -1;
            var shoot = Instantiate(ShootEnemy, pos, rot);
            shoot.GetComponent<PlayerShootScript>().shootColor.color = new Color(0, 1, 0, 1);
            shoot.GetComponent<PlayerShootScript>().shootParticleColor.startColor = new Color(0, 1, 0, 1);
            shoot.GetComponent<PlayerShootScript>().speed = 2;
            ShootTime = 0f;
        }
        else {
            ShootTime += Time.deltaTime;
        }
        
    }
}
