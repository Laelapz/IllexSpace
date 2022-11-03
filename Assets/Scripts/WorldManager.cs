using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour
{
    public AudioSource BackgroundMusic;
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
    public GameObject Player;

    private PlayerController PlayerScript;
    public GameObject Item;
    public GameObject PauseScreen;
    public GameObject PowerScreen;

    [Header("Buttons")]
    [SerializeField] private Button upgrade1;
    [SerializeField] private Button upgrade2;
    [SerializeField] private Button upgrade3;

    [Header("UITexts")]
    public UnityEngine.UI.Text Option1;
    public UnityEngine.UI.Text Option2;
    public UnityEngine.UI.Text Option3;

    [Header("SpriteItems")]
    public Sprite Item1;
    public Sprite Item2;
    public Sprite Item3;
    public Sprite Item4;
    public Sprite EmptyItem;

    public GameObject UIHolder;
    UnityEngine.UI.Image PowerHolder;
    UnityEngine.UI.Image Power;

    private int score = 0;
    private int maxScore = 0;
    private int actualOp = -1;
    private float nextLvl = 5f;
    private float actualTime = 0f;
    private float itemTime = 0f;
    private List<string> ChoiceItens = new List<string>();
    private float powerUpTimer;
    private bool isPowerActive = false;
    public bool IsPaused = false;

    private float currentEnemySize = 1.5f;

    //Atribui os diferentes Objetos as suas variáveis
    void Start()
    {
        
        PlayerScript = Player.GetComponent<PlayerController>();

        UIHolder = GameObject.Find("Canvas");
        PowerHolder = UIHolder.GetComponentInChildren<UnityEngine.UI.Image>();
        Power = PowerHolder.GetComponentInChildren<UnityEngine.UI.Image>();

        Enemy1 = (GameObject)Resources.Load("Prefabs/Enemy1", typeof(GameObject));
        Enemy2 = (GameObject)Resources.Load("Prefabs/Enemy2", typeof(GameObject));
        Item = (GameObject)Resources.Load("Prefabs/PowerUp", typeof(GameObject));

        ResetPlayerStats();
        
    }

    // O Update tem a função de cuidar dos inputs do player e chamar as funções que vão cuidar de partes específicas do jogo
    //Ex: movimentação | Pausa | Intanciar Prefabs
    void Update()
    {
        float distance = Camera.main.orthographicSize + 1;
        var rot = new Quaternion(0, 0, 0, 0);
        InstantiateEnemies(distance, rot);
        InstantiateItens(distance, rot);
        PowerUpTimer();
    }

    public void PauseGame()
    {
        if (!PowerScreen.activeSelf)
        {
            if (IsPaused)
            {
                //Se esta pausado volta as configurações ao normal
                BackgroundMusic.volume = 1f;
                PauseScreen.SetActive(false);
                Time.timeScale = 1f;
                IsPaused = false;
            }
            else
            {
                //Se não muda as configurações para parar o jogo e reduzir o volume 
                BackgroundMusic.volume = 0.4f;
                PauseScreen.SetActive(true);
                Time.timeScale = 0f;
                IsPaused = true;
            }
        }
    }

    public void UnpauseGame () {
        //Função para despausar através do Resume
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
    void InstantiateEnemies (float distance, Quaternion rot) {
        //Função usa como timer o tempo entre cada frame para spawnar os prefabs
        if ( actualTime > 2) {
            //Caso já tenha passado o tempo necessário
            
            //Encontra as proporções da tela para poder definir o range do spawn dos inimigos em relação ao orthographicSize
            float ScreenRatio = (float)Screen.width / (float)Screen.height;
            float ScreenOrtho = (Camera.main.orthographicSize * ScreenRatio)-1;  

            int num = Random.Range(0, 3 );
            int pos_x = Random.Range(((int)-ScreenOrtho), ((int)ScreenOrtho));

            GameObject enemy = null;
            EnemyScript EnemyVars = null;

            //Setando as variáveis para cada tipo de inimigo
            if ( num == 0 ){
                enemy = Instantiate(Enemy1, new Vector3(pos_x, distance, -2), rot);
                EnemyVars = enemy.GetComponent<EnemyScript>();
                EnemyVars.life = 1;
                EnemyVars.xpBase = 1;
            }
            else if ( num == 1) {
                enemy = Instantiate(Enemy2, new Vector3(pos_x, distance, -2), rot);
                EnemyVars = enemy.GetComponent<EnemyScript>();
                EnemyVars.life = 2;
                EnemyVars.xpBase = 2;
            }
            else {
                enemy = Instantiate(Enemy3, new Vector3(pos_x, distance, -2), rot);
                EnemyVars = enemy.GetComponent<EnemyScript>();
                EnemyVars.life = 2;
                EnemyVars.xpBase = 3;
            }
            
            int playerdamage = Player.GetComponents<PlayerController>()[0].currentPlayerDamage;
            print(enemy.transform.localScale);
            enemy.transform.localScale = (new Vector3(currentEnemySize, currentEnemySize, currentEnemySize));
            print(enemy.transform.localScale);

            EnemyVars.worldManager = gameObject.GetComponent<WorldManager>();
            EnemyVars.playerDamage = playerdamage;

            actualTime = 0f;
        }
        else {
            //Se não adiciona o tempo passado desde o último frame
            actualTime += Time.deltaTime;
        }
    }

    private void InstantiateItens (float distance, Quaternion rot) {
        //Usa a mesma lógica do spawn de inimigos
        if ( itemTime > 10 ) {
            int num = Random.Range(0, 4);

            float ScreenRatio = (float)Screen.width / (float)Screen.height;
            float ScreenOrtho = (Camera.main.orthographicSize * ScreenRatio)-1;  

            int pos_x = Random.Range(((int)-ScreenOrtho), ((int)ScreenOrtho));
            GameObject item = null;

            //Todos os itens são o mesmo prefab que apenas carregam o seu efeito(type) e o sprite

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
        //Atualiza os pontos atuais

        UnityEngine.UI.Text Text = UIHolder.GetComponentInChildren<UnityEngine.UI.Text>();

        //Os pontos também gerenciam um sistema de xp que permite a escolha de bonus conforme progride no game
        score += xp;

        if ( ((int)score) > ((int)maxScore) ) {
            maxScore = score;
        }
        Text.text = score.ToString() + "\n\n"+maxScore.ToString();
        
        //Caso seu score atual seja maior que o limite necessario para passar de lvl
        //É liberado o acesso a tela de escolha de item e a quantia necessária para upar de nível é dobrada
        if ( score >= nextLvl ) {
            Time.timeScale = 0f;
            nextLvl *= 2f;
            GenerateChoiceItens();
        }
    }

    public void ActivatePower (int type) {
        UnityEngine.UI.Text Text = UIHolder.GetComponentInChildren<UnityEngine.UI.Text>();

        //Todos os itens brincam com as váriáveis do jogo sendo possível diminuir o tamanho do player | aumentar os inimigos
        //Ficar mais rápido | ter balas maiores, entre outros

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
        //Função para resetar os status totalmente em caso de morte
        //Diferente do ResetPowerUp esta função muda o dicionário que monitora os efeitos dos itens escolhidos
        //Podendo assim acumular bonus ao longo de uma jogatina, mas perdendo todos ao morrer

        PlayerScript.ResetStatus();

        Player.transform.localScale = new Vector3(PlayerScript.currentPlayerSize, PlayerScript.currentPlayerSize, PlayerScript.currentPlayerSize);
        currentEnemySize = 1.5f;
        PlayerScript.canDamage = true;
        UnityEngine.UI.Text Text = UIHolder.GetComponentInChildren<UnityEngine.UI.Text>();
        Text.text = "0\n\n"+maxScore;
        score = 0;
        nextLvl = 5f;
        
        ResetPowerUp();
    }

    public void ChoiceBonus (int numOption) {
        //Desativa a tela de escolha dos itens e aplica o bonus escolhido

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
            PlayerScript.currentPlayerSize -= 0.01f;
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
        //Timer manual para monitorar e desativar os PowerUps das caixas

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
        //Desativa os bonus oferecidos temporariamente

        Power.sprite = EmptyItem;
        //PlayerScript.ResetStatus
        PlayerScript.canDamage = true;
        PlayerScript.Flash(0);
        PlayerScript.ShootPower = false;
        PlayerScript.StopShield();

        Player.transform.localScale = new Vector3(PlayerScript.currentPlayerSize, PlayerScript.currentPlayerSize, PlayerScript.currentPlayerSize);
    }

    public void GenerateChoiceItens () {
        //Gera os itens aleatóriamente para serem escolhidos e ativa a tela de seleção

        IsPaused = true;
        BackgroundMusic.volume = 0.5f;
        ChoiceItens.Clear();
        List <string> options = new List<string>();
        if(PlayerScript.currentInvulnerabilityTime < 10)    options.Add("invulnerability time");
        if(PlayerScript.currentPlayerSize > 0.3f) options.Add("player size");
        if (currentEnemySize < 3f) options.Add("enemy size");
        
        options.Add("bullet speed");
        options.Add("player speed");
        options.Add("bullet size");

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

    public void ConfirmItem () {
        //Função para previnir a escolha de um item sem querer
        //Esta função só permite o jogo continuar após clicar no "OK"  com um bonus selecionado antes

        if ( actualOp != -1 ) {
            ChoiceBonus(actualOp);
        }
    }
   
   //Funções apenas para setar o bonus escolhido em cada lvl
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
