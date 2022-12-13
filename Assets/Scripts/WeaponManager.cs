using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperClasses;

public class WeaponManager : MonoBehaviour
{
    public Team team = Team.BLUE;

    public void SetTeam(Team t){
        
        team = t;
        // TODO find all weapons attached to it and set their tags and layers
        weapon childwp;
        foreach (Transform child in transform){
            childwp = child.GetComponent<weapon>();
            if (childwp != null){
                childwp.SetTeam(t);
            }
        }
    }

    
}
