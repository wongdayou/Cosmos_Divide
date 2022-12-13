using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperClasses;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public int maxSpace;
    public int cash;
    public List<GameObject> weapons = new List<GameObject>();
    public List<GameObject> battleships = new List<GameObject>();
    public List<GameObject> carriers = new List<GameObject>();


    void Awake(){
        instance = this;
        //TODO initialise inventory based on final state of inventory in the previous level
    }

    public void AddItem(ItemType type, GameObject item){
        switch (type) {
            case ItemType.WEAPONS:
                weapons.Add(item);
                break;

            case ItemType.SHIPS:
                battleships.Add(item);
                break;

            case ItemType.CARRIERS:
                carriers.Add(item);
                break;
        }
        InventoryUI.instance.AddItemIntoUI(type, item);
        return;
    }

    public void RemoveItem(ItemType type, GameObject item){
        switch (type) {
            case ItemType.WEAPONS:
                weapons.Remove(item);
                break;

            case ItemType.SHIPS:
                battleships.Remove(item);
                break;

            case ItemType.CARRIERS:
                carriers.Remove(item);
                break;
        }
        InventoryUI.instance.RemoveItemFromUI();
        return;
    }

    public void BuyItem(ItemType type, GameObject itemPrefab, int cost){
        // GameObject prefabInstance = Instantiate (itemPrefab);
        AddItem(type, itemPrefab);
        cash -= cost;
        Shop.instance.UpdatePlayerCash(cash);
        return;
    }

    public void SellItem(GameObject o){
        IItem _item = o.GetComponent<IItem>();
        if (_item != null){
            cash += _item.ResaleValue;
            RemoveItem(_item.Type, o);
        }
        Shop.instance.UpdatePlayerCash(cash);
        
    }

    public bool CheckPurchaseEligibility (int itemCost, ItemType itemType) {
        if (itemCost > cash) {
            Debug.Log("Not enough cash to buy");
            return false;
        }
        switch (itemType) {
            case ItemType.WEAPONS:
                if (weapons.Count < maxSpace) {
                    return true;
                }
                break;

            case ItemType.SHIPS:
                if (battleships.Count < maxSpace){
                    return true;
                }
                break;

            case ItemType.CARRIERS:
                if (carriers.Count < maxSpace){
                    return true;
                }
                break;
        }
        Debug.Log("Not enough space!");
        return false;
    }
}
