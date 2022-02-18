using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Player;
    void Start()
    {
        Enemy1 = (GameObject)Resources.Load("Prefabs/Enemy1", typeof(GameObject));
        Enemy2 = (GameObject)Resources.Load("Prefabs/Enemy2", typeof(GameObject));
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Camera.main.orthographicSize + 1;
        var rot = new Quaternion(0, 0, 0, 0);
        if ( Input.GetKeyDown(KeyCode.Mouse1) ) {
            InstantiateEnemies(distance, rot);
        }
        
    }

    void InstantiateEnemies (float distance, Quaternion rot) {

        int num = Random.Range(0, 2 );
        GameObject enemy = null;

        if ( num == 0 ){
            enemy = Instantiate(Enemy1, new Vector3(0, distance, 0), rot);
        }else{
            enemy = Instantiate(Enemy2, new Vector3(0, distance, 0), rot);
        }
        
        int playerdamage = Player.GetComponents<PlayerController>()[0].damage;
        var EnemyVars = enemy.GetComponent<EnemyScript>();
        EnemyVars.PlayerDamage = playerdamage;
        EnemyVars.life = 2;
    }
}
