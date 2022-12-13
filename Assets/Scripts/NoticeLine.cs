using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeLine : MonoBehaviour
{
    public void DestroyNoticeLine(){
        Destroy(this.transform.parent.gameObject);
    }
}
