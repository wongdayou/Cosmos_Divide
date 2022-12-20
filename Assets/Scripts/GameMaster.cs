/*

    GameMaster serves to manage overall game matters.
    The following are its functions:
        1. Load levels and manage the loading bar
        2. Handle user inputs regarding overall game functions e.g. pausing, settings





*/








using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HelperClasses;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm; 
    
    
    public KeyCode pauseButton = KeyCode.Escape;
    private bool gameIsPaused = false;
    private bool levelCompleted = false;

    

    [SerializeField]
    private GameObject gameUI;

    

    // to manage the loading screen loading progress bar
    public Slider slider;



    private void Awake() {
        if (gm == null){
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }



    private void Start() {
        
        if (!gameIsPaused && !gameUI.activeSelf)
        {
            ToggleGameUI(true);
        }
    }



    void Update(){
        

        if (Input.GetKeyDown(pauseButton)){

            if (levelCompleted){
                LoadLevel("MainMenu");
            }

            else{
                Debug.Log("Pausing Game");
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




    public void LevelComplete(){
        levelCompleted = true;
    }



    public void LoadLevel(string name){
        //TODO get the slider
        StartCoroutine(LoadLevelAsync(name));
    }



    IEnumerator LoadLevelAsync (string name) {
        
        AsyncOperation loading = SceneManager.LoadSceneAsync("Loading");

        while (!loading.isDone){
            yield return null;
        }

        GameObject sliderGameObject = GameObject.FindWithTag("LoadingBar");
        if (sliderGameObject == null){
            Debug.LogError("LevelManager: LoadLevel() sliderGameObject not found");
            yield return false;
        }
        slider = sliderGameObject.GetComponent<Slider>();
        if (slider == null){
            Debug.LogError("LevelManager: LoadLevel() slider not found");
            yield return false;
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(name);
        
        while (!op.isDone){
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
    }



}
