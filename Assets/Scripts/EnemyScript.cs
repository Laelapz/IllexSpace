using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public AudioSource EnemyEffects;

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
        //O playerDamage era para se por um acaso eu decidisse implementar aumento de dano por parte do player
        DamageEnemy(playerDamage);
    }

    void DamageEnemy ( int damage ) {
        //Ao tomar dano ativa os efeitos sonoros e visuais de dano

        if ( canDamage ) {
            EnemyEffects.Play();
            life -= damage;

            if ( life <= 0 )
            {
                worldManager.IncreasePoints(xpBase);
                Destroy(gameObject);
            }
        }
    }
    void DestroyEnemy (Vector3 pos) {
        //Caso o inimigos esteja fora de tela destroi o mesmo

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
