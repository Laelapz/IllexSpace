using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private Slider XpBar;
    private PlayerController PlayerScript;
    private List<string> ChoiceItens = new List<string>();
    private Camera mainCamera;
    
    public AudioSource BackgroundMusic;

    [Header("Enemy Prefabs")]
    public GameObject[] Enemies;
    public GameObject Player;


    [Header("UI Objects")]
    public GameObject Item;
    public GameObject PauseScreen;
    public GameObject PowerScreen;
    public GameObject UIHolder;
    public Image PowerHolder;
    public Image Power;

    [Header("Buttons")]
    [SerializeField] private Button upgrade1;
    [SerializeField] private Button upgrade2;
    [SerializeField] private Button upgrade3;

    [Header("UI Texts")]
    public Text Option1;
    public Text Option2;
    public Text Option3;

    [Header("Sprite Items")]
    public Sprite Item1;
    public Sprite Item2;
    public Sprite Item3;
    public Sprite Item4;
    public Sprite EmptyItem;


    
    private int score = 0;
    private int maxScore = 0;
    private int actualOp = -1;
    private int nextLvl = 50;
    private float actualTime = 0f;
    private float itemTime = 0f;
    private float powerUpTimer;
    private float currentEnemySize = 1.5f;
    private float ScreenRatio;
    private float ScreenOrtho;
    private float distance;

    private float enemySpawnTime = 2f;

    private List<string> options;

    [Header("Bools")]
    public bool IsPaused = false;
    private bool isPowerActive = false;
    private Quaternion rot;

    void Start()
    {
        mainCamera = Camera.main;
        options = new List<string>();

        ScreenRatio = (float)Screen.width / (float)Screen.height;
        ScreenOrtho = (mainCamera.orthographicSize * ScreenRatio) - 1;
        distance = mainCamera.orthographicSize + 1;
        rot = new Quaternion(0, 0, 0, 0);

        PlayerScript = Player.GetComponent<PlayerController>();

        UIHolder = GameObject.Find("Canvas");
        Item = (GameObject)Resources.Load("Prefabs/PowerUp", typeof(GameObject));


        XpBar.value = score;
        XpBar.maxValue = nextLvl;

        ResetPlayerStats();
        
    }

    void Update()
    {
        InstantiateEnemies();
        InstantiateItens();
        PowerUpTimer();
    }

    public void PauseGame()
    {
        if (!PowerScreen.activeSelf)
        {
            if (IsPaused)
            {
                BackgroundMusic.volume = 1f;
                PauseScreen.SetActive(false);
                Time.timeScale = 1f;
                IsPaused = false;
            }
            else
            {
                BackgroundMusic.volume = 0.4f;
                PauseScreen.SetActive(true);
                Time.timeScale = 0f;
                IsPaused = true;
            }
        }
    }

    public void UnpauseGame () {
        BackgroundMusic.volume = 1f;
        PauseScreen.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }
    public void QuitGame () {
        Time.timeScale = 1f;
        IsPaused = false;
        SceneManager.LoadScene("Assets/Scenes/Menu.unity");
    }

    void InstantiateEnemies () {
        if ( actualTime > enemySpawnTime) {
            

            int num = Random.Range(0, Enemies.Length );
            int pos_x = Random.Range(((int)-ScreenOrtho), ((int)ScreenOrtho));
            GameObject enemy = null;
            EnemyScript EnemyVars = null;

            if ( num == 0 ){
                enemy = Instantiate(Enemies[0], new Vector3(pos_x, distance, -2), rot);
                EnemyVars = enemy.GetComponent<EnemyScript>();
                EnemyVars.life = 1;
                EnemyVars.xpBase = 1;
            }
            else if ( num == 1) {
                enemy = Instantiate(Enemies[1], new Vector3(pos_x, distance, -2), rot);
                EnemyVars = enemy.GetComponent<EnemyScript>();
                EnemyVars.life = 2;
                EnemyVars.xpBase = 2;
            }
            else if (num == 2) {
                enemy = Instantiate(Enemies[2], new Vector3(pos_x, distance, -2), rot);
                EnemyVars = enemy.GetComponent<EnemyScript>();
                EnemyVars.life = 2;
                EnemyVars.xpBase = 3;
            }
            else
            {
                enemy = Instantiate(Enemies[3], new Vector3(pos_x, distance, -2), rot);
                EnemyVars = enemy.GetComponent<EnemyScript>();
                var horizontalMovimentScript = enemy.GetComponent<HorizontalMovement>();
                horizontalMovimentScript.amplitude = 1f;
                horizontalMovimentScript.speed = 1f;
                EnemyVars.life = 1;
                EnemyVars.xpBase = 2;
            }

            int playerdamage = Player.GetComponents<PlayerController>()[0].currentPlayerDamage;
            enemy.transform.localScale = (new Vector3(currentEnemySize, currentEnemySize, currentEnemySize));
            EnemyVars.worldManager = gameObject.GetComponent<WorldManager>();
            EnemyVars.playerDamage = playerdamage;

            actualTime = 0f;
        }
        else {
            actualTime += Time.deltaTime;
        }
    }

    private void InstantiateItens () {
        if ( itemTime > 10 ) {
            int num = Random.Range(0, 4);

            float ScreenRatio = (float)Screen.width / (float)Screen.height;
            float ScreenOrtho = (mainCamera.orthographicSize * ScreenRatio)-1;  

            int pos_x = Random.Range(((int)-ScreenOrtho), ((int)ScreenOrtho));
            GameObject item = null;

            if (num == 0) {
                item = Instantiate(Item, new Vector3(pos_x, distance, -1), rot);
                item.GetComponent<PowerUpScript>().type = 0;
                item.GetComponent<PowerUpScript>().worldManager = gameObject.GetComponent<WorldManager>();
                item.GetComponent<PowerUpScript>().SetSprite(Item4);
            }
            else if ( num == 1 ) {
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

        Text Text = UIHolder.GetComponentInChildren<Text>();

        score += xp;

        if ( ((int)score) > ((int)maxScore) ) {
            maxScore = score;
        }
        Text.text = score.ToString() + "\n\n"+maxScore.ToString();

        if ( score >= nextLvl ) {
            Time.timeScale = 0f;
            nextLvl += 50;
            XpBar.maxValue = nextLvl;

            if (enemySpawnTime > 0.6f) enemySpawnTime -= 0.2f;
            GenerateChoiceItens();
        }

        XpBar.value = score;
    }

    public void ActivatePower (int type) {
        Text Text = UIHolder.GetComponentInChildren<Text>();

        if (type == 0) {
            IncreasePoints(10);
            
            powerUpTimer = 0.1f;
            isPowerActive = true;
        }
        else if ( type == 1 ){
            Power.sprite = Item1;
            powerUpTimer = 5f;
            float actualScale = PlayerScript.currentPlayerSize;
            Player.transform.localScale = new Vector3(actualScale - 0.2f, actualScale - 0.2f, actualScale - 0.2f);
            isPowerActive = true;
        
        }
        else if ( type == 2 ) {
            Power.sprite = Item2;
            PlayerScript.AddInvulnerabilityTime(5);
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

        PlayerScript.AddInvulnerabilityTime(PlayerScript.currentInvulnerabilityTime);
    }

    public void ResetPlayerStats () {

        PlayerScript.ResetStatus();

        Player.transform.localScale = new Vector3(PlayerScript.currentPlayerSize, PlayerScript.currentPlayerSize, PlayerScript.currentPlayerSize);
        currentEnemySize = 1.5f;
        enemySpawnTime = 2f;
        PlayerScript.canDamage = true;
        Text Text = UIHolder.GetComponentInChildren<UnityEngine.UI.Text>();
        Text.text = "0\n\n"+maxScore;
        score = 0;
        nextLvl = 50;


        XpBar.value = score;
        XpBar.maxValue = nextLvl;

        ResetPowerUp();
    }

    public void ChoiceBonus (int numOption) {

        IsPaused = false;
        BackgroundMusic.volume = 1f;
        actualOp = -1;
        string op = ChoiceItens[numOption];
        PowerScreen.SetActive(false);
        Time.timeScale = 1f;

        if ( op == "bullet size" ) {
            PlayerScript.currentBulletSize += 0.01f;
        }

        else if ( op == "player size" ) {
            PlayerScript.currentPlayerSize -= 0.1f;
            Player.transform.localScale = new Vector3(PlayerScript.currentPlayerSize, PlayerScript.currentPlayerSize, PlayerScript.currentPlayerSize);
        }

        else if (op == "enemy size")
        {
            currentEnemySize += 0.1f;
        }

        else if ( op == "player speed" )
        {
            PlayerScript.currentPlayerSpeed += 1;
        }

        else if (op == "reaload speed")
        {
            PlayerScript.currentShootCooldown -= 0.1f;
        }

        else if ( op == "bullet speed" )
        {
            PlayerScript.currentBulletSpeed += 0.1f;
        }

        else if (op == "invulnerability time")
        {
            PlayerScript.currentInvulnerabilityTime += 0.2f;
        }

        PlayerScript.AddInvulnerabilityTime(PlayerScript.currentInvulnerabilityTime);
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
        PlayerScript.canDamage = true;
        PlayerScript.Flash(0);
        PlayerScript.ShootPower = false;
        PlayerScript.StopShield();

        Player.transform.localScale = new Vector3(PlayerScript.currentPlayerSize, PlayerScript.currentPlayerSize, PlayerScript.currentPlayerSize);
    }

    public void GenerateChoiceItens () {

        IsPaused = true;
        BackgroundMusic.volume = 0.5f;
        ChoiceItens.Clear();
        options.Clear();
        
        if(PlayerScript.currentInvulnerabilityTime < 10) options.Add("invulnerability time");
        if(PlayerScript.currentPlayerSize > 0.3f) options.Add("player size");
        if(currentEnemySize < 3f) options.Add("enemy size");
        if(PlayerScript.currentShootCooldown > 0.15f) options.Add("reaload speed");
        if(PlayerScript.currentBulletSize < 0.08f) options.Add("bullet size");
        if(PlayerScript.currentBulletSpeed < 15) options.Add("bullet speed");
        
        options.Add("player speed");
        
        if (options.Count < 3) return;

        for (int i = 0; i < 3; i++){
            int num = Random.Range(0, options.Count);
            ChoiceItens.Add(options[num]);
            options.RemoveAt(num);
        }


        Option1.text = ChoiceItens[0];
        Option2.text = ChoiceItens[1];
        Option3.text = ChoiceItens[2];

        PowerScreen.SetActive(true);
    }

    public void ConfirmItem () {
        //Função para previnir a escolha de um item sem querer
        //Esta função só permite o jogo continuar após clicar no "OK"  com um bonus selecionado antes

        if ( actualOp != -1 ) {
            ChoiceBonus(actualOp);
        }
    }
 
    public void SelectedOp1 () {
        actualOp = 0;
        upgrade1.Select();
    }

    public void SelectedOp2 () {
        actualOp = 1;
        upgrade2.Select();
    }

    public void SelectedOp3 () {
        actualOp = 2;
        upgrade3.Select();
    }
}
