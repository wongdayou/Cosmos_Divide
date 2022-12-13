using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperClasses;
public class weapon : MonoBehaviour
{
    public WeaponData data;
    public List<Transform> firePoints = new List<Transform>();
    // public Transform[] firePoints;
    public Transform target;
    // public int bulletTier = 0;
    // public int numOfBarrels;

    public string identifier;                               // to set the layer of the bullets it shoots
    public string enemyTeam;                                // enemyTeam is for inherited classes like RotaryCannon and HomingMissile to detect targets

    protected bool isShooting = false;
    protected float timeToFire = 0;

    protected virtual void Start(){
        data.detectRange = data.firingRange + 5.0f;
        foreach (Transform tr in transform){
            if (tr.tag == "FirePoint"){
                firePoints.Add(tr);
            }
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isShooting && Time.time > timeToFire)
        {
            timeToFire = Time.time + 1 / data.fireRate;
            Shoot();
        }
    }

    public void TriggerWeaponOn()
    {
        isShooting = true;
    }

    public void TriggerWeaponOff()
    {
        //Debug.Log("Triggered off");
        isShooting = false;
    }
    public virtual void Shoot()
    {
        foreach (Transform firePoint in firePoints){
            GameObject bullet = Instantiate(data.bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<bullet>().SetBulletLayer(identifier + " Bullet");
        }
        AudioManager.instance.Play(data.shootSound);
    }

    public void SetTarget(Transform tr){
        // Debug.Log("Setting weapon target " + tr);
        target = tr;
        // Debug.Log(tr);
    }

    public void ClearTarget(){
        target = null;
    }

    public bool CheckWhetherTargetInRange(){
        if (target != null){
            Component[] _colliders = target.gameObject.GetComponentsInChildren<Collider2D>();
            if (_colliders != null){
                // if the closest point is within firing range start shooting
                Vector2 closestPoint;
                foreach (Collider2D _col in _colliders){
                    closestPoint = _col.ClosestPoint(this.transform.position);
                    float closestDistance = Vector2.Distance(this.transform.position, closestPoint);
                    if (closestDistance <= data.firingRange){
                        return true;
                    }
                }
            }
        }    
        return false;
    }

    public virtual void SetTeam(Team t){
        switch (t){
            case Team.BLUE:
                identifier = "Blue";
                enemyTeam = "Red";
                break;
            case Team.RED:
                identifier = "Red";
                enemyTeam = "Blue";
                break;
            default:
                Debug.Log("weapon: " + this.gameObject.name + " was not given a valid team");
                break;
        }

    }
}
