using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void LoadLevel(string name){
        GameMaster.gm.LoadLevel(name);
    }


    public void LoadMenu(string name){
        SceneManager.LoadSceneAsync(name);
    }

    public void QuitGame(){
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
