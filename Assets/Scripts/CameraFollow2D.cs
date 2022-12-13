using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;
    Vector3 offset = new Vector3(0, 0, -10);
    public float xRestriction = 8;
    public float yRestriction = 5;
    public float xBorder = 150;
    public float yBorder = 150;
    public bool searchingTarget = false;
    public float searchDelay = 3f;


    // Update is called once per frame
    void Update()
    {
        if (target == null){
            if (!searchingTarget){
                searchingTarget = true;
            StartCoroutine(SearchForTarget());
            }            
        }
        else{
            Vector3 newPos = target.position;
            newPos.x = Mathf.Clamp(target.position.x, -xBorder + xRestriction, xBorder - xRestriction);
            newPos.y = Mathf.Clamp(target.position.y, -yBorder + yRestriction, yBorder - yRestriction);
            
            this.transform.position = newPos + offset;
        }
        
    }

    IEnumerator SearchForTarget(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null){
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else{
            yield return new WaitForSeconds(searchDelay);
            StartCoroutine(SearchForTarget());
        }
        
        yield return false;
    }
}
