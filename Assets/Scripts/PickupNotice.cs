using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupNotice : MonoBehaviour
{
    public static PickupNotice pickUpNoticeInstance;

    public void Start() {
        if (pickUpNoticeInstance == null){
            pickUpNoticeInstance = this;
        }
    }

    public float textSpaceOffset = 43f;
    public GameObject noticeLinePrefab;
    // Start is called before the first frame update
    public void ItemPickupNotice(int weight, string itemName){
        //for each current notice line move it upwards to give space for the new notice line
        Vector3 pos;
        foreach (Transform ntl in transform){
            pos = ntl.position;
            pos.y += textSpaceOffset;
            ntl.position = pos;
        }
        GameObject noticeLine = Instantiate(noticeLinePrefab, this.transform.position, this.transform.rotation);
        noticeLine.transform.SetParent(this.transform);
        Transform noticeLineChild = noticeLine.transform.GetChild(0);
        if (noticeLineChild == null) {
            Debug.Log("noticeLineChild missing");
        }
        TMP_Text noticeLineText = noticeLineChild.GetComponent<TMP_Text>();
        if (noticeLineText == null){
            Debug.Log("PickupNotice: noticeLineText is null!");
        }
        noticeLineText.text = "+ " + weight + " " + itemName;
        
        return;
    }
}
