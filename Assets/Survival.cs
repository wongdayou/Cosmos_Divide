/*
    Survival.cs manages the score, highScoreTable and gameUI in survival mode.
    When the player gets kills, survival.cs records the score
    When the player dies, survival.cs displays the high score table and hides the game UI
    Survival.cs then records the player's high score in the file.    
*/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Survival : MonoBehaviour
{
    private int score = 0;
    public Text scoreText;

    public GameObject highScoreTable;
    public GameObject gameElements;

    public static Survival instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null){
            instance = this;
        }


        if (scoreText == null){
            Debug.Log("Survival.cs: scoreText is null!");
        }
        else{
            scoreText.text = "Score: " + score.ToString();
        }



        if (highScoreTable == null){
            Debug.LogError("Survival.cs: highScoreTable is null!");
        }
        if (gameElements == null){
            Debug.LogError("Survival.cs: gameElements is null!");
        }

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p == null){
            Debug.LogError("Survival.cs: player is null");
        }
        else{
            p.GetComponentInParent<Player>().whenPlayerDies += ShowHighScore;
        }


    }




    public void IncreaseScore(int amt){
        score += amt;
        scoreText.text = "Score: " + score.ToString();
    }


    public void ShowHighScore(){
        if (highScoreTable == null){
            Debug.LogError("Survival.cs: HighScoreTable is null!");
            return;
        }
        highScoreTable.SetActive(true);
        highScoreTable.GetComponent<HighScoreTable>().SetScoreText(score);
        gameElements.SetActive(false);
    }

}
