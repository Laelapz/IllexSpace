using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    
    public WorldManager worldManager;
    public int type;
    private float speed = 0.7f;


    void Update() {
        //Força a caixa a cair lentamente

        Vector3 pos = transform.position;
        pos.y -= speed * Time.deltaTime;
        pos.z = -1;
        transform.position = pos;
        
    }

    void OnTriggerEnter2D (Collider2D collider) {
        //Ao colidir com algo chama a função do world manager que ativa o coletável
        //As colisões foram feitas com layers então garante que a caixa só pode colidir com o player e/ou suas balas
        if ( collider.tag != "MyBullet" ) {
            worldManager.ActivatePower(type);
            Destroy(gameObject);
        }
    }

    public void SetSprite(Sprite sprite) {
        FindObjectOfType<SpriteRenderer>().sprite = sprite;
    }
}
