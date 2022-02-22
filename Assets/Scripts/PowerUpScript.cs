using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    
    public WorldManager worldManager;
    public int type;
    private float speed = 0.7f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y -= speed * Time.deltaTime;
        pos.z = -1;
        transform.position = pos;
        
    }

    void OnTriggerEnter2D (Collider2D collider) {
        if (    collider.tag != "MyBullet" ) {
            worldManager.ActivatePower(type);
            Destroy(gameObject);
        }
    }

    public void SetSprite(Sprite sprite) {
        FindObjectOfType<SpriteRenderer>().sprite = sprite;
    }
}
