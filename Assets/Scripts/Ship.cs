using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HelperClasses;

public class Ship : Entity
{
    protected PlayerUIManager pUIm;
    public int defeatScore = 0;
    public DropData dropData;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // gm = FindObjectOfType<GameMaster>();
        // if (gm == null){
        //     Debug.Log("Entity: GameMaster not found!");
        // }
        pUIm = FindObjectOfType<PlayerUIManager>();
        if (pUIm == null){
            Debug.Log("SurvivalEnemy: playerUIManager is null!");
        }
        
    }

    protected override void Die(){
        // gm.MinusShip();
        if (!dying){
            dying = true;
            Instantiate(data.deathExplosion, transform.position, transform.rotation);
            AudioManager.instance.Play(data.deathExplosionSound);
            LevelManager.instance.ReduceLoad(team, data.load);

            RaiseOnDeathEvent();
            if (pUIm != null){
                pUIm.IncreaseScore(defeatScore);
            }
            // if (dropsEnabled){
            //     DropData dropData = GetComponent<DropData>();
            //     if (dropData == null){
            //         Debug.Log("Enemy.cs: Tried to access DropData but cannot be found");
            //     }
            //     else{
            //         dropData.Drop(this.transform.position);
            //     }
                
            // }

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
