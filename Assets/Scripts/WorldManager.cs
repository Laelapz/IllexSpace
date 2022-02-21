using UnityEngine;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour
{
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Player;

    private PlayerController PlayerScript;

    public GameObject Item;

    public Sprite Item1;
    public Sprite Item2;
    public Sprite Item3;
    public Sprite EmptyItem;

    public GameObject UIHolder;
    private int score = 0;
    private float actualTime = 0f;
    private float itemTime = 0f;
    private float powerUpTimer;
    private bool isPowerActive = false;
    public Dictionary<string, string> PlayerData = new Dictionary<string, string>();

    void Start()
    {
        PlayerScript = Player.GetComponent<PlayerController>();

        ResetPlayerStats();

        PlayerData["Damage"] = "1";
        PlayerData["BulletSize"] = "0.0";
        PlayerData["BulletSpeed"] = "10";
        PlayerData["PlayerSize"] = "0.0";

        UIHolder = GameObject.Find("Canvas");
        Enemy1 = (GameObject)Resources.Load("Prefabs/Enemy1", typeof(GameObject));
        Enemy2 = (GameObject)Resources.Load("Prefabs/Enemy2", typeof(GameObject));
        Item = (GameObject)Resources.Load("Prefabs/PowerUp", typeof(GameObject));
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Camera.main.orthographicSize + 1;
        var rot = new Quaternion(0, 0, 0, 0);
        // InstantiateEnemies(distance, rot);
        InstantiateItens(distance, rot);
        PowerUpTimer();

        if ( Input.GetKeyDown(KeyCode.KeypadMinus) ) {
            ChoiceBonus("PlayerSize");
        }

        if ( Input.GetKeyDown(KeyCode.KeypadPlus) ) {
            ChoiceBonus("BulletSize");
        }

        if ( Input.GetKeyDown(KeyCode.R) ) {
            ResetPowerUp();
        }
        
    }

    void InstantiateEnemies (float distance, Quaternion rot) {
        if ( actualTime > 2) {
            
            float ScreenRatio = (float)Screen.width / (float)Screen.height;
            float ScreenOrtho = (Camera.main.orthographicSize * ScreenRatio)-1;  

            int num = Random.Range(0, 2 );
            int pos_x = Random.Range(((int)-ScreenOrtho), ((int)ScreenOrtho));

            GameObject enemy = null;

            if ( num == 0 ){
                enemy = Instantiate(Enemy1, new Vector3(pos_x, distance, -1), rot);
            }else{
                enemy = Instantiate(Enemy2, new Vector3(pos_x, distance, -1), rot);
            }
            
            int playerdamage = Player.GetComponents<PlayerController>()[0].damage;
            var EnemyVars = enemy.GetComponent<EnemyScript>();
            EnemyVars.worldManager = gameObject.GetComponent<WorldManager>();
            EnemyVars.playerDamage = playerdamage;
            EnemyVars.life = 2;
            EnemyVars.xpBase = 2;

            actualTime = 0f;
        }
        else {
            actualTime += Time.deltaTime;
        }
    }

    void InstantiateItens (float distance, Quaternion rot) {

        if ( itemTime > 10 ) {
            int num = Random.Range(0, 4);

            float ScreenRatio = (float)Screen.width / (float)Screen.height;
            float ScreenOrtho = (Camera.main.orthographicSize * ScreenRatio)-1;  

            int pos_x = Random.Range(((int)-ScreenOrtho), ((int)ScreenOrtho));
            GameObject item = null;

            if ( num == 1 ) {
                item = Instantiate(Item, new Vector3(pos_x, distance, -1), rot);
                item.GetComponent<PowerUpScript>().type = 1;
                item.GetComponent<PowerUpScript>().worldManager = gameObject.GetComponent<WorldManager>();
                item.GetComponent<PowerUpScript>().SetSprite(Item1);
            }
            else if ( num == 2 ) {
                item = Instantiate(Item, new Vector3(pos_x, distance, -1), rot);
                item.GetComponent<PowerUpScript>().type = 2;
                item.GetComponent<PowerUpScript>().worldManager = gameObject.GetComponent<WorldManager>();
                item.GetComponent<PowerUpScript>().SetSprite(Item2);
            }
            else if ( num == 3 ) {
                item = Instantiate(Item, new Vector3(pos_x, distance, -1), rot);
                item.GetComponent<PowerUpScript>().type = 3;
                item.GetComponent<PowerUpScript>().worldManager = gameObject.GetComponent<WorldManager>();
                item.GetComponent<PowerUpScript>().SetSprite(Item3);
            }

            itemTime = 0f;
        }
        else {
            itemTime += Time.deltaTime;
        }
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
            float PlayerScale = -0.5f;
            powerUpTimer = 5f;
            Vector3 actualScale = Player.transform.localScale;
            Player.transform.localScale = new Vector3(actualScale.x + PlayerScale, actualScale.y + PlayerScale, actualScale.z + PlayerScale);
            isPowerActive = true;
        }
        else if ( type == 2 ) {
            Power.sprite = Item2;
            Player.GetComponent<PlayerController>().canDamage = false;

        }
        else if ( type == 3 ) {
            Power.sprite = Item3;

        }
        else{
            Power.sprite = EmptyItem;
        }

    
    }

    public void ResetPlayerStats () {
        PlayerScript.BulletSize = 0.01f;
        PlayerScript.speed = 5;
        Player.transform.localScale = new Vector3(1, 1, 1);
        PlayerScript.canDamage = true;
    }
    public void ChoiceBonus (string op) {
        if ( op == "BulletSize" ) {
            PlayerData["BulletSize"] = (float.Parse(PlayerData["BulletSize"]) + 0.01).ToString();
            PlayerScript.BulletSize = float.Parse(PlayerData["BulletSize"]);
        }

        else if ( op == "PlayerSize" ) {
            PlayerData["PlayerSize"] = (float.Parse(PlayerData["BulletSize"]) - 0.01).ToString();
            float PlayerScale = float.Parse(PlayerData["PlayerSize"]);
            Vector3 actualScale = Player.transform.localScale;
            Player.transform.localScale = new Vector3(actualScale.x + PlayerScale, actualScale.y + PlayerScale, actualScale.z + PlayerScale);
        }

    }

    public void PowerUpTimer () {
        if ( isPowerActive ) {
            if ( powerUpTimer > 0) {
                powerUpTimer -= Time.deltaTime;
            }
            else {
                isPowerActive = false;
                powerUpTimer = 0f;
                ResetPowerUp();
            }
        }
    }
    public void ResetPowerUp () {
        PlayerScript.damage = 1;
        PlayerScript.canDamage = true;

        float scale = float.Parse(PlayerData["PlayerSize"]);
        Player.transform.localScale = new Vector3(1+scale, 1+scale, 1+scale);

        scale = float.Parse(PlayerData["BulletSize"]);
        PlayerScript.BulletSize = float.Parse(PlayerData["BulletSize"]);
    }

}

