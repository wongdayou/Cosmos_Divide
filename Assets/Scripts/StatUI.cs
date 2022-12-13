using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatUI : MonoBehaviour
{
    public GameObject shipStats;
    public GameObject weaponStats;
    public GameObject carrierStats;
    public GameObject currentStat;

    

    public void SetShipStats(Entity ship) {
        ChangeStatDisplay(shipStats);
        Transform health = shipStats.transform.Find("Health");
        Transform speed = shipStats.transform.Find("Speed");
        Transform rotSpeed = shipStats.transform.Find("RotSpeed");
        Transform numTurrets = shipStats.transform.Find("NumOfTurrets");
        Transform numOfDeploymentPods = shipStats.transform.Find("NumOfDeploymentPods");

        health.Find("StatValue").gameObject.GetComponent<TMP_Text>().text = "" + ship.data.maxHealth;
        speed.Find("StatValue").gameObject.GetComponent<TMP_Text>().text = "" + ship.data.speed;
        rotSpeed.Find("StatValue").gameObject.GetComponent<TMP_Text>().text = "" + ship.data.rotationSpeed;
        numTurrets.Find("StatValue").gameObject.GetComponent<TMP_Text>().text = "" + ship.data.numOfTurrets;
        numOfDeploymentPods.Find("StatValue").gameObject.GetComponent<TMP_Text>().text = "" + ship.data.numOfDeploymentPods;

        return;
    }

    public void SetWeaponStats(weapon wp){
        // Transform bulletTier = shipStats.transform.Find("BulletTier");                               // might wanna consider changing bullet tier to damage instead
        // Transform rotSpeed = shipStats.transform.Find("RotSpeed");
        // Transform barrels = shipStats.transform.Find("NumOfBarrels");
        // Transform fireRate = shipStats.transform.Find("FireRate");
        // Transform range = shipStats.transform.Find("Range");

        // //TODO check if the weapon is a rotary cannon. If yes, get rot speed

        // // bulletTier.Find("StatValue").gameObject.GetComponent<TMP_Text>().text = "" + wp.bulletTier;     // might wanna consider changing bullettier to damage instead
        // // rotSpeed.Find("StatValue").gameObject.GetComponent<TMP_Text>().text = "" + ship.speed;
        // barrels.Find("StatValue").gameObject.GetComponent<TMP_Text>().text = "" + wp.numOfBarrels;
        // fireRate.Find("StatValue").gameObject.GetComponent<TMP_Text>().text = "" + wp.fireRate;
        // range.Find("StatValue").gameObject.GetComponent<TMP_Text>().text = "" + wp.firingRange;
    }

    void ChangeStatDisplay(GameObject display){
        if (currentStat != null) {
            currentStat.SetActive(false);
        }
        display.SetActive(true);
        currentStat = display;
    }

    void ClearDisplay(){
        if (currentStat != null) {
            currentStat.SetActive(false);
        }
    }
}
