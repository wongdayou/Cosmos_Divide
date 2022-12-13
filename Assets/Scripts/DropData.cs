using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropData : MonoBehaviour
{
    [System.Serializable]
    public class DropItem {
        public string name;
        public GameObject itemPrefab;
        public int dropRate = 0;

    }
    public DropItem[] dropItems;

    private int totalWeight = 0;
    public float dropChance = 1.0f;

    private void Start() {
        foreach (DropItem d in dropItems){
            totalWeight += d.dropRate;
        }
    }

    public void Drop(Vector3 position){
        if (!this.enabled){
            // Debug.Log("DropData not enabled, returning...");
            return;
        }
        float dice = Random.Range(0f, 1.0f);
        if (dice > dropChance){
            return;
        }
        if (totalWeight == 0){
            Debug.LogError("DropData: totalWeight is 0 in Drop() !");
            return;
        }

        int ranNum = Random.Range(0, totalWeight);
        foreach (DropItem d in dropItems){
            if (ranNum <= d.dropRate){
                GameObject drop = Instantiate(d.itemPrefab, position, Quaternion.identity);
                //TODO make the drops float around for a while
                //drop.transform.velocity
                return;
            }
            else{
                ranNum -= d.dropRate;
            }
        }
    }
}
