using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
public class GameController : MonoBehaviour
{
    private int score = 0;
    private int coinsCollected = 0;
    private float count1 = 0;
    private float count2 = 0;
    [Header("PLayer Elments")]
    [SerializeField] private PlayerC player;

    [Header("Coins Event Elments ")]
    //Lista de las monedas sencillas
    [SerializeField] private List<CoinC> coinsOnMap;
    //Objetos S3Coin que es la moneda especial con la que termina el minijuego
    [SerializeField] private CoinC grandCoin;
    //Lista de objetos S3Coin para la lógica de recolección
    private List<CoinC> coinsRemaining;

    [Header("PowerUps Event Elments ")]
    //Lista de powerUps sencilla
    [SerializeField] private List<PowerUpC> powerUpsOnMap;
    //Lista de objetos CoinC para la lógica de recolección
    private List<PowerUpC> powerUpsRemaining;

    [Header("Enemys Event Elments ")]
    //Lista de objetos S3Coin que la hacemos [SerializeField] para editarla desde el inspector llenando con las monedas sencillas
    [SerializeField] private EnemyC[] enemysOnMap;

    [Header("Invocar Subscribers")]
    //UnityEvent que invocará a todos los Subscribers cuando se termine el Nivel 1
    [SerializeField] private UnityEvent PerspectiveCameraEvent;
    //UnityEvent que invocará a todos los Subscribers cuando se termine el Nivel 1
    [SerializeField] private UnityEvent OrthographicCameraEvent;
    //UnityEvent que invocará a todos los Subscribers cuando se termine el Nivel 1
    [SerializeField] private UnityEvent ReciveDamageCameraEvent;
    //UnityEvent que invocará a todos los Subscribers cuando se termine el Nivel 1
    [SerializeField] private UnityEvent onFinishLevel1;
    //UnityEvent que invocará a todos los Subscribers cuando se termine el Nivel 2
    [SerializeField] private UnityEvent onFinishLevel2;
    //UnityEvent que invocará a todos los Subscribers cuando se termine el juego
    [SerializeField] private UnityEvent onEndGameWin;
    [SerializeField] private UnityEvent onEndGameOver;

    [SerializeField] CinemachineBasicMultiChannelPerlin camaraCam;
    
    void Start()
    {
        LogicCoins();
        LogicPowerUps();
        LogicReciveDamage();
        LogicKillEnemy();
    }
    public void LogicCoins()
    {
        coinsRemaining = new List<CoinC>(coinsOnMap); //Se inicializa nuestra lista de objetos faltantes por recolectar usando nuestra primera lista como base

        //Haciendo un ForEach (loop por cada elemento) se accede al evento action de cada S3Coin y hacemos que nuestro método HandleCollected se subscriba a dicho evento
        foreach (CoinC coin in coinsRemaining)
        {
            coin.onColleted += CoinssssCollected;
        }

        coinsRemaining.Add(grandCoin); //Y luego se añade la moneda final
        grandCoin.onColleted += CoinssssCollected; //Y subscribimos nuestro método HandleCollected también a este último objeto

    }
    public void LogicPowerUps()
    {
        powerUpsRemaining = new List<PowerUpC>(powerUpsOnMap);
        foreach (PowerUpC powerUp in powerUpsRemaining)
        {
            powerUp.onColleted += PowerUPCollected;
        }
    }
    public void LogicReciveDamage()
    {
        for (int i = 0; i < enemysOnMap.Length; i++)
        {
            enemysOnMap[i].OnHitEnemy += HitsCollectedEnemy;
        }
    }
    public void LogicKillEnemy()
    {
        for (int i = 0; i < enemysOnMap.Length; i++)
        {
            enemysOnMap[i].OnKillEnemy += KillEnemy;
        }
    }

    private void OnGUI()
    {
        //Se instancia en la GUI dos textos de Monedas Recolectadas y el Puntaje Total
        GUI.Label(new Rect(600, 10, 500, 20), string.Format("Coins Collected: {0}", coinsCollected));
        GUI.Label(new Rect(600, 30, 500, 20), string.Format("Total Score: {0}", score));
        GUI.Label(new Rect(40, 10, 500, 20), string.Format("Vidas: {0}", player.vidas));

        count1 = count1 - Time.deltaTime;
        count2 = count2 - Time.deltaTime;
        if (count1 >= 0)
        {
            GUI.Label(new Rect(250, 180, 500, 20), string.Format("Te pegaron"));
        }
        if (count2 >= 0)
        {
            GUI.Label(new Rect(300, 180, 500, 20), string.Format("Mataste a {0}", name));
        }

    }

    private void CoinssssCollected(CoinC coin)
    {
        coinsRemaining.Remove(coin);

        coinsCollected++;

        score += coin.coinValue;

        if (coinsRemaining.Count == 9)
        {
            onFinishLevel1?.Invoke(); //Invocar el evento cuando se termina el nivel 1

        }
        if (coinsRemaining.Count == 5)
        {
            onFinishLevel2?.Invoke(); //Invocar el evento cuando se termina el nivel 2

        }
        if (coinsRemaining.Count == 0)
        {
            onEndGameWin?.Invoke(); //Invocar el evento cuando se termina el minijuego
        }
    }

    private void PowerUPCollected(PowerUpC PowerUpC)
    {
        powerUpsRemaining.Remove(PowerUpC);

        if (powerUpsRemaining.Count%2==0)
        {
            OrthographicCameraEvent?.Invoke();
        }
        if (powerUpsRemaining.Count % 2 == 1)
        {
            PerspectiveCameraEvent?.Invoke();
        }
    }

    private void HitsCollectedEnemy(EnemyC enemyC)
    {
        player.vidas = player.vidas - enemyC.damage;
        ReciveDamageCameraEvent?.Invoke();
        count1 = 2;
        if (player.vidas <= 0)
        {
            onEndGameOver?.Invoke();
        }
    }
    private void KillEnemy(EnemyC enemyC)
    {
        count2 = 2;
        name = enemyC.name;
    }
}
