using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Transform target;

    //this function sets the rotation of our spawn effect so when we spawn the ship the ship will face towards the target
    public void FindTarget(){
        //if there is not target don't change the rotation of the spawn effect
        if (target == null){
            Debug.Log("SpawnEffect: No target");
            return;
        }
        Vector3 dir = (target.position - this.transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        return;
    }
    
    public void SpawnObject(){
        Instantiate(objectToSpawn, this.transform.position, this.transform.rotation);
        return;
    }

    public void RemoveFromGame(){
        Destroy(gameObject);
    }
}
