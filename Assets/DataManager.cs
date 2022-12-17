// classes and functions to save player data

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Format of data we are saving
// modify accordingly to your needs
[System.Serializable]
public class GameData
{
    public List<int> levelsCompleted;
    public int cash;
    public int maxHealth;
    public int currentHealth;

    public List<int> survivalScore;

    public GameData(List<int> levelsCompletedList, int cashInt, int maxHealthInt, int currentHealthInt, List<int> survivalScoreList){
        levelsCompleted = levelsCompletedList;
        cash = cashInt;
        maxHealth = maxHealthInt;
        currentHealth = currentHealthInt;
        survivalScore = survivalScoreList;
    }
}






public class DataManager
{
    // filepath to where your saved data file is
    // modify accordingly to your needs
    string destination = Application.persistentDataPath + "/save.dat";



    public void SaveFile(GameData data){
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


    public GameData LoadFile()
    {
        FileStream file;
        if (File.Exists(destination)) {
            file = File.OpenRead(destination);
        }
        else{
            Debug.LogError("File not found");
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        GameData data = (GameData) bf.Deserialize(file);
        file.Close();

        return data;
    }


    public void DeleteSavedData(){
        File.Delete(destination);
        return;
    }


}
