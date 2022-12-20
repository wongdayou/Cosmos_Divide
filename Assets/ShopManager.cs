using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public KeyCode shopButton = KeyCode.B;
    [SerializeField]
    private GameObject shop;

    public GameMaster gm;


    public delegate void ShopToggle(bool active);
    public ShopToggle onShopToggle;

    // Start is called before the first frame update
    void Start()
    {
        if (gm == null){
            Debug.LogError("ShopManager: no gm present");
        }

        if (shop == null){
            Debug.LogError("ShopManager: no shop present");
        }


        // if we accidentally left the shop open when the level starts, turn it off
        if (shop.activeSelf){
            ToggleShop();
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(shopButton)){
            Debug.Log("Shop Button pressed");
            if (shop.activeSelf)
            {
                ToggleShop();
                gm.ResumeGame();
                gm.ToggleGameUI(!shop.activeSelf);
            }
            else{   
                ToggleShop();
                gm.PauseGame();
                gm.ToggleGameUI(!shop.activeSelf);
            }
            
        }
    }


    void ToggleShop() {
        if (shop != null) {
            shop.SetActive( !shop.activeSelf );
        }
        if (onShopToggle != null){
            onShopToggle.Invoke(shop.activeSelf);
        } 
    }
}
