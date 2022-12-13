using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperClasses;

public class RotaryCannon : weapon
{
    public enum State { IDLE, ENGAGING }
    State state = State.IDLE;
    TargetPriority targetPriority = TargetPriority.CLOSEST;
    public bool lockTarget = false;
    
    float timeToSearch = 0f;
    float distFromTarget;
    bool isFacingTarget = false;
    Quaternion newIdleRotation;
    float timeToRotate = 0f;
    

    ContactFilter2D targetFilter = new ContactFilter2D();
    List<Collider2D> targetsInRange = new List<Collider2D>();
    int numTargets;


    // Update is called once per frame
    protected override void Update()
    {
        // Check for targets within firing range
        // if there are targets
        // if we are locking in to a target and the target is still within firing range
        // we continue as usual
        // else if the target is not within firing range
        // find the next target based on priority
        // else if we are not locking on to any targets
        // find targets based on priority
        
        if (Time.time > timeToSearch){
            // Debug.Log(targetFilter.useLayerMask);
            // Debug.Log(targetFilter);
            // Debug.Log(targetFilter.layerMask.value);
            numTargets = Physics2D.OverlapCircle(this.transform.position, data.firingRange, targetFilter, targetsInRange);
            timeToSearch = Time.time + Constants.weaponSearchInterval;
            if (numTargets > 0){
                // Debug.Log("Found something");
                SearchTarget();
            }
            else {
                // Debug.Log("Nothing found");
                // timeToRotate = Time.time + idleRotateInterval;
                if (state != State.IDLE){
                    state = State.IDLE;
                }
                
            }
        }
        // Debug.Log(numTargets);
        

        base.Update();
        switch (state) {
            case (State.IDLE):
                //Rotate the cannon randomly around at random intervals
                if (Time.time > timeToRotate) {
                    // Debug.Log("In here");
                    Quaternion random = Random.rotation;
                    newIdleRotation = Quaternion.Euler(0f, 0f, random.eulerAngles.z);
                    // newIdleRotation = Random.rotation;
                    // Debug.Log("Setting new rotation: " + newIdleRotation);
                    timeToRotate = Time.time + Constants.idleRotateInterval;
                }
                if (this.transform.rotation != newIdleRotation){
                    // Debug.Log("Rotating");
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newIdleRotation, data.rotateSpeed * Time.deltaTime);
                }
                 break;

            case (State.ENGAGING):

                TrackTarget();

                // CheckWhetherTargetInRange belongs to "weapon" base class
                if (isFacingTarget) {
                    if (!isShooting) {
                        TriggerWeaponOn();
                    }
                }
                else {
                    if (isShooting) {
                        TriggerWeaponOff();
                    }
                }
                
                break;

        }
    }

    public void TrackTarget(){
        if (target == null) return;
        Vector3 direction = (target.position - this.transform.position).normalized;
        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, data.rotateSpeed * Time.deltaTime);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, data.firingRange, LayerMask.GetMask(enemyTeam));
        // Debug.Log(hit);
        if (hit.collider != null && hit.transform == target){
            if (!isFacingTarget) isFacingTarget = true;
        }
        else {
            if (isFacingTarget) isFacingTarget = false;
        }
        return;
    }

    void RotateIdly () {
        if (Time.time > timeToRotate) {
            newIdleRotation = Random.rotation;
            timeToRotate += Constants.idleRotateInterval;
        }
        if (this.transform.rotation == newIdleRotation){
            return;
        }
        else {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newIdleRotation, data.rotateSpeed * Time.deltaTime);
        }
    }

    void SearchTarget(){
        if (lockTarget) {
            // check whether target still within firing range
            // if yes, no need to do anything and leave target as it is
            // else find another target based on priority
            foreach (Collider2D _target in targetsInRange){
                if (_target.transform == target) {
                    return;
                }
            }
        }
        
        switch (targetPriority) {
            case TargetPriority.CLOSEST:
                // FIND CLOSEST TARGET
                GetClosestTarget();
                break;
            case TargetPriority.STRONGEST:
                // FIND STRONGEST TARGET
                GetStrongestTarget();
                break;
            case TargetPriority.WEAKEST:
                GetWeakestTarget();
                break;
        }
        if (state == State.IDLE){
            state = State.ENGAGING;
        }
        
    }

    void GetClosestTarget() {
        Transform closest = null;
        Transform cannon = this.transform;
        float closestDistance = Mathf.Infinity;
        float newDistance;
        foreach (Collider2D _enemy in targetsInRange) {
            newDistance = Vector2.Distance(cannon.position, _enemy.transform.position);
            if (closest == null || newDistance < closestDistance) {
                closest = _enemy.transform;
                closestDistance = newDistance;
            }
        }
        // target = closest;
        Entity targetEn = null;
        if (closest != null){
            targetEn = closest.gameObject.GetComponentInParent<Entity>();
        }
        
        if (targetEn != null) {
            // Debug.Log("Setting target");
            target = targetEn.gameObject.transform;
            targetEn.onDeath += OnTargetDeath;
        }
        return;
    }

    void GetStrongestTarget(){
        Transform strongest = null;
        Entity _en;
        foreach (Collider2D _enemy in targetsInRange) {
            _en = _enemy.gameObject.GetComponentInParent<Entity>();
            if (_en != null){
                if (strongest == null || _en.GetMaxHealth() > strongest.gameObject.GetComponentInParent<Entity>().GetMaxHealth()) {
                    strongest = _enemy.transform;
                }
            }
        }
        target = strongest;
        Entity targetEn = target.gameObject.GetComponentInParent<Entity>();
        if (targetEn != null) {
            targetEn.onDeath += OnTargetDeath;
        }
        return;
    }

    void GetWeakestTarget(){
        Transform weakest = null;
        Entity _en;
        foreach (Collider2D _enemy in targetsInRange) {
            _en = _enemy.gameObject.GetComponentInParent<Entity>();
            if (_en != null){
                if (weakest == null || _en.GetMaxHealth() < weakest.gameObject.GetComponentInParent<Entity>().GetMaxHealth()) {
                    weakest = _enemy.transform;
                }
            }
        }
        target = weakest;
        Entity targetEn = target.gameObject.GetComponentInParent<Entity>();
        if (targetEn != null) {
            targetEn.onDeath += OnTargetDeath;
        }
        return;
    }

    public void OnTargetDeath(){
        // Debug.Log("Executed in rotary cannon");
        target = null;
        TriggerWeaponOff();
        state = State.IDLE;
        return;
    }

    public override void SetTeam(Team team) {
        base.SetTeam(team);
        targetFilter.NoFilter();                                            //sets the contact filter that we will pass in into OverlapCircle so that it will only detect
        targetFilter.SetLayerMask(LayerMask.GetMask(enemyTeam));            //ships from the enemy team
    }


}
