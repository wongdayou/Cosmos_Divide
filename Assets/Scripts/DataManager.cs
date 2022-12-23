// classes and functions to save player data

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Format of data we are saving
// modify accordingly to your needs
[System.Serializable]
public class GameData
{
    public int highestLevelCompleted;
    public int cash;
    public int maxHealth;
    public int currentHealth;

    public List<int> survivalScore;



    public GameData(int highestLevelCompletedInt = 0, int cashInt = 0, int maxHealthInt = 0, int currentHealthInt = 0, List<int> survivalScoreList = null){
        highestLevelCompleted = highestLevelCompletedInt;
        cash = cashInt;
        maxHealth = maxHealthInt;
        currentHealth = currentHealthInt;
        if (survivalScoreList == null){
            survivalScore = new List<int>();
        }
        else {
            survivalScore = survivalScoreList;
        }
        
    }



}






public static class DataManager
{
    // filepath to where your saved data file is
    // modify accordingly to your needs
    static string destination = Application.persistentDataPath + "/save.dat";



    public static void SaveFile(GameData data){
        FileStream file;

        if (File.Exists(destination)){
            file = File.OpenWrite(destination);
        }
        else{
            file = File.Create(destination);
        }

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }


    public static GameData LoadFile()
    {
        Debug.Log("Destination: " + destination);
        GameData data;
        FileStream file;
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(destination)) {
            file = File.OpenRead(destination);
        }
        else{
            CreateDefaultFile();
            file = File.OpenRead(destination);
        }


        try {
            data = (GameData) bf.Deserialize(file);
        }
        catch (Exception e){
            Debug.Log(e);
            Debug.Log("Error encountered here");
            file.Close();
            DeleteSavedData();
            CreateDefaultFile();
            file = File.OpenRead(destination);
            data = (GameData) bf.Deserialize(file);
        }

        
        file.Close();

        return data;
    }



    public static void DeleteSavedData(){
        File.Delete(destination);
        return;
    }



    static void CreateDefaultFile(){
        FileStream file = File.Create(destination);
        GameData data = new GameData();
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

}
