using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HelperClasses;

public class Ship : Entity
{
    public int defeatScore = 0;
    public DropData dropData;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        
    }

    protected override void Die(){
        // gm.MinusShip();
        if (!dying){
            dying = true;
            Instantiate(data.deathExplosion, transform.position, transform.rotation);
            AudioManager.instance.Play(data.deathExplosionSound);
            LevelManager.instance.ReduceLoad(team, data.load);

            RaiseOnDeathEvent();


            if (dropData == null){
                Debug.Log("No dropdata. Continuing...");
            }
            else{
                dropData.Drop(this.transform.position);
            }
            Destroy(gameObject);
        }
        
    }

    public override void SetTeam(Team team){
        base.SetTeam(team);
        //TODO find the weapons on the gameObject and set their team and layers
        WeaponManager wpm = this.gameObject.GetComponentInChildren<WeaponManager>();
        if (wpm != null) {
            wpm.SetTeam(team);
        }
    }
}
