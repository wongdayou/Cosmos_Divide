using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperClasses;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;
    public int shipLimit = 50;
    public int numShips = 0; 
    public int loadLimit = 50;
    // private bool spawning = false;
    // private bool mutex = false;
    public int blueLoad = 0;
    public int redLoad = 0;
    public KeyCode shopButton = KeyCode.B;
    private bool gameIsPaused = false;

    [SerializeField]
    private GameObject shop;

    [SerializeField]
    private GameObject gameUI;

    public delegate void ShopToggle(bool active);
    public ShopToggle onShopToggle;

    // list of ships on each team
    public List<GameObject> redTeamShips = new List<GameObject>();
    public List<GameObject> blueTeamShips = new List<GameObject>();

    private void Awake() {
        if (gm == null){
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }

    private void Start() {
        if (shop.activeSelf){
            ToggleShop();
        }
        if (!gameIsPaused && !gameUI.activeSelf)
        {
            ToggleGameUI(true);
        }
    }

    void Update(){
        if (Input.GetKeyDown(shopButton)){
            if (shop.activeSelf)
            {
                ToggleShop();
                ResumeGame();
                ToggleGameUI(!shop.activeSelf);
            }
            else{   
                ToggleShop();
                PauseGame();
                ToggleGameUI(!shop.activeSelf);
            }
            
        }
    }

    void PauseGame(){
        Time.timeScale = 0f;
        gameIsPaused = true;
        Debug.Log("Paused");
    }

    void ResumeGame() {
        Time.timeScale = 1f;
        gameIsPaused = false;
        Debug.Log("Resumed");
    }

    void ToggleGameUI(bool active){
        if (gameUI != null){
            gameUI.SetActive(active);
        }
    }

    void ToggleShop() {
        if (shop != null) {
            shop.SetActive( !shop.activeSelf );
        }
        if (onShopToggle != null){
            onShopToggle.Invoke(shop.activeSelf);
        } 
    }

    //version of function to restrict spawn based on load
    public bool CanSpawn(Team t, int load = 10) {
        switch (t) {
            case Team.RED: 
                if (redLoad + load > loadLimit) {
                    // Debug.Log("Too much load");
                    return false;
                }
                else {
                    redLoad += load;
                    // Debug.Log("Load acceptable");
                    // Debug.Log(redLoad);
                    return true;
                }
                
            case Team.BLUE:
                if (blueLoad + load > loadLimit) {
                    // Debug.Log("Too much load");
                    return false;
                }
                else{
                    blueLoad += load;
                    // Debug.Log("Blue Load acceptable");
                    // Debug.Log(blueLoad);
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
