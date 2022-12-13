using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [System.Serializable]
    public class WeaponControl {
        public weapon wp;
        public bool underControl = false;
    }
    [SerializeField]
    public List<WeaponControl> weapons = new List<WeaponControl>();
    bool isShooting = false;
    void Start()
    {
        string weaponLayer = null;
        string identifier = null;
        if (this.gameObject.layer == LayerMask.NameToLayer("Blue Team")) {
            weaponLayer = "Blue Team Weapon";
            identifier = "Blue Team Bullet";
        }
        else if (this.gameObject.layer == LayerMask.NameToLayer("Red Team")){
            weaponLayer = "Red Team Weapon";
            identifier = "Red Team Bullet";
        }
        
    }

    void Update()
    {
        //shooting the weapon
        if (Input.GetButton("Fire1"))
        {
            if (weapons.Count > 0 && !isShooting)
            {
                isShooting = true;
                
                foreach (WeaponControl wpc in weapons){
                    if (wpc.underControl){
                        // Debug.Log("Firing");
                        wpc.wp.TriggerWeaponOn();
                    }
                }
            }

        }
        if (Input.GetButtonUp("Fire1")){
            if (weapons.Count > 0)
            {
                isShooting = false;
                foreach (WeaponControl wpc in weapons){
                    if (wpc.underControl){
                        wpc.wp.TriggerWeaponOff();
                    }
                }
            }
        }
    }

    public void AddWeapon(GameObject newWpPrefab){
        // Add actual gameObject as a child for the object with weapon manager
        WeaponManager wpm = this.gameObject.GetComponentInChildren<WeaponManager>();
        if (wpm != null){
            Transform wpmT = wpm.transform;
        }
        // Instantiate(newWpPrefab, )
    }
}
