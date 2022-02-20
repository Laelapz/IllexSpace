using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Player;

    public Sprite Item1;
    public Sprite Item2;
    public Sprite Item3;
    public Sprite EmptyItem;

    public GameObject UIHolder;
    private int score = 0;

    void Start()
    {
        UIHolder = GameObject.Find("Canvas");
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
        EnemyVars.worldManager = gameObject.GetComponent<WorldManager>();
        EnemyVars.playerDamage = playerdamage;
        EnemyVars.life = 2;
        EnemyVars.xpBase = 2;
    }

    public void IncreasePoints (int xp) {
        UnityEngine.UI.Text Text = UIHolder.GetComponentInChildren<UnityEngine.UI.Text>();
        score += xp;
        Text.text = score.ToString();
    }

    public void ActivatePower (int type) {
        UnityEngine.UI.Image PowerHolder = UIHolder.GetComponentInChildren<UnityEngine.UI.Image>();
        UnityEngine.UI.Image Power = PowerHolder.GetComponentInChildren<UnityEngine.UI.Image>();

        if ( type == 1 ){
            Power.sprite = Item1;

        }
        else if ( type == 2 ) {
            Power.sprite = Item2;

        }
        else if ( type == 3 ) {
            Power.sprite = Item3;

        }
        else{
            Power.sprite = EmptyItem;
        }

    
    }
}
