using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperClasses;

public class Player : Entity 
{
    public HealthBar healthBar;

    public delegate void PlayerDeathManager();
    public event PlayerDeathManager whenPlayerDies;

    public int cashAmount = 0;
    protected override void Start() {
        base.Start();
        this.gameObject.tag = "Player";
        healthBar.SetMaxHealth(this.data.maxHealth);
        if (this.currentHealth != this.data.maxHealth){
            healthBar.SetHealth(this.currentHealth);
        }
        GameMaster.gm.onShopToggle += OnShopToggle;
        string teamName;
        if (team == Team.BLUE){
            teamName = "Blue";
        }
        else {
            teamName = "Red";
        }
        Transform ship = transform.Find("Ship");
        if (ship != null){
            ship.gameObject.layer = LayerMask.NameToLayer(teamName);
            ship.gameObject.tag = teamName;
            Transform graphics = ship.Find("Graphics");
            if (graphics != null){
                graphics.gameObject.layer = LayerMask.NameToLayer(teamName);
            }
        }

    }

    public override void Damage(int damage)
    {
        base.Damage(damage);
        healthBar.SetHealth(this.currentHealth);
    }

    public override void Heal(int healAmount)
    {
        //Debug.Log("Healing player");
        base.Heal(healAmount);
        healthBar.SetHealth(this.currentHealth);
        //Debug.Log("Healthbar set");
    }

    protected override void Die(){
        if (whenPlayerDies != null){
            whenPlayerDies();
        }
        base.Die();
    }

    public void IncreaseCash (int amount){
        cashAmount += amount;
        PlayerUIManager.instance.UpdateCash(cashAmount);
        return;
    }

    public void DecreaseCash(int amount){
        cashAmount = Mathf.Min(0, cashAmount - amount);
        PlayerUIManager.instance.UpdateCash(cashAmount);
        return;
    }

    void OnShopToggle(bool active) {
        // when shop toggles disable all controls and movement
        PlayerMovement pm = GetComponent<PlayerMovement>();
        WeaponController wc = GetComponent<WeaponController>();
        if (pm == null || wc == null) {
            Debug.LogError("Player.cs.OnShopToggle(): PlayerMovement/WeaponController is null");
            return;
        }

        pm.enabled = !active;
        wc.enabled = !active;
    }
}