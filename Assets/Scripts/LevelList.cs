using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelList : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // load saved data
        GameData data = DataManager.LoadFile();
        // from saved data, find the levels the player has completed
        int numberOfLevelsCompleted = data.highestLevelCompleted;
        int temp = 0;
        foreach (Transform child in this.transform){
            Button button = child.gameObject.GetComponent<Button>();
            TMP_Text text = child.transform.GetChild(0).GetComponent<TMP_Text>();
            Color text_color;
            if (text != null){
                text_color = text.color;
            }
            if (temp <= numberOfLevelsCompleted){
                
                if (button != null){
                    button.interactable = true;
                }
                if (text != null){
                    text_color.a = 1f;
                }
            }

            else {
                if (button != null){
                    button.interactable = false;
                }

                if (text != null){
                    text_color.a = 0.5f;
                }
            }
            temp ++;
        
        }
        // if the level has been completed, then the next level button will be enabled
        // else, it will be disabled and it will be slightly tinted

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
