using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public enum ListOfPickups {
        CASH,
        HEALTH
    }

    public ListOfPickups pickupType;
    public float powerupDuration = 0f;
    public int weight;
    public string pickUpSound;
    public float lifeTime = 5f;
    private float timeToExpire;
    public bool permanent = false;

    private void Start() {
        if (!permanent){
            timeToExpire = Time.time + lifeTime;
        }   
        
    }

    private void Update() {
        if (!permanent) {
            if (Time.time >= timeToExpire)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")){
            Debug.Log("its the player!");
            StartCoroutine(Pickup(other));
        }
    }

    IEnumerator Pickup(Collider2D player){
        Debug.Log("picking up");
        Player p = player.GetComponentInParent<Player>();
        switch (pickupType) {
            case ListOfPickups.CASH:
                if (p == null){
                    Debug.Log("PickupItem: cannot get player component");
                    yield return false;
                }
                p.IncreaseCash(this.weight);
                break;

            case ListOfPickups.HEALTH:
                //Debug.Log("Healing");
                p.Heal(this.weight);
                break;

        }
        AudioManager.instance.Play(pickUpSound); 

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        PickupNotice.pickUpNoticeInstance.ItemPickupNotice(this.weight, this.pickupType.ToString());

        if (powerupDuration != 0f) {
            yield return new WaitForSeconds(powerupDuration);
            //for special powerups that have a fixed duration

        }

        Destroy(gameObject);
    }
}
