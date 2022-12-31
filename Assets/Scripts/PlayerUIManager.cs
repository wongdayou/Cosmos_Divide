using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    
    public TMP_Text cashText;
    
    public static PlayerUIManager instance;

    void Start() {
 
        if (cashText == null){
            Debug.Log("PlayerUIManager: cashText is null!");
        }
        

        if (instance == null){
            instance = this;
        }
        
        
    }


    public void UpdateCash(int amt){
        cashText.text = "Cash: $" + amt;
    }

    
    
}
