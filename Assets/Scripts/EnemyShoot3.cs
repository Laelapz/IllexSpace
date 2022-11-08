using UnityEngine;
using UnityEngine.Events;

public class EnemyShoot3 : MonoBehaviour
{
    
    public GameObject ShootEnemy;
    private float ShootTime = 0f;
    private float ReloadShoot = 2.5f;

    UnityAction<Vector3, GameObject, Transform> m_call;

    void Update()
    {
        ShootOnPlayer(transform.position);
    }

    public void ShootOnPlayer(Vector3 pos) {
        if ( ShootTime > ReloadShoot)
        {
            ShootTime = 0f;
            Quaternion rot = new Quaternion(0, 0, 5, 0);
            pos.z = -1;
            var shoot = Instantiate(ShootEnemy, pos, rot);
            var playerShootScript = shoot.GetComponent<PlayerShootScript>();
            playerShootScript.shootColor.color = new Color(1, 0.5f, 0.5f, 1);
            playerShootScript.speed = 2;

            m_call = shoot3callback;
            playerShootScript.callback += m_call;

        }
        else {
            ShootTime += Time.deltaTime;
        }
    }

    private void shoot3callback(Vector3 pos, GameObject ShootEnemy, Transform parent)
    {
        for (int rot_z = -30; rot_z <= 30; rot_z += 15)
        {
            Quaternion rot = new Quaternion(0, 0, 5, 0);

            pos.z = -1;
            var shoot = Instantiate(ShootEnemy, pos, rot, parent);

            var playerShootScript = shoot.GetComponent<PlayerShootScript>();
            playerShootScript.shootColor.color = new Color(1, 0.5f, 0.5f, 1);
            playerShootScript.timeToDestroy = 2f;
            shoot.transform.Rotate(new Vector3(0, 0, rot_z));
            playerShootScript.speed = 2;
        }
    }
}
