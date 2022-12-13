using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : bullet
{
    public float rotateSpeed = 200f;
    public float blastRadius = 5f;
    public string explosionSound;
    string enemyTeam;
    public Transform target;
    Vector3 targetDirection;
    Quaternion toRotation;
    // Start is called before the first frame update
    protected override void Start()
    {
        Destroy(this.gameObject, lifeSpan);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null){
            //TODO get target location and move missile to track target
            targetDirection = (target.position - this.transform.position).normalized;
            toRotation = Quaternion.LookRotation(Vector3.forward, targetDirection);
            transform.rotation = Quaternion.RotateTowards(this.transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
            rb.AddForce(transform.up * bulletSpeed);
        }
        else {
            rb.AddForce(transform.up * bulletSpeed);
        }
    }

    public void SetTarget(Transform tr){
        target = tr;
        return;
    }

    public void SetRotateSpeed(float rs){
        rotateSpeed = rs;
    }

    protected override void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Entity en = hitInfo.GetComponentInParent<Entity>();
        //Debug.Log(hitInfo.name);
        if (en != null) {
            Explode();     
        }

    }

    public override void SetBulletLayer(string team){
        base.SetBulletLayer(team);
        if (team == "Blue Team"){
            enemyTeam = "Red Team";
        }
        else if (team == "Red Team"){
            enemyTeam = "Blue Team";
        }
    }

    void Explode(){
        Instantiate(impactEffect, this.transform.position, Quaternion.identity);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, blastRadius, LayerMask.GetMask(enemyTeam));
        Entity _en = null;
        foreach (Collider2D hitObject in colliders){
            _en = hitObject.GetComponentInParent<Entity>();
            if (_en != null){
                _en.Damage(damage);
            }
        }
        AudioManager.instance.Play(explosionSound);
        Destroy(gameObject);
    }
}
