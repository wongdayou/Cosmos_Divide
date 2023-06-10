using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HighScoreTable : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text highScoreList;

    //TODO when activated lists all the high scores in a table
    public void Start(){
        if (scoreText == null){
            Debug.Log("High Score Table: scoreText is null!");
        }

        

    }

    public void SetScoreText(int score){
        scoreText.text = "" + score;
    }

    public void SetHighScores(List<int> highScores){
        highScores.Sort();
        for (int i = highScores.Count - 1; i >= 0; i --){
            highScoreList.text = highScoreList.text + "\n" + highScores[i];
        }
    }

    public void RestartSurvival(){
        GameMaster.gm.LoadLevel("Survival");
    }

    public void ReturnToMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }
}
