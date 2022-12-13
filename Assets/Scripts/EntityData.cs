using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperClasses;

[CreateAssetMenu(fileName = "EntityData", menuName = "Cosmos divide/EntityData", order = 0)]
public class EntityData : ScriptableObject {
    public int maxHealth;
    public int load;
    public int cost;
    public string description;
    public float speed;
    public float rotationSpeed;
    public ItemType type;
    public int numOfTurrets;
    public int numOfDeploymentPods;
    public int defeatScore;
    public GameObject deathExplosion;
    public string deathExplosionSound;

    // data for ship movement ai
    public float tooCloseDistance;
    public float safeDistance;
}
