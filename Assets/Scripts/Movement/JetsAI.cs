using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetsAI : ShipMovementAI
{
    //for jets they only have fixed cannons
    public FixedCannonAI fc;

    protected override void Start(){
        fc = GetComponentInChildren<FixedCannonAI>();
        if (fc == null){
            Debug.LogError(this.gameObject.name + " has no fixedcannon attached");
        }
        base.Start();        
    }

    protected override void SetTarget(GameObject result){
        base.SetTarget(result);
        if (fc != null){
            // Debug.Log("Setting fc target " + result.transform);
            fc.SetTarget(result.transform);
        }        
        return;
    }

    protected override void OnTargetDeath() {
        base.OnTargetDeath();
        if (fc != null){
            fc.ClearTarget();
        }
        return;
    }
}
