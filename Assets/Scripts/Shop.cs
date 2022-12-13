using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HelperClasses;

public class Shop : MonoBehaviour
{
    public GameObject battleshipSection;
    public GameObject deploymentSection;
    [SerializeField]
    private GameObject curItemType;
    [SerializeField]
    private GameObject curDeploymentShipType;

    #region UI elements
    public TMP_Text amount;
    public GameObject costHeader;
    public GameObject resaleHeader;
    public TMP_Text description;
    public TMP_Text playerCash;
    public TMP_Text buyButtonText;
    public TMP_Text sellButtonText;

    public StatUI statUI;

    public Image shipCloseUp;
    #endregion

    public static Shop instance;
    private ShopItem currentlySelectedItem;
    private GameObject currentSelectedInventoryItem;

    private void Awake() {
        instance = this;
    }

    private string activeSection = "battleship";
    // Start is called before the first frame update
    void Start()
    {
        deploymentSection.SetActive(false);
        battleshipSection.SetActive(true);
        if (battleshipSection == null){
            battleshipSection = this.transform.Find("BattleshipItemsDisplay").gameObject;
            if (battleshipSection == null){
                Debug.LogError("battleshipSection is null");
            }
            
        }

        if (deploymentSection == null){
            deploymentSection = this.transform.Find("DeploymentDisplay").gameObject;
            if (deploymentSection == null){
                Debug.LogError("battleshipSection is null");
            }
            
        }
    }

    private void OnEnable() {
        //reset the selected item everytime the shop is opened
        currentlySelectedItem = null;
    }

    public void ShowDeploymentSection(){
        // Debug.Log("Showing Deployment section");
        if (activeSection == "battleship"){
            activeSection = "deployment";
            battleshipSection.SetActive(false);
            deploymentSection.SetActive(true);
        }

    }

    public void ShowBattleshipSection(){
        // Debug.Log("Showing Battleship Section");
        if (activeSection == "deployment"){
            activeSection = "battleship";
            deploymentSection.SetActive(false);
            battleshipSection.SetActive(true);
        }
    }

    public void ChangeItemType (GameObject typeSection){
        curItemType.SetActive(false);
        typeSection.SetActive(true);
        curItemType = typeSection;
    }

    public void ChangeDeploymentShipType (GameObject itemType){
        curDeploymentShipType.SetActive(false);
        itemType.SetActive(true);
        curDeploymentShipType = itemType;
    }

    public void DisplayShopItemInfo (ShopItem shopItem) {
        currentlySelectedItem = shopItem;
        resaleHeader.SetActive(false);
        costHeader.SetActive(true);
        if (description != null) {
            // Debug.Log("setting description");
            description.text = shopItem.description;
        }

        if (amount != null){
            amount.text = "" + shopItem.cost;
        }
        if (buyButtonText != null){
            buyButtonText.text = "-$" + shopItem.cost;
        }

        ItemType shopItemType = shopItem.itemType;
        switch (shopItemType) {
            case ItemType.WEAPONS:
                DisplayWeaponInfo(shopItem.itemPrefab);
                break;

            case ItemType.SHIPS:
                DisplayShipInfo(shopItem.itemPrefab);
                break;

            case ItemType.CARRIERS:

                break;
        }
    }

    private void DisplayShipInfo (GameObject ship) {
        Entity _en = ship.GetComponent<Entity>();
        if (_en == null){
            Debug.LogError("Shop.cs: the selected prefab has no entity attached to it");
            return;
        }
        else {
            // Debug.Log("Setting UI");
            statUI.SetShipStats(_en);
        }
        
        if (shipCloseUp != null && shipCloseUp.IsActive()){
            Sprite prefabImage = _en.GetComponentInChildren<SpriteRenderer>().sprite;
            if (prefabImage != null){
                shipCloseUp.overrideSprite = prefabImage;
                Color _cl = shipCloseUp.color;
                if (_cl == null){
                    Debug.Log("No color");
                }
                _cl.a = 255;
                shipCloseUp.color = _cl;
            }
            else {
                Debug.LogError("Shop: Prefab has no sprites");
            }
            
        }
        
        return;
    }

    private void DisplayWeaponInfo(GameObject item) {
        weapon _wp = item.GetComponent<weapon>();
        if (_wp != null){
            statUI.SetWeaponStats(_wp);
        }
        return;
    }

    public void DisplayInventoryItem(GameObject itemPrefab){
        currentSelectedInventoryItem = itemPrefab;
        IItem item = itemPrefab.GetComponent<IItem>();
        try {
            Debug.Log(item.Cost);
            Debug.Log(item.ResaleValue);
            if (item.ResaleValue == -1){
                Debug.Log(item.Cost);
                item.ResaleValue = (int)(item.Cost / 2);
                Debug.Log(item.ResaleValue);
            }
            if (amount != null) amount.text = "" + item.ResaleValue;
            if (sellButtonText != null) sellButtonText.text = "+$" + item.ResaleValue;
             
            costHeader.SetActive(false);
            resaleHeader.SetActive(true);
            description.text = item.Description;
            switch (item.Type){
                case ItemType.WEAPONS:
                    DisplayWeaponInfo(itemPrefab);
                    break;

                case ItemType.SHIPS:
                    DisplayShipInfo(itemPrefab);
                    break;
            }
        }
        catch (Exception e) {
            Debug.LogException(e, this);
        }
    }

    public void BuyItem(){
        //TODO buy the currently selected item
        if (currentlySelectedItem != null) {
            // check if inventory has enough cash
            if (Inventory.instance.CheckPurchaseEligibility(currentlySelectedItem.cost, currentlySelectedItem.itemType))
            // if there is enough cash add item into inventory and deduct amount from inventory
            {
                Inventory.instance.BuyItem(currentlySelectedItem.itemType, currentlySelectedItem.itemPrefab, currentlySelectedItem.cost);

            }
        }
    }

    public void SellItem() {
        if (currentSelectedInventoryItem == null) {
            Debug.Log("There is no item selected to sell!");
            return;
        }

        Inventory.instance.SellItem(currentSelectedInventoryItem);
        currentSelectedInventoryItem = null;
    }

    public void UpdatePlayerCash(int amt){
        if (playerCash != null){
            playerCash.text = "$" + amt;
        }
        
    }
}
