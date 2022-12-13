using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperClasses;

public class ShipMovementAI : MonoBehaviour
{
    protected Team team;
    public EntityData data;
    private Rigidbody2D rb;
    public Transform target;
    public DefensePlan defensePlan;
    public Vector3 patrolPosition;

    public Vector3 roamPosition;
    private Quaternion retreatingRotation;
    Vector3 direction;
    Quaternion toRotation;

    //for defense purposes
    float xBound = -1;
    float yBound = -1;

    public enum State
    {
        searching,
        chasing,
        backingOff,
        defending,
        patrolling
    }

    public State state = State.searching;
    private Vector3 newRoamPosition;
    private float timeToSearch = 0f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        if (rb == null){
            Debug.LogError(this.gameObject.name + " has no rigidbody!");
        }
        Entity _en = this.gameObject.GetComponent<Entity>();
        if (_en != null){
            team = _en.team;
        }
        if ((state == State.defending || state == State.patrolling) && defensePlan == null){
            Debug.LogWarning(this.gameObject.name + " movement AI has no defense plan even though it is defending");
        }
        else if ((state == State.defending || state == State.patrolling) && defensePlan != null){
            patrolPosition = defensePlan.GetNewPos();
        }
    }

    protected virtual void FixedUpdate()
    {
        switch (state) {
            case (State.searching) :
                if (Time.time > timeToSearch) {
                    SearchTarget();
                }
                RoamAround();
                break;

            case (State.chasing) :
                ChaseTarget();

                //if too close to target back off
                if (Vector2.Distance(target.position, transform.position) <= data.tooCloseDistance)
                {
                    //find the direction to retreat away to
                    direction = -(target.position - this.transform.position).normalized;
                    retreatingRotation = Quaternion.LookRotation(Vector3.forward, direction);
                    state = State.backingOff;
                }
                break;

            case (State.backingOff) :
                transform.rotation = Quaternion.RotateTowards(transform.rotation, retreatingRotation, data.rotationSpeed * Time.deltaTime);
                rb.AddForce(transform.up * data.speed);
                if (Vector2.Distance(target.position, transform.position) >= data.safeDistance)
                {
                    state = State.chasing;
                }
                break;

            case (State.patrolling) :
                rb.AddForce(transform.up * data.speed);
                direction = (patrolPosition - this.transform.position).normalized;
                toRotation = Quaternion.LookRotation(Vector3.forward, direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, data.rotationSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, patrolPosition) < Constants.patrolOffset){
                    patrolPosition = defensePlan.GetNewPos();
                }
                break;
        }
    }

    void SearchTarget()
    {
        GameObject result = null;
        switch (team) {
            case Team.BLUE:
                result = GetNearestTarget(GameMaster.gm.redTeamShips);
                break;
            case Team.RED:
                result = GetNearestTarget(GameMaster.gm.blueTeamShips);
                break;
            default:
                result = null;
                break;
        }
        

        //this statement is in case if this function is called before the target has being destroyed fully
        if (((target != null) && (result == target.gameObject)) || result == null) return;
        else
        {
            // Debug.Log("Found something");
            SetTarget(result);
            return;
        }
    }

    GameObject GetNearestTarget(List<GameObject> results) {
        if (results.Count < 1) return null;
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;
        Vector3 position = transform.position;
        float curDistance = 0f;
        foreach (GameObject target in results) {
            Vector3 diff = target.transform.position - position;
            curDistance = diff.sqrMagnitude;
            if (curDistance < closestDistance){
                closestDistance = curDistance;
                closest = target;
            }
        }
        return closest;
    }

    protected virtual void SetTarget(GameObject result){
        // this part where we check for the presence of a parent is if the team identifier is placed in a child object (e.g. for the player)
        if (result.transform.parent != null) {
            target = result.transform.parent;
        }
        else {
            target = result.transform;
        }
        
        state = State.chasing;
        Entity t = target.GetComponent<Entity>();
        if (t != null){
            t.onDeath += OnTargetDeath;
        }
        
    }

    protected void ChaseTarget()
    {
        //Debug.Log("In chase target");
        //move towards target
        rb.AddForce(transform.up * data.speed);
        //rotate ship to face target
        if (target != null){
            direction = (target.position - this.transform.position).normalized;
            toRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, data.rotationSpeed * Time.deltaTime);
        }
        else{
            state = State.searching;
        }
    }

    protected virtual void OnTargetDeath() {
        //roam around and find new target
        FindNewRoamPosition();
        state = State.searching;
        return;
    }

    protected void RoamAround() {
        if ((Vector2.Distance(roamPosition, transform.position) <= Constants.distanceFromRoam)){
            FindNewRoamPosition();
            return;
        }
        rb.AddForce(transform.up * data.speed);
        direction = (roamPosition - this.transform.position).normalized;
        toRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, data.rotationSpeed * Time.deltaTime);
    }

    protected void FindNewRoamPosition()
    {
        //Debug.Log("In FindNewRoamPosition");
        //limit the roaming position to be within the boundary
        float xPos = Mathf.Clamp(this.transform.position.x + Random.Range(-Constants.roamOffset, Constants.roamOffset), -Constants.xLimit, Constants.xLimit);
        float yPos = Mathf.Clamp(this.transform.position.y + Random.Range(-Constants.roamOffset, Constants.roamOffset), -Constants.yLimit, Constants.yLimit);
        roamPosition = new Vector3(xPos, yPos, 0);
        return;
    }

    protected void SetPatrolBound(){
        Vector3 _targetSize = new Vector3(0, 0, 0);
        if (target != null) {
            Entity _targetEn = target.gameObject.GetComponentInParent<Entity>();
            if (_targetEn != null){
                _targetSize = _targetEn.SizeOfEntity();
            }
        }
        Vector3 _thisSize = new Vector3(0, 0, 0);
        Entity _thisEn = this.gameObject.GetComponent<Entity>();
        if (_thisEn != null){
            _thisSize = _thisEn.SizeOfEntity();
        }
        xBound = target.position.x + _targetSize.x/2 + Constants.patrolOffset + _thisSize.x/2;
        yBound = target.position.y + _targetSize.y/2 + Constants.patrolOffset + _thisSize.y/2; 
        Debug.Log(this.gameObject.name + " patrol area: x = " + xBound + ", y = " + yBound);
    }

    protected void Patrol() {
        //patrol around the target, making sure not to move too far away from it

    }

    //if this ship dies need to remove the event dependencies else will have error
    void OnDisable() {
        if (target != null){
            Entity targetEn = target.GetComponent<Entity>();
            if (targetEn != null){
                targetEn.onDeath -= OnTargetDeath;
            }
        }
        return;
    }
}
