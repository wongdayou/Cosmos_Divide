using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    private int score = 0;

    public Text scoreText;
    public TMP_Text cashText;
    public GameObject highScoreTable;
    public GameObject gameElements;

    public static PlayerUIManager instance;

    void Start() {
        if (scoreText == null){
            Debug.Log("PlayerUIManager: scoreText is null!");
        }
        if (cashText == null){
            Debug.Log("PlayerUIManager: cashText is null!");
        }
        if (scoreText != null){
            scoreText.text = "Score: " + score.ToString();
        }

        if (instance == null){
            instance = this;
        }
        if (highScoreTable == null){
            Debug.LogError("Survival.cs: highScoreTable is null!");
        }
        if (gameElements == null){
            Debug.LogError("Survival.cs: gameElements is null!");
        }
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p == null){
            Debug.LogError("PlayerUIManager: player is null");
        }
        else{
            p.GetComponentInParent<Player>().whenPlayerDies += ShowHighScore;
        }
    }

    public void IncreaseScore(int amt){
        score += amt;
        scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateCash(int amt){
        cashText.text = "Cash: $" + amt;
    }

    public void ShowHighScore(){
        if (highScoreTable == null){
            Debug.LogError("PlayerUIManager: HighScoreTable is null!");
            return;
        }
        highScoreTable.SetActive(true);
        highScoreTable.GetComponent<HighScoreTable>().SetScoreText(score);
        gameElements.SetActive(false);
    }

    
}
