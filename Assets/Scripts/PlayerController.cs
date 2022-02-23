using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public AudioSource PlayerEffects;
    public AudioClip laserSound;
    public AudioClip damageSound;

    public Rigidbody2D Player;
    public WorldManager worldManager;
    public Animator animator;
    public Animator boostAnimator;
    public GameObject Shoot;
    public GameObject Boost;
    public ParticleSystem ShieldParticles;
    public Transform ShieldTranform;

    public Sprite ShildSprite;
    public float speed;
    public int damage = 1;
    public float BulletSize = 0.01f;

    public float BulletSpeed = 0f;
    public bool canDamage = true;
    public bool canMove = true;
    public bool isDead = false;
    public bool ShootPower = false;
    private float PlayerBoundary = 0.5f;
    private float explosionTimer = 0f;

    void Update()
    {
        Vector3 pos;
        
        if ( canMove && !worldManager.IsPaused ){
            //Caso o player possa se movimentar e o jogo não esteja pausado
            //Recebe os Inputs de movimento tiro e também limita o mesmo as dimensões da tela

            pos = transform.position;
            transform.rotation = new Quaternion(0, 0, 0, 0);
            pos.y += Input.GetAxis("Vertical") * speed * Time.deltaTime;
            pos.x += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            PlayerShoot(pos, transform.rotation.z);
            pos = ScreenBoundary(pos);
            transform.position = pos;
        }

        Explosion();
        UpdateAnimation();

        
    }

    public void StartShield () {
        //Deixa o escudo visível ao ser ativado
        ShieldParticles.Play();
        ShieldTranform.localScale = new Vector3(1, 1, 1);
    }

    public void StopShield () {
        //Esconde o escudo ao acabar o tempo de efeito
        ShieldTranform.localScale = new Vector3(0, 0, 0);
    }
    void UpdateAnimation () {
        //Gerencia todos os booleanos do animator para reproduzir as animações

        if ( Input.GetAxis("Horizontal") > 0 ){
            Boost.transform.localScale = new Vector3(2, 3, 1);
            animator.SetBool("Right", true);
            animator.SetBool("Middle", false);
        }
        else if( Input.GetAxis("Horizontal") < 0 ){
            Boost.transform.localScale = new Vector3(2, 3, 1);
            animator.SetBool("Left", true);
            animator.SetBool("Middle", false);
        }
        else{
            Boost.transform.localScale = new Vector3(3, 3, 1);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
            animator.SetBool("Middle", true);
        }
    }
    void OnTriggerEnter2D (Collider2D Obj) {
        //Caso o player colida com algo que não é um power up e possa ser danificado

        if ( Obj.gameObject.tag != "PowerUp" && canDamage ){
            //Seta o estado de morte
            isDead = true;
            canMove = false;
            animator.SetBool("IsDead", true);
            boostAnimator.SetBool("IsDead", true);
            
            PlayerEffects.clip = damageSound;
            PlayerEffects.volume = 1f;
            PlayerEffects.Play();
        }
    }

    void Explosion () {
        //Caso o player esteja setado como morto
        //Inicia os sons e animações de morte
        if( isDead ) {
            if(explosionTimer > 0.3f ) {
                canMove = true;
                animator.SetBool("IsDead", false);
                boostAnimator.SetBool("IsDead", false);
                isDead = false;
                explosionTimer = 0f;
                Dead();
            }
            else{
                explosionTimer += Time.deltaTime;
            }
        }
    }
    void Dead () {
        transform.position = new Vector3(0, -4, -2);
        worldManager.ResetPlayerStats();
    }

    void PlayerShoot (Vector3 pos, float z_rot) {
        //Caso o player clique com o mouse instancia um tiro
        //E se ele tiver com o coletável de tiro instancia vários ao mesmo tempo 


        if ( Input.GetKeyDown(KeyCode.Mouse0 ) ) {
            PlayerEffects.clip = laserSound;
            PlayerEffects.volume = 0.7f;
            PlayerEffects.Play();
            Quaternion rot = new Quaternion(0, 0, z_rot, 0);
            if (ShootPower) {
                for(int begin = -15; begin < 15; begin += 5) {
                    var shoot = Instantiate(Shoot, pos, rot);
                    shoot.GetComponent<PlayerShootScript>().speed = 1+BulletSpeed;
                    shoot.transform.Rotate(new Vector3(0, 0, begin));
                    shoot.transform.localScale = new Vector3(0.01f +BulletSize, 0.01f+BulletSize, 0.01f+BulletSize);
                }
            }
            else {
                var shoot = Instantiate(Shoot, pos, rot);
                shoot.GetComponent<PlayerShootScript>().speed = 1+BulletSpeed;
                shoot.transform.localScale = new Vector3(0.01f +BulletSize, 0.01f+BulletSize, 0.01f+BulletSize);

            }
        
        }

    }
    Vector3 ScreenBoundary (Vector3 pos) {

        //Se a posição do player for maior ou menor que os valores limites da camera força a posição a se encaixar
        if ( pos.y + PlayerBoundary > Camera.main.orthographicSize){
            pos.y = Camera.main.orthographicSize - PlayerBoundary;
        }
        else if ( pos.y - PlayerBoundary < -Camera.main.orthographicSize ) {
            pos.y = -Camera.main.orthographicSize + PlayerBoundary;
        }


        //Encontra a proporção da tela e usa a mesma lógica dos limites de camera
        float ScreenRatio = (float)Screen.width / (float)Screen.height;
        float ScreenOrtho = Camera.main.orthographicSize * ScreenRatio;  
        if ( pos.x + PlayerBoundary > ScreenOrtho ) {
            pos.x = ScreenOrtho - PlayerBoundary;
        }
        else if ( pos.x - PlayerBoundary < -ScreenOrtho ) {
            pos.x = -ScreenOrtho + PlayerBoundary;
        }

        return pos;
    }
}
