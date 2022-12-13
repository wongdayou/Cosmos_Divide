using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public enum SpawnPosition { TOP, BOTTOM, LEFT, RIGHT };

    public SpawnPosition _sp;

    public Vector3 GetRandomPosition () {
        Vector3 _pos = this.transform.position;
        switch (_sp) {
            case SpawnPosition.TOP:
            case SpawnPosition.BOTTOM:
                _pos.x = Random.Range(-170f, 170f);
                break;
            case SpawnPosition.LEFT:
            case SpawnPosition.RIGHT:
                _pos.y = Random.Range(-170f, 170f);
                break;
        }
        return _pos;
    }
}
