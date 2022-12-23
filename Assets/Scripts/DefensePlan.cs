using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperClasses;

public class DefensePlan : MonoBehaviour
{
    public float xMin = 0f;
    public float xMax = 0f;
    public float yMin = 0f;
    public float yMax = 0f;
    public Team team;
    public List<Entity> enemies = new List<Entity>();
    public delegate void OnEnemyEnter();
    public OnEnemyEnter onEnemyEnter;
    // Start is called before the first frame update
    private void Start() {
        if (xMin == 0f && xMax == 0f && yMin == 0f && yMax == 0f){
            Debug.LogWarning("Warning: Defense plan has boundaries on default");
        }
    }

    public Vector2 GetNewPos(){
        return new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
    }

    public void OnTriggerEnter2D(Collider2D other) {
        //TODO alert all ships in the defense area to head towards enemy
        Entity _en = other.gameObject.GetComponentInParent<Entity>();
        if (_en != null){
            if (_en.team != team){
                //check if this entity is already inside
                foreach(Entity _enemy in enemies){
                    if (_enemy == _en){
                        return;
                    } 
                }
                enemies.Add(_en);
                _en.onDeath += UpdateEnemyList;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        Entity _en = other.gameObject.GetComponentInParent<Entity>();
        if (_en != null){
            if (_en.team != team){
                foreach (Entity _enemy in enemies){
                    if (_enemy == _en){
                        enemies.Remove(_en);
                        _en.onDeath -= UpdateEnemyList;
                        break;                                          //need a break here as if you iterate over the list after removing something you will get an error
                    }
                }
            }
        }
    }

    public void UpdateEnemyList(){
        foreach (Entity _en in enemies){
            if (_en == null){
                enemies.Remove(_en);
            }
        }
    }

    private void OnDestroy() {
        //Remove all the linked functions
        foreach (Entity _en in enemies){
            _en.onDeath -= UpdateEnemyList;
        }
    }
}
