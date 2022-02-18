using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public GameObject enemy;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Camera.main.orthographicSize + 1;
        var rot = new Quaternion(180, 0, 0, 0);
        if ( Input.GetKeyDown(KeyCode.Mouse1) ) {
            InstantiateEnemies(distance, rot);
        }
        
    }

    void InstantiateEnemies (float distance, Quaternion rot) {
            Instantiate(enemy, new Vector3(0, distance, 0), rot);
    }
}
