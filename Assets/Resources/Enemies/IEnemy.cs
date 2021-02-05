using System.Collections;
using UnityEngine;

interface IEnemy {
    int CYCLES_SEARCHING {
        get;
    }
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

    //Vector3 GetTarget(GameObject player, GameObject church);
    void IsSeeingPlayer(GameObject player);
    void IsSearchingPlayer(int cyclesRemaining);
    void IsGoingToChurch();
    void IsMoving();
    void IsArrived(Vector3 target);
    void IsDamaged(float amount);
}
