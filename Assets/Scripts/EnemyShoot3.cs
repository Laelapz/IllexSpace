using UnityEngine;

public class EnemyShoot3 : MonoBehaviour
{
    
    public GameObject ShootEnemy;
    public bool shooting = false;
    public float ShootTime = 0f;

    void Update()
    {
        //Shooting só será verdadeiro quando o animator setar como true
        //Que é o momento onde o inimigo rosa está aberto e aberto a dano
        if ( shooting ) {
            ShootOnPlayer(transform.position);
        }
    }

    public void ShootOnPlayer(Vector3 pos) {
        //Alterna os efeitos das cores para combinar com o inimigo e gerencia o tempo de espera entre os disparos
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
