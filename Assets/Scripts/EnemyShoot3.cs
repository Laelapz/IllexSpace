using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot3 : MonoBehaviour
{
    
    public GameObject ShootEnemy;
    public bool shooting = false;
    public float ShootTime = 0f;

    void Update()
    {
        if ( shooting ) {
            ShootOnPlayer(transform.position);
        }
    }

    public void ShootOnPlayer(Vector3 pos) {
        if ( ShootTime > 0.5 ) {
            Quaternion rot = new Quaternion(0, 0, 5, 0);
            pos.z = -1;
            var shoot = Instantiate(ShootEnemy, pos, rot);
            shoot.GetComponent<PlayerShootScript>().shootColor.color = new Color(1, 0.5f, 0.5f, 1);
            shoot.GetComponent<PlayerShootScript>().shootParticleColor.startColor = new Color(1, 0.5f, 0.5f, 1);
            shoot.GetComponent<PlayerShootScript>().speed = 2;
            ShootTime = 0f;
        }
        else {
            ShootTime += Time.deltaTime;
        }
    }
}
