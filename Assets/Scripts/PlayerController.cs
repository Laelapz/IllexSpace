using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.InputSystem;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour
{
    public PlayerInput playerInput;
    public Actions actions;
    private Vector2 inputVector = Vector2.zero;

    [SerializeField] private Material _myMaterial;

    public AudioSource PlayerEffects;
    public AudioClip laserSound;
    public AudioClip damageSound;

    public Rigidbody2D Player;
    public WorldManager worldManager;
    public Animator animator;
    public Renderer rendererColor;
    public Animator boostAnimator;
    public GameObject Shoot;
    public GameObject Boost;
    public ParticleSystem ShieldParticles;
    public Transform ShieldTranform;

    public Sprite ShildSprite;

    [Header("Player Status")]
    public float currentPlayerSpeed = 5;
    private float playerSpeed = 5;
    public int currentPlayerDamage = 1;
    private int playerDamage = 1;
    public float currentPlayerSize = 1f;
    private float playerSize = 1f;
    public float currentShootCooldown = 1f;
    private float invulnerabilityTime = 1f;
    public float currentInvulnerabilityTime = 1f;

    [Header("Bullet Status")]
    public float currentBulletSize = 0.02f;
    private float bulletSize = 0.02f;
    private float shootCooldown = 1f;
    public float currentBulletSpeed = 1f;
    private float bulletSpeed = 1f;
    public float invulnerabilityCounter = 0f;

    [Header("Bools")]
    public bool canDamage = true;
    public bool canMove = true;
    public bool canShoot = true;
    public bool isDead = false;
    public bool ShootPower = false;
    
    private float PlayerBoundary = 0.5f;
    private float explosionTimer = 0f;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        actions = new Actions();
        actions.Enable();
        
    }

    void Update()
    {

        if ( canMove && !worldManager.IsPaused ){
            //Caso o player possa se movimentar e o jogo não esteja pausado
            //Recebe os Inputs de movimento tiro e também limita o mesmo as dimensões da tela

            inputVector = actions.Ship.Move.ReadValue<Vector2>();
            Move(inputVector);
            PlayerShoot(transform.position, transform.rotation.z);
            if (invulnerabilityCounter > 0)
            {
                canDamage = false;
                Flash(1);
            }
            else
            {
                canDamage = true;
                Flash(0);
            }
            
            invulnerabilityCounter -= Time.deltaTime;
        }

        Explosion();
        UpdateAnimation(inputVector);
    }

    private void Move(Vector2 value)
    {
        if (!canMove || worldManager.IsPaused) return;
        print("Movendo");

        Vector3 pos;

        pos = transform.position;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        pos.y += value.y * currentPlayerSpeed * Time.deltaTime;
        pos.x += value.x * currentPlayerSpeed * Time.deltaTime;
        pos = ScreenBoundary(pos);
        transform.position = pos;
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
    void UpdateAnimation (Vector2 value) {
        //Gerencia todos os booleanos do animator para reproduzir as animações

        if (value.x > 0)
        {
            Boost.transform.localScale = new Vector3(2, 3, 1);
            animator.SetBool("Right", true);
            animator.SetBool("Middle", false);
        }
        else if (value.x < 0)
        {
            Boost.transform.localScale = new Vector3(2, 3, 1);
            animator.SetBool("Left", true);
            animator.SetBool("Middle", false);
        }
        else
        {
            Boost.transform.localScale = new Vector3(3, 3, 1);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
            animator.SetBool("Middle", true);
        }
    }

    public void AddInvulnerabilityTime(float time)
    {
        if (invulnerabilityCounter < 0) invulnerabilityCounter = 0;

        invulnerabilityCounter += time;
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
    private void Dead () {
        transform.position = new Vector3(0, -4, -2);
        worldManager.ResetPlayerStats();

        AddInvulnerabilityTime(currentInvulnerabilityTime);
    }

    public void Flash(float boolValue)
    {
        _myMaterial.SetFloat("_WhiteFlash", boolValue);
    }

    void PlayerShoot (Vector3 pos, float z_rot) {
        //Caso o player clique com o mouse instancia um tiro
        //E se ele tiver com o coletável de tiro instancia vários ao mesmo tempo 


        if ( canShoot ) {
            PlayerEffects.clip = laserSound;
            PlayerEffects.volume = 0.7f;
            PlayerEffects.Play();
            Quaternion rot = new Quaternion(0, 0, z_rot, 0);
            if (ShootPower) {
                for(int begin = -15; begin < 15; begin += 5) {
                    var shoot = Instantiate(Shoot, pos, rot);
                    shoot.GetComponent<PlayerShootScript>().speed = currentBulletSpeed;
                    shoot.transform.Rotate(new Vector3(0, 0, begin));
                    shoot.transform.localScale = new Vector3(currentBulletSize, currentBulletSize, currentBulletSize);
                }
            }
            else {
                var shoot = Instantiate(Shoot, pos, rot);
                shoot.GetComponent<PlayerShootScript>().speed = currentBulletSpeed;
                shoot.transform.localScale = new Vector3(currentBulletSize, currentBulletSize, currentBulletSize);
            }

            StartCoroutine(ShootCooldown());

        }

    }

    public void ResetStatus()
    {
        currentPlayerSpeed = playerSpeed;
        currentPlayerDamage = playerDamage;
        currentBulletSize = bulletSize;
        currentShootCooldown = shootCooldown;
        currentInvulnerabilityTime = invulnerabilityTime;
        currentBulletSpeed = bulletSpeed;
        currentPlayerSize = playerSize;
    }

    private IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(currentShootCooldown);
        canShoot = true;
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
