using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D Player;
    public Animator animator;
    public GameObject Shoot;
    public GameObject Boost;
    public ParticleSystem ShieldParticles;
    public float speed;
    public int damage = 1;
    public float BulletSize = 0.01f;
    public bool canDamage = true;
    public bool ShootPower = false;
    private float PlayerBoundary = 0.5f;


    void Start () {
        ShieldParticles.Pause();
    }
    void Update()
    {
        Vector3 pos = transform.position;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        pos.y += Input.GetAxis("Vertical") * speed * Time.deltaTime;
        pos.x += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        UpdateAnimation();
        pos = ScreenBoundary(pos);
        transform.position = pos;

        PlayerShoot(pos, transform.rotation.z);
        
    }

    public void StartShield () {
        ShieldParticles.Play();
    }

    public void StopShield () {
        ShieldParticles.Pause();
        ShieldParticles.Stop();
        ShieldParticles.time = 0f;

    }
    void UpdateAnimation () {
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
        if ( Obj.gameObject.tag != "PowerUp" && canDamage ){
            Dead();
        }
    }
    void Dead () {
        transform.position = new Vector3(0, -4, -2);
    }

    void PlayerShoot (Vector3 pos, float z_rot) {

        if ( Input.GetKeyDown(KeyCode.Mouse0 ) ) {

            Quaternion rot = new Quaternion(0, 0, z_rot, 0);
            if (ShootPower) {
                for(int begin = -15; begin < 15; begin += 5) {
                    var shoot = Instantiate(Shoot, pos, rot);
                    shoot.transform.Rotate(new Vector3(0, 0, begin));
                    shoot.transform.localScale = new Vector3(0.01f +BulletSize, 0.01f+BulletSize, 0.01f+BulletSize);
                }
            }
            else {
                var shoot = Instantiate(Shoot, pos, rot);
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
