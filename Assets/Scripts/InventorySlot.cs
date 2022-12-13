using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public GameObject itemPrefab;     //the item that this slot contains

    public void AddItem(GameObject newItem){
        itemPrefab = newItem;
        //get sprite renderer from children
        GameObject graphics = itemPrefab.transform.Find("Graphics").gameObject;
        if (graphics == null){
            Debug.LogError("InventorySlot: can't get graphics from child. Check your child component");
            return;
        }
        SpriteRenderer _sp = graphics.GetComponent<SpriteRenderer>();
        if (_sp == null){
            Debug.LogError("InventorySlot: Sprite Renderer not found");
            return;
        }
        icon.sprite = _sp.sprite;
        icon.enabled = true;
        icon.preserveAspect = true;
    }

    public void ClearItem() {
        Destroy(this.gameObject);
    }

    public void DisplayInventoryItem(){
        //If this function is called in the shop set the corresponding gameObjects in the shop
        if (Shop.instance.enabled){
            Shop.instance.DisplayInventoryItem(itemPrefab);
        }

        //else set the corresponding stuff in the inventory UI
        InventoryUI.instance.MarkSlot(this);
    }
}
