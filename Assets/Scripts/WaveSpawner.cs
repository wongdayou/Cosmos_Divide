using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperClasses;

public class WaveSpawner : MonoBehaviour
{
    public enum WaveState { SPAWNING, WAITING, COUNTDOWN };

    public GameMaster gm;
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
    }

    public Wave[] waves;
    private WaveState waveState = WaveState.COUNTDOWN;
    public SpawnPoint[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        waveCountdown = countdownTime;
        if (spawnPoints.Length <= 0){
            Debug.LogError("WaveSpawner: no spawn points!");
        }
        gm = FindObjectOfType<GameMaster>();
        if (gm == null){
            Debug.Log("WaveSpawner: GameMaster not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (waveState) {
            case WaveState.SPAWNING: return;
            case WaveState.WAITING: 
                if (searchCountdown <= 0f) {
                    if (GameObject.FindGameObjectWithTag("Enemy") == null){
                        WaveCompleted();
                    }
                    searchCountdown = searchRate;
                }
                else {
                    searchCountdown -= Time.deltaTime;
                }
                break;

            case WaveState.COUNTDOWN: 
                if (waveCountdown <= 0) {
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
                    
                else {
                    waveCountdown -= Time.deltaTime;
                }
                break;
        }
    }

    void WaveCompleted() {
        waveState = WaveState.COUNTDOWN;
        waveCountdown = countdownTime;
        if (nextWave + 1 > waves.Length - 1) {
            nextWave = 0;
            Debug.Log("All waves cleared");
        }
        else {
            nextWave ++;
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
                yield return new WaitForSeconds( 1f/_waveEnemy.spawnRate );
            }
        }

        int i = 0;
        while (i < _waveEnemy.count){

            if (gm.CanSpawn(Team.RED, _waveEnemy.enemy.GetComponent<Entity>().data.load)){
                CreateGameObject(_waveEnemy.enemy);
                i++;
            }
            yield return new WaitForSeconds( 1f/_waveEnemy.spawnRate );
            
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
        newShip.GetComponent<Entity>().SetTeam(Team.RED);
        return;
}
}
