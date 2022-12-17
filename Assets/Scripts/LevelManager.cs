using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Slider slider;
    public static LevelManager instance;

    [SerializeField]
    private GameObject missionCompleteUI;



    private void Awake() {
        if (instance == null){
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        // if (missionCompleteUI == null){
        //     Debug.LogError("No mission complete UI in LevelManager.cs");
        // }

        DontDestroyOnLoad(gameObject);    


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



    public void EndLevel(){
        Debug.Log("Level ended");
        missionCompleteUI.SetActive(true);
        GameMaster gameMaster = this.gameObject.GetComponent<GameMaster>();
        if (gameMaster != null) {
            gameMaster.LevelComplete();
        }

    }
}
