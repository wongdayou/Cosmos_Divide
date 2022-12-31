using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperClasses;

public class Entity : MonoBehaviour
{
    public EntityData data;

    public bool invincible = false;
    public Team team = Team.BLUE;

    [SerializeField]
    protected int currentHealth;
    public int resaleValue = 0;


    public delegate void DeathHandler();
    public event DeathHandler onDeath;

    protected bool dying = false;

    protected virtual void Start() {
        if (data != null){
            currentHealth = data.maxHealth;
            resaleValue = (int)(data.cost * 0.5f);
        }
        else{
            Debug.LogError("No Data detected on entity: " + this.gameObject.name);
        }
        
        Transform minimapIcon = transform.Find("MinimapIcon");
        if (minimapIcon != null){
            minimapIcon.gameObject.SetActive(true);
        }

        WeaponManager wpm = this.gameObject.GetComponentInChildren<WeaponManager>();
        if (wpm != null) {
            wpm.SetTeam(team);
        }

        // set this entity and all its child gameobjects that contain a collider to the layer of its team
        Transform graphics;
        string teamName;
        if (team == Team.BLUE){
            teamName = "Blue";
        }
        else {
            teamName = "Red";
        }
        this.gameObject.layer = LayerMask.NameToLayer(teamName);
        graphics = transform.Find("Graphics");
        if (graphics != null){
            graphics.gameObject.layer = LayerMask.NameToLayer(teamName);
        }
        // this.gameObject.tag = teamName;

        LevelManager.instance.RecordShip(team, this.gameObject);
    }


    public virtual void Damage(int damage)
    {
        //Debug.Log("Damaging");
        if (invincible) return;
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Die();
        }
    }

    public virtual void Heal(int healAmount)
    {
        //Debug.Log("In Entity.cs: healing entity");
        currentHealth = (currentHealth + healAmount > data.maxHealth) ? data.maxHealth : currentHealth + healAmount;
    }

    protected virtual void Die() {
        //TODO instantiate explosion particles
        //Debug.Log("Dying");
        if (!dying){
            dying = true;
            Instantiate(data.deathExplosion, transform.position, transform.rotation);
            AudioManager.instance.Play(data.deathExplosionSound);
            LevelManager.instance.ReduceLoad(team, data.load);
            LevelManager.instance.PopShip(team, this.gameObject);

            if (onDeath != null){
                //TODO make it so that the enemy the enemy only looks for a player object after the current player object has been destroyed,
                //else there will be errors
                onDeath();
            }

            Destroy(gameObject);
        }
        
        
    }

    public virtual void SetTeam(Team team) {

        switch (team) {
            case Team.BLUE: 
                this.gameObject.layer = LayerMask.NameToLayer("Blue");
                break;
            case Team.RED:
                this.gameObject.layer = LayerMask.NameToLayer("Red");
                break;
        }
        return;
    }


    // function for ship collision. Can remove if no longer want ship collision anymore
    // void OnTriggerEnter2D(Collider2D hitInfo) {
    //     Entity en = hitInfo.GetComponent<Entity>();
    //     if (en != null){
    //         if (en.maxHealth > this.maxHealth){
    //             //Debug.Log("en health greater");
    //             this.Damage(this.maxHealth);
    //             en.Damage(this.collideDamage);
    //         }
    //         else if (en.maxHealth == this.maxHealth){
    //             //Debug.Log("both health same");
    //             this.Damage(maxHealth);
    //         }
            
    //     }
    // }

    protected void RaiseOnDeathEvent(){
        if (onDeath != null){
            onDeath();
        }
    }

    public int GetMaxHealth(){
        return data.maxHealth;
    }

    // a simple function to update resale value based on the percentage of health left
    public void UpdateResaleValue(){
        float healthPercent = currentHealth/data.maxHealth;
        if (data.cost > 0) {
            resaleValue = (int)(0.5f * data.cost * healthPercent);
        }
        return;
    }

    public Vector3 SizeOfEntity(){
        return GetComponentInChildren<SpriteRenderer>().bounds.size;
    }

}
