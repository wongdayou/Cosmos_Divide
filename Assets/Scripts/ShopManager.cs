using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    public KeyCode shopButton = KeyCode.B;
    [SerializeField]
    private GameObject shop;

    public delegate void ShopToggle(bool active);
    public ShopToggle onShopToggle;

    void Awake(){
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {


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
                GameMaster.gm.ResumeGame();
            }
            else{   
                ToggleShop();
                GameMaster.gm.PauseGame();
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
