/*
    old script. See ShipMovementAI for newer script
    can delete this if really unnecessary
*/

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveAI : MonoBehaviour
{
    //get reference to enemy's rigidbody so we can move it
    public Rigidbody2D rb;

    //get transform of target (player)
    public Transform target;

    public float distanceFromRoam = 2f;

    //stats of enemy
    [SerializeField]
    private float movementSpeed = 100f;
    [SerializeField]
    private float rotateSpeed = 300f;
    [SerializeField]
    private float searchDelay = 0.5f;

    [SerializeField]
    private float tooCloseDistance = 2f;

    [SerializeField]
    private float safeDistance = 10f;

    public Vector3 roamPosition;
    public float roamOffset = 20f;
    private Quaternion retreatingRotation;
    public float xLimit = 165f;
    public float yLimit = 165f;

    //states
    public enum State
    {
        searching,
        chasing,
        backingOff
    }

    public State state = State.searching;
    private Vector3 newRoamPosition;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (rb == null)
        {
            Debug.Log("Rigidbody not found");
        }

        if (target == null){
            StartCoroutine(FindTarget());
        }
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        switch (state)
        {
            case State.searching:
                RoamAround();
                break;

            case State.chasing:
                //move towards target
                ChaseTarget();

                //if too close to target back off
                if (Vector2.Distance(target.position, transform.position) <= tooCloseDistance)
                {
                    //find the direction to retreat away to
                    Vector3 direction = -(target.position - this.transform.position).normalized;
                    retreatingRotation = Quaternion.LookRotation(Vector3.forward, direction);
                    state = State.backingOff;
                }
                
                break;

            case State.backingOff:               
                transform.rotation = Quaternion.RotateTowards(transform.rotation, retreatingRotation, rotateSpeed * Time.deltaTime);
                rb.AddForce(transform.up * movementSpeed);
                if (Vector2.Distance(target.position, transform.position) >= safeDistance)
                {
                    state = State.chasing;
                }
                break;
        }
    }


    IEnumerator FindTarget()
    {
        GameObject result = GameObject.FindGameObjectWithTag("Player");
        if (result == null)
        {
            yield return new WaitForSeconds(searchDelay);
            StartCoroutine(FindTarget());
        }

        //this statement is in case if this function is called before the target has being destroyed fully
        else if ((target != null) && (result == target.gameObject)){
            yield return new WaitForSeconds(searchDelay);
            StartCoroutine(FindTarget());
        }
        else
        {
            SetTarget(result);
            yield return false;
        }
    }

    protected virtual void SetTarget(GameObject result){
        target = result.transform;
        state = State.chasing;
        Entity t = target.GetComponent<Entity>();
        t.onDeath += OnTargetDeath;
    }

    protected void FindNewRoamPosition()
    {
        //Debug.Log("In FindNewRoamPosition");
        //limit the roaming position to be within the boundary
        float xPos = Mathf.Clamp(this.transform.position.x + Random.Range(-roamOffset, roamOffset), -xLimit, xLimit);
        float yPos = Mathf.Clamp(this.transform.position.y + Random.Range(-roamOffset, roamOffset), -yLimit, yLimit);
        roamPosition = new Vector3(xPos, yPos, 0);
        return;
    }


    protected void RoamAround() {
        //Debug.Log("In roamAround");
        if ((Vector2.Distance(roamPosition, transform.position) <= distanceFromRoam) || roamPosition == null){
            FindNewRoamPosition();
            return;
        }
        rb.AddForce(transform.up * movementSpeed);
        Vector3 direction = (roamPosition - this.transform.position).normalized;
        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
    }

    protected void ChaseTarget()
    {
        //Debug.Log("In chase target");
        //move towards target
        rb.AddForce(transform.up * movementSpeed);
        //rotate ship to face target
        if (target != null){
            Vector3 direction = (target.position - this.transform.position).normalized;
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
        }
    }

    protected virtual void OnTargetDeath() {
        //roam around and find new target
        state = State.searching;
        StartCoroutine(FindTarget());
        FindNewRoamPosition();
        return;
    }

    public void RemoveDelegateDependencies(){
        if (target != null){
            target.GetComponent<Entity>().onDeath -= OnTargetDeath;
        }
        return;
    }
}

*/