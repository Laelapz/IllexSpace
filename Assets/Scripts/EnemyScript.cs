using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int playerDamage;
    public WorldManager worldManager;
    public Animator animator;
    public float speed = 1f;
    public int xpBase = 1;
    public int life = 1;
    public bool canDamage = true;
    private bool IsDead = false;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y -= 1 * speed * Time.deltaTime;
        transform.position = pos;
        DestroyEnemy(pos);

    }

    void OnTriggerEnter2D() {
        DamageEnemy(playerDamage);
    }

    void DamageEnemy ( int damage ) {
        if ( canDamage ) {
            life -= damage;

            if ( life <= 0 ) {
                DestroyEnemy(new Vector3(0, -1000, 0));
            }
        }
    }
    void DestroyEnemy (Vector3 pos) {
        if ( !IsDead && pos.y < -Camera.main.orthographicSize - 1 ) {
            IsDead = true;
            animator.SetBool("IsDead", true);
            Destroy(gameObject, 0.30f);
            if ( life <= 0 ) { 
                worldManager.IncreasePoints(xpBase);
            }
        }
    }
}
