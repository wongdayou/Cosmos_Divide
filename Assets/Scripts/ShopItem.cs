using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperClasses;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public GameObject itemPrefab;
    public int cost;
    public ItemType itemType;
    public string description = "This is a shop item";

    // private void Awake() {
    //     // if (cost < 0) {
    //     //     Entity _en = itemPrefab.gameObject.GetComponent<Entity>();
    //     //     if (_en != null){
    //     //         cost = _en.cost;
    //     //     }
    //     // }

    // }

    private void Start() {
        IItem _item = itemPrefab.GetComponent<IItem>();
        if (_item == null){
            Debug.Log("Can't get IItem");
        }
        else {
            cost = _item.Cost;
            description = _item.Description;
            itemType = _item.Type;
        }
    }
}
