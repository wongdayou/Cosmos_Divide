using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HighScoreTable : MonoBehaviour
{
    public TMP_Text scoreText;
    //TODO when activated lists all the high scores in a table
    public void Start(){
        if (scoreText == null){
            Debug.Log("High Score Table: scoreText is null!");
        }

    }

    public void SetScoreText(int score){
        scoreText.text = "" + score;
    }

    public void RestartSurvival(){
        GameMaster.gm.LoadLevel("Survival");
    }

    public void ReturnToMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }
}
