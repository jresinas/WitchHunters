using System.Collections;
using UnityEngine;

interface IEnemy {
    float life {
        get;
        set;
    }
    float speed {
        get;
    }
    float meleeDamage {
        get;
    }

    Vector3 GetTarget(GameObject player, GameObject church);
    void IsMoving();
    void IsArrived();
}
