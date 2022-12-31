using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperClasses;

public class Player : Entity 
{
    public HealthBar healthBar;
    Rigidbody2D rb;
    public delegate void PlayerDeathManager();
    public event PlayerDeathManager whenPlayerDies;

    public int cashAmount = 0;



    protected override void Start() {
        base.Start();
        this.gameObject.tag = "Player";


        if (healthBar == null){
            Debug.LogError("No healthbar detected on player");
        }
        else{

            healthBar.SetMaxHealth(this.data.maxHealth);
            if (this.currentHealth != this.data.maxHealth){
                healthBar.SetHealth(this.currentHealth);
            }
        }
        

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


        rb = GetComponent<Rigidbody2D>();
        if (rb == null) {
            Debug.LogError("Player does not have a rigidbody!");
        }

    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("w"))
        {
            rb.AddForce(transform.up * data.speed);
        }

        if (Input.GetKey("a"))
        {
            this.transform.Rotate(Vector3.forward * data.rotationSpeed * Time.fixedDeltaTime);
        }

        if (Input.GetKey("s"))
        {
            rb.AddForce(-transform.up * data.speed);
        }

        if (Input.GetKey("d"))
        {
            this.transform.Rotate(Vector3.back * data.rotationSpeed * Time.fixedDeltaTime);
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





}