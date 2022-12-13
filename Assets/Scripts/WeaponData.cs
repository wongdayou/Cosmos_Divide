using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperClasses;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Cosmos divide/WeaponData", order = 0)]
public class WeaponData : ScriptableObject {
    public GameObject bulletPrefab;
    public float fireRate;
    public float firingRange;
    public float detectRange;
    public int numOfBarrels;
    public string shootSound;
    public float rotateSpeed;

    public int cost;
    public string description;
    public ItemType type;
}

