using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float bulletSpeed = 10000f;
    public float lifeSpan = 1f;
    public GameObject impactEffect;
    [SerializeField]
    protected int damage = 40;

    protected Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.Log("Rigidbody not found for bullet");
        }
        
    }

    protected virtual void Start()
    {
        rb.velocity = transform.up * bulletSpeed;
        Destroy(this.gameObject, lifeSpan);
    }

    protected virtual void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Entity en = hitInfo.GetComponentInParent<Entity>();
        //Debug.Log(hitInfo.name);
        if (en != null) {
            Instantiate(impactEffect, this.transform.position, Quaternion.identity);
            en.Damage(damage);
            Destroy(gameObject);
        }
    }

    public virtual void SetBulletLayer(string identifier){
        this.gameObject.layer = LayerMask.NameToLayer(identifier);
    }
}
