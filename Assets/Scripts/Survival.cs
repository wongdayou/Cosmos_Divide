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
            p.GetComponentInParent<Player>().whenPlayerDies += EndGame;
        }

        GameData test = DataManager.LoadFile();
        Debug.Log("Loaded survival high score list: ");
        string testprint = "";
        for (int i = 0; i < test.survivalScore.Count; i ++){
            testprint += test.survivalScore[i].ToString();
            testprint += " ";
        }
        Debug.Log(testprint);

    }

    void Update(){
        if (Input.GetKeyDown("l")){
            DataManager.DeleteSavedData();
        }

        if (Input.GetKeyDown("[")){
            Debug.Log("Increasing Score by 50");
            IncreaseScore(50);
        }

        if (Input.GetKeyDown("]")){
            Debug.Log("Decreasing Score by 50");
            DecreaseScore(50);
        }
    }




    public void IncreaseScore(int amt){
        score += amt;
        scoreText.text = "Score: " + score.ToString();
    }

    public void DecreaseScore(int amt){
        score -= amt;
        scoreText.text = "Score: " + score.ToString();
    }


    public void EndGame(){
        // record high score and safe to file
        Debug.Log("Inside Survival.cs: Ending game");
        GameData savedData = DataManager.LoadFile();
        savedData.AddSurvivalScore(score);
        DataManager.SaveFile(savedData);
        // load all the high scores
        // show high scores    
        if (highScoreTable == null){
            Debug.LogError("Survival.cs: HighScoreTable is null!");
            return;
        }
        highScoreTable.SetActive(true);
        HighScoreTable temp = highScoreTable.GetComponent<HighScoreTable>();
        temp.SetScoreText(score);
        temp.SetHighScores(savedData.survivalScore);
        gameElements.SetActive(false);
    }

}
