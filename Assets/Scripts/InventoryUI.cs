using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperClasses;

public class InventoryUI : MonoBehaviour
{
    public GameObject shipsSection;
    public GameObject weaponsSection;
    public GameObject carriersSection;
    public GameObject currentActiveSection;
    public GameObject InventorySlotPrefab;
    public InventorySlot currentlySelectedSlot;
    public static InventoryUI instance;

    private void Awake() {
        instance = this;
    }

    public void ChangeActiveDisplay(GameObject newSection){
        currentActiveSection.SetActive(false);
        newSection.SetActive(true);
        currentActiveSection = newSection;
    }

    public void RemoveItemFromUI(){
        if (currentlySelectedSlot != null){
            Destroy(currentlySelectedSlot.gameObject);
        }
        
    }

    public void MarkSlot(InventorySlot slot){
        currentlySelectedSlot = slot;
    }

    void OnEnable() {
        currentlySelectedSlot = null;
    }

    public void AddItemIntoUI(ItemType type, GameObject itemPrefab){
        GameObject newSlot = Instantiate(InventorySlotPrefab);

        //maybe add the corresponding item component into the prefab to keep track of the item values
        newSlot.GetComponent<InventorySlot>().AddItem(itemPrefab);
        switch (type){
            case ItemType.WEAPONS:
                newSlot.transform.SetParent(weaponsSection.transform, false);
                break;

            case ItemType.SHIPS:
                newSlot.transform.SetParent(shipsSection.transform, false);
                break;

            case ItemType.CARRIERS:
                newSlot.transform.SetParent(carriersSection.transform, false);
                break;
        }
    }
}
