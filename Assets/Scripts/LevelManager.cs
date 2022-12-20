/*

    LevelManager serves to manage a game level.
    The following are its functions:
        1. Control the number of ships that can be spawned on each team
        2. Record the ships on both teams that are present at a certain time instance
        3. Enable the mission complete UI when the player finishes the level
        4. Handle user inputs regarding game functions present in a level (e.g. opening the shop)


*/




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using HelperClasses;

public class LevelManager : MonoBehaviour
{
    public int loadLimit = 50;
    public int blueLoad = 0;
    public int redLoad = 0;

    // list of ships on each team
    // forgot what was the purpose for this already, can delete if not necessary
    public List<GameObject> redTeamShips = new List<GameObject>();
    public List<GameObject> blueTeamShips = new List<GameObject>();


    public static LevelManager instance;

    [SerializeField]
    private GameObject missionCompleteUI;





    private void Awake() {
        instance = this;

        // if (missionCompleteUI == null){
        //     Debug.LogError("No mission complete UI in LevelManager.cs");
        // }

        DontDestroyOnLoad(gameObject);    


    }




    public void EndLevel(){
        Debug.Log("Level ended");
        missionCompleteUI.SetActive(true);
        GameMaster gameMaster = this.gameObject.GetComponent<GameMaster>();
        if (gameMaster != null) {
            gameMaster.LevelComplete();
        }

    }


    //version of function to restrict spawn based on load
    public bool CanSpawn(Team t, int load = 10) {
        switch (t) {
            case Team.RED: 
                if (redLoad + load > loadLimit) {
                    return false;
                }
                else {
                    redLoad += load;
                    return true;
                }
                
            case Team.BLUE:
                if (blueLoad + load > loadLimit) {
                    return false;
                }
                else{
                    blueLoad += load;
                    return true;
                }
                
        }
        return false;
    }



    public void ReduceLoad(Team t, int load) {
        switch (t) {
            case Team.RED:
                redLoad -= load;
                break;
            case Team.BLUE:
                blueLoad -= load;
                break;
            default: 
                return;
        }
    }



    public void RecordShip(Team t, GameObject ship){
        switch (t) {
            case Team.RED:
                redTeamShips.Add(ship);
                break;
            case Team.BLUE:
                blueTeamShips.Add(ship);
                break;
        }
        return;
    }



    public void PopShip(Team t, GameObject ship){
        switch (t) {
            case Team.RED:
                redTeamShips.Remove(ship);
                break;
            case Team.BLUE:
                blueTeamShips.Remove(ship);
                break;
        }
        return;
    }
}
