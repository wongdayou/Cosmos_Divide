using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//the difference between rocket launcher and missile system is that rocket launcher needs to face the enemy before shooting
public class RocketLauncher : FixedCannon
{
    RaycastHit2D enemyInFront;
    protected override void Update(){
        base.Update();
        

        
        foreach (Transform tr in firePoints) {
            // note to self, layermask filter in Physics2D.Raycast DOES NOT use layer index given by LayerMask.NameToLayer
            hit = Physics2D.Raycast(tr.position, tr.up, data.detectRange, LayerMask.GetMask(enemyTeam));

            if (hit.collider != null && !isShooting){
                TriggerWeaponOn();
            }
            else if (hit.collider == null){
                TriggerWeaponOff();
            }
        }
            
    }

    public override void Shoot(){
        foreach (Transform firePoint in firePoints){
            GameObject bullet = Instantiate(data.bulletPrefab, firePoint.position, firePoint.rotation);
            HomingMissile _hm = bullet.GetComponent<HomingMissile>();
            if (_hm == null){
                Debug.Log("Rocket Launcher: bullet does not have homing missile script!");
            }
            else{
                _hm.SetBulletLayer(identifier + " Bullet");
                enemyInFront = Physics2D.Raycast(firePoint.position, firePoint.up, data.detectRange, LayerMask.GetMask(enemyTeam));
                if (enemyInFront.collider != null){
                    _hm.SetTarget(enemyInFront.transform);
                }
                else {
                    _hm.SetTarget(target);
                }
                
            }
        }
        AudioManager.instance.Play(data.shootSound);
    }
}
