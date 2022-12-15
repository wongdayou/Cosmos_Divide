using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperClasses;

public class WaveSpawner : MonoBehaviour
{
    // WAITING: waiting for the player to kill off all enemies
    // COUNTDOWN: countdown to the start of the next wave
    public enum WaveState { SPAWNING, WAITING, COUNTDOWN, END };

    static GameMaster gm;
    public float countdownTime = 5f;
    public float searchRate = 2f;
    private float waveCountdown;
    private float searchCountdown;
    private int nextWave = 0;
    public Transform target;
    protected int waveCounter = 0;



    [System.Serializable]
    public class Wave{
        public string name;
        public WaveEnemyInfo[] waveEnemies;
        public GameObject boss;
        public float bossSpawnTime = 60f;
        public int numOfEnemies;

        public void GetNumOfEnemies () {
            int numOfTypesOfEnemies = waveEnemies.Length;
            int totalNumOfEnemies = 0;
            for (int i = 0; i < numOfTypesOfEnemies; i ++){
                totalNumOfEnemies += waveEnemies[i].count;
            }
            numOfEnemies = totalNumOfEnemies;
        }
    }



    [System.Serializable]
    public class WaveEnemyInfo {
        public GameObject enemy;
        public int count;
        public float cooldown;
    }




    public Wave[] waves;
    private WaveState waveState = WaveState.COUNTDOWN;
    public SpawnPoint[] spawnPoints;


    LevelManager levelManager;



    private void Awake() {
        if (gm == null){
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
            if (gm == null){
                Debug.LogError("WaveSpawner: GameMaster not found (Awake())");
            }
        }

        levelManager = this.gameObject.GetComponent<LevelManager>();
        if (levelManager == null){
            Debug.LogError("WaveSpawner: no levelManager found");
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        waveCountdown = countdownTime;
        if (spawnPoints.Length <= 0){
            Debug.LogError("WaveSpawner: no spawn points!");
        }

        waves[nextWave].GetNumOfEnemies();

    }



    // Update is called once per frame
    void Update()
    {
        switch (waveState) {
            case WaveState.SPAWNING: return;
            case WaveState.WAITING: return;
                // if (searchCountdown <= 0f) {
                //     if (GameObject.FindGameObjectWithTag("Enemy") == null){
                //         WaveCompleted();
                //     }
                //     searchCountdown = searchRate;
                // }
                // else {
                //     searchCountdown -= Time.deltaTime;
                // }
                // break;

            case WaveState.COUNTDOWN: 
                if (waveCountdown <= 0) {
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
                    
                else {
                    waveCountdown -= Time.deltaTime;
                }
                break;

            case WaveState.END:
                levelManager.EndLevel();
                break;
        }
    }



    void WaveCompleted() {
        
        if (nextWave + 1 > waves.Length - 1) {
            Debug.Log("All waves cleared");
            waveState = WaveState.END;
        }
        else {
            waveState = WaveState.COUNTDOWN;
            waveCountdown = countdownTime;
            nextWave ++;
            Debug.Log("Spawning Next Wave");
            waves[nextWave].GetNumOfEnemies();
        }

        return;
    }




    IEnumerator SpawnWave(Wave _wave){
        //TODO 
        //Spawn each type of enemy at their own rate
        //Spawn the boss after a specific time
        if (waveState == WaveState.SPAWNING) yield return false;
        waveState = WaveState.SPAWNING;
        waveCounter = _wave.waveEnemies.Length;
        if (_wave.boss != null) {
            waveCounter += 1;
        }
        foreach (WaveEnemyInfo w in _wave.waveEnemies){
            StartCoroutine(SpawnEnemy(w));
        }

        //spawn boss after certain amount of time
        if (_wave.boss != null){
            yield return new WaitForSeconds(_wave.bossSpawnTime);
            CreateGameObject(_wave.boss);
            waveCounter--;
            if (waveCounter <= 0){
                waveState = WaveState.WAITING;
            }
        }
        
        yield return false;
    }




    IEnumerator SpawnEnemy(WaveEnemyInfo _waveEnemy){
        //if -1 then spawn infinitely until player dies
        if (_waveEnemy.count == -1){
            while (true) {
                // if (gm.CanSpawn()){
                //     gm.PlusShip();
                //     Debug.Log("Spawning");
                //     CreateGameObject(_waveEnemy.enemy);
                    
                // }

                if (gm.CanSpawn(Team.RED, _waveEnemy.enemy.GetComponent<Entity>().data.load)){
                    //Debug.Log("Spawning");
                    CreateGameObject(_waveEnemy.enemy);
                    
                }
                yield return new WaitForSeconds( _waveEnemy.cooldown );
            }
        }

        int i = 0;
        while (i < _waveEnemy.count){

            if (gm.CanSpawn(Team.RED, _waveEnemy.enemy.GetComponent<Entity>().data.load)){
                CreateGameObject(_waveEnemy.enemy);
                i++;
            }
            yield return new WaitForSeconds( _waveEnemy.cooldown );
            
        }

        waveCounter --;
        if (waveCounter <= 0){
            waveState = WaveState.WAITING;
        }
        yield return false;
    }





    //function to Create a ship (gameObject) at a random spawn point
    private void CreateGameObject (GameObject _go) {
        SpawnPoint _spawnPlace = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector3 _sp = _spawnPlace.GetRandomPosition();
        Quaternion _spr;
        if (target != null){
            _spr = Quaternion.LookRotation(Vector3.forward, (target.position - _sp).normalized);
        }
        else{
            Vector3 _mid = new Vector3(0f, 0f, 0f);
            _spr = Quaternion.LookRotation(Vector3.forward, (_mid - _sp).normalized);
        }
        
        //Debug.Log("Creating ship");
        GameObject newShip = Instantiate(_go, _sp, _spr);
        Entity newShipEntity = newShip.GetComponent<Entity>();
        newShipEntity.SetTeam(Team.RED);
        newShipEntity.onDeath += DecreaseWaveEnemyCount;

        return;
    }   




    public void DecreaseWaveEnemyCount () {
        waves[nextWave].numOfEnemies -= 1;
        if (waves[nextWave].numOfEnemies <= 0) {
            WaveCompleted();
        }
    }




}
