using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperClasses;

public class CarrierSpawner : MonoBehaviour
{
    
    [System.Serializable]
    public class SpawnPoint {
        public Transform spawnPoint;
        public bool isSpawning = false;
        public float readyInterval = 0f;
        public float timeToReady = 0f;
    }

    [System.Serializable]
    public class SpawnInfo {
        public GameObject spawnPrefab;
        public float spawnInterval;
        public float timeToSpawn = 0f;
        public float timeToStartSpawning = 0f;
        public bool isSpawning = false;
    }

    public SpawnInfo[] spawnInfos;
    public SpawnPoint[] spawnPoints;
    public GameMaster gm;

    int numOfSpawnPoints;

    public Team team = Team.BLUE;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnPoints.Length == 0){
            Debug.LogError("CarrierSpawner: no spawnpoints added");
        }
        if (spawnInfos.Length == 0){
            Debug.LogError("CarrierSpawner: no spawnInfos added");
        }

        foreach (SpawnInfo _si in spawnInfos){
            _si.timeToSpawn += _si.timeToStartSpawning;
        }

        gm = FindObjectOfType<GameMaster>();
        if (gm == null){
            Debug.Log("WaveSpawner: GameMaster not found!");
        }

        numOfSpawnPoints = spawnPoints.Length;

        // check the team of the ship this script is attached to
        Entity _en = this.gameObject.GetComponent<Entity>();
        if (_en != null){
            team = _en.team;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnInfos.Length > 0){
            foreach (SpawnInfo _si in spawnInfos){

                if (Time.time > _si.timeToSpawn && !_si.isSpawning) {
                    Spawn(_si);       
                }
            }
        }
        
    }

    public void Spawn (SpawnInfo _si) {
        if (spawnPoints.Length == 0){
            Debug.LogError("CarrierSpawner: There are no spawnPoints!");
            return;
        }
        int spIndex = Random.Range(0, numOfSpawnPoints);

        //if the spawnpoint is spawning something now or it is not ready yet
        if (spawnPoints[spIndex].isSpawning || (Time.time <= spawnPoints[spIndex].timeToReady) || !LevelManager.instance.CanSpawn(team, _si.spawnPrefab.GetComponent<Entity>().data.load)){
            return;
        }    
        else {
            _si.isSpawning = true;
            spawnPoints[spIndex].isSpawning = true;
            _si.timeToSpawn += _si.spawnInterval;
            spawnPoints[spIndex].timeToReady = Time.time + spawnPoints[spIndex].readyInterval;
            // Debug.Log("Spawning: " + _si.spawnPrefab.name + " at spIndex: " + spIndex);
            GameObject _new = Instantiate(_si.spawnPrefab, spawnPoints[spIndex].spawnPoint.position, spawnPoints[spIndex].spawnPoint.rotation);
            _new.GetComponent<Entity>().SetTeam(team);
            _si.isSpawning = false;
            spawnPoints[spIndex].isSpawning = false;
            
        }
    }
}
