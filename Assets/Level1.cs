using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : MonoBehaviour
{
    public int numOfEnemies = 0;
    public WaveSpawner WaveSpawner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DecreaseNumEnemies(){
        numOfEnemies -= 1;
        if (numOfEnemies <= 0){
            EndLevel();
        }
    }

    void EndLevel(){
        Debug.Log("Level 1 Ended");

    }
}
