using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : weapon
{
    public float reloadRate = 3f;
    public float searchInterval = 0.2f;
    public enum State { SHOOTING, SEARCHING }

    State state = State.SEARCHING;
    float timeToSearch = 0f;
    float timeToReload = 0f;
    Collider2D[] targetsInRange;
    int targetNum = 0;
    int firePointNum = 0;
    int firePointsLength = 0;

    protected override void Start(){
        base.Start();
        firePointsLength = firePoints.Count;
    }


    protected override void Update()
    {
        switch (state){
            case State.SEARCHING:
                if (Time.time > timeToSearch){
                    timeToSearch = Time.time + searchInterval;
                    targetsInRange = Physics2D.OverlapCircleAll(this.transform.position, data.firingRange, LayerMask.GetMask(enemyTeam));
                    if (targetsInRange.Length != 0 && Time.time > timeToReload){
                        state = State.SHOOTING;
                        targetNum = 0;
                        firePointNum = 0;   
                    }
                    
                }
                break;

            case State.SHOOTING:
                if (Time.time > timeToFire)
                {
                    timeToFire = Time.time + 1 / data.fireRate;
                    Shoot();
                }
                break;
        }
        
    }

    public override void Shoot(){
        if (targetNum >= targetsInRange.Length){
            state = State.SEARCHING;
            return;
        }
        if (targetsInRange[targetNum] == null){
            targetNum ++;
            return;
        }
        GameObject _bullet = Instantiate(data.bulletPrefab, firePoints[firePointNum].position, Quaternion.identity);
        AudioManager.instance.Play(data.shootSound);
        HomingMissile _hm = _bullet.GetComponent<HomingMissile>();
        if (_hm == null){
            Debug.Log("Missile System: bullet does not have homing missile script!");
        }
        else{
            _hm.SetBulletLayer(identifier + " Bullet");
            _hm.SetTarget(targetsInRange[targetNum].transform);
        }
        //Debug.Log(firePointNum);
        firePointNum += 1;
        //if we have fired all missiles, reload and switch state to SEARCHING
        if (firePointNum >= firePointsLength) {
            timeToReload = Time.time + reloadRate;
            state = State.SEARCHING;
            return;
        }
        targetNum += 1;
        return;        
    }
}
