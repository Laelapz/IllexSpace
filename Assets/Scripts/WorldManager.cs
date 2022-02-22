using UnityEngine;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour
{
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Player;

    private PlayerController PlayerScript;
    public GameObject Item;
    public GameObject PauseScreen;
    public GameObject PowerScreen;
    public UnityEngine.UI.Text Option1;
    public UnityEngine.UI.Text Option2;
    public UnityEngine.UI.Text Option3;
    public Sprite Item1;
    public Sprite Item2;
    public Sprite Item3;
    public Sprite EmptyItem;

    public GameObject UIHolder;
    UnityEngine.UI.Image PowerHolder;
    UnityEngine.UI.Image Power;

    private int score = 0;
    private int maxScore = 0;
    private float nextLvl = 5f;
    private float actualTime = 0f;
    private float itemTime = 0f;
    private List<string> ChoiceItens = new List<string>();
    private float powerUpTimer;
    private bool isPowerActive = false;
    private bool IsPaused = false;
    public Dictionary<string, string> PlayerData = new Dictionary<string, string>();

    void Start()
    {
        PlayerScript = Player.GetComponent<PlayerController>();

        PlayerData["Damage"] = "1";
        PlayerData["BulletSize"] = "0.0";
        PlayerData["BulletSpeed"] = "10";
        PlayerData["PlayerSize"] = "0.0";

        UIHolder = GameObject.Find("Canvas");
        PowerHolder = UIHolder.GetComponentInChildren<UnityEngine.UI.Image>();
        Power = PowerHolder.GetComponentInChildren<UnityEngine.UI.Image>();

        Enemy1 = (GameObject)Resources.Load("Prefabs/Enemy1", typeof(GameObject));
        Enemy2 = (GameObject)Resources.Load("Prefabs/Enemy2", typeof(GameObject));
        Item = (GameObject)Resources.Load("Prefabs/PowerUp", typeof(GameObject));

        ResetPlayerStats();
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Camera.main.orthographicSize + 1;
        var rot = new Quaternion(0, 0, 0, 0);
        InstantiateEnemies(distance, rot);
        InstantiateItens(distance, rot);
        PowerUpTimer();

        if ( Input.GetKeyDown(KeyCode.R) ) {
            IncreasePoints(1);
        }
        
        if ( Input.GetKeyDown(KeyCode.P) ) {
            GenerateChoiceItens();
            if ( IsPaused ) {
                PauseScreen.SetActive(false);
                Time.timeScale = 1f;
                IsPaused  = false;
            }
            else {
                PauseScreen.SetActive(true);
                Time.timeScale = 0f;
                IsPaused = true;
            }
        }
    }

    public void UnpauseGame () {
        PauseScreen.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
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
        if ( ((int)score) > ((int)maxScore) ) {
            maxScore = score;
        }
        Text.text = score.ToString() + "\n"+maxScore.ToString();
        
        if ( score >= nextLvl ) {
            Time.timeScale = 0f;
            nextLvl *= 2f;
            GenerateChoiceItens();
        }
    }

    public void ActivatePower (int type) {

        if ( type == 1 ){
            Power.sprite = Item1;
            float PlayerScale = -0.5f;
            powerUpTimer = 5f;
            Vector3 actualScale = Player.transform.localScale;

            if ( actualScale.x + PlayerScale > 0.1 ) {
                Player.transform.localScale = new Vector3(actualScale.x + PlayerScale, actualScale.y + PlayerScale, actualScale.z + PlayerScale);
                isPowerActive = true;
            }
        }
        else if ( type == 2 ) {
            Power.sprite = Item2;
            PlayerScript.canDamage = false;
            PlayerScript.StartShield();
            powerUpTimer = 5f;
            isPowerActive = true;

        }
        else if ( type == 3 ) {
            Power.sprite = Item3;
            PlayerScript.ShootPower = true;
            powerUpTimer = 8f;
            isPowerActive = true;

        }
        else{
            Power.sprite = EmptyItem;
        }

    
    }

    public void ResetPlayerStats () {
        PlayerScript.BulletSize = 0.02f;
        PlayerScript.speed = 5;
        Player.transform.localScale = new Vector3(1, 1, 1);
        PlayerScript.canDamage = true;
        UnityEngine.UI.Text Text = UIHolder.GetComponentInChildren<UnityEngine.UI.Text>();
        Text.text = "0\n"+maxScore;
        score = 0;
        nextLvl = 5f;
        ResetPowerUp();
    }
    public void ChoiceBonus (int numOption) {

        string op = ChoiceItens[numOption];
        PowerScreen.SetActive(false);
        Time.timeScale = 1f;

        if ( op == "bullet size" ) {
            PlayerData["BulletSize"] = (float.Parse(PlayerData["BulletSize"]) + 0.01).ToString();
            PlayerScript.BulletSize = float.Parse(PlayerData["BulletSize"]);
        }

        else if ( op == "player size" ) {
            PlayerData["PlayerSize"] = (float.Parse(PlayerData["PlayerSize"]) - 0.01).ToString();
            float PlayerScale = float.Parse(PlayerData["PlayerSize"]);
            Vector3 actualScale = Player.transform.localScale;

            if ( Player.transform.localScale.x + PlayerScale > 0.1 ) {
                Player.transform.localScale = new Vector3(1 + PlayerScale, 1 + PlayerScale, 1 + PlayerScale);
            }else{
                PlayerData["PlayerSize"] = (float.Parse(PlayerData["PlayerSize"]) + 0.01).ToString();
            }
        }

        else if ( op == "player speed" ) {
            PlayerScript.speed += 1;
        }

        else if ( op == "bullet speed" ) {
            PlayerScript.BulletSpeed += 0.1f;
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
        Power.sprite = EmptyItem;
        PlayerScript.damage = 1;
        PlayerScript.canDamage = true;
        PlayerScript.ShootPower = false;
        PlayerScript.StopShield();

        float scale = float.Parse(PlayerData["PlayerSize"]);
        Player.transform.localScale = new Vector3(1+scale, 1+scale, 1+scale);

        scale = float.Parse(PlayerData["BulletSize"]);
        PlayerScript.BulletSize = float.Parse(PlayerData["BulletSize"]);
    }
    public void GenerateChoiceItens () {
        ChoiceItens.Clear();
        List <string> options = new List<string>();
        options.Add("bullet speed");
        options.Add("player speed");
        options.Add("bullet size");
        options.Add("player size");
        options.Add("enemy size");
        
        for (int i = 0; i < 3; i++){
            int num = Random.Range(i, options.Count);
            ChoiceItens.Add(options[num]);
            options.Remove(options[num]);
        }

        Option1.text = ChoiceItens[0];
        Option2.text = ChoiceItens[1];
        Option3.text = ChoiceItens[2];

        PowerScreen.SetActive(true);
    }

    public void SelectedOp1 () {
        ChoiceBonus(0);
    }

    public void SelectedOp2 () {
        ChoiceBonus(1);
    }

    public void SelectedOp3 () {
        ChoiceBonus(2);
    }
}

// if ( type == 1) {
//                 worldManager.ChoiceBonus("PlayerSize");
//             }

//             if ( type == 2) {
//                 worldManager.ChoiceBonus("BulletSize");
//             }