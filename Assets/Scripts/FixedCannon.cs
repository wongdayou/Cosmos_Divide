using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCannon : weapon
{
    protected RaycastHit2D hit;
    protected override void Update(){
        base.Update();
        
        foreach (Transform tr in firePoints) {
            // note to self, layermask filter in Physics2D.Raycast DOES NOT use layer index given by LayerMask.NameToLayer
            hit = Physics2D.Raycast(tr.position, tr.up, data.detectRange, LayerMask.GetMask(enemyTeam));

            if (hit.collider != null && !isShooting){
                TriggerWeaponOn();
            }
            else if (hit.collider == null){
                TriggerWeaponOff();
            }
        }
            
    }
}
