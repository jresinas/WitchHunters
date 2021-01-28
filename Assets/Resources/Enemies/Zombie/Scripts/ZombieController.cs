using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour, IEnemy {
    // Number of SEARCH_TIME cycles that enemy continues searching player since stop seeing him
    public int CYCLES_SEARCHING { get => 10; }

    // Minimum speed multiplicator for move animation
    private float MIN_SPEED_MULT = 0.8f;
    // Maximum speed multiplicator for move animation
    private float MAX_SPEED_MULT = 1.4f;

    // Zombie life
    private float _life = 5;
    // Zombie speed (when make an step and when stop)
    private float _speed;
    private float speedStep = 3.7f;
    private float speedStop = 0.8f;
    // Zombie melee damage
    private float _meleeDamage = 1;

    private EnemyController ec;
    // Auxiliar variable to count cylces spend searching player
    private int cyclesSearching;

    public float life {
        get => _life;
        set => _life = value;
    }
    public float speed {
        get => _speed;
    }
    public float meleeDamage {
        get => _meleeDamage;
    }
    

    // Start is called before the first frame update
    void Start() {
        ec = GetComponent<EnemyController>();
        ec.anim.SetFloat("SpeedMultiplier", Random.Range(MIN_SPEED_MULT, MAX_SPEED_MULT));
    }

    // Update is called once per frame
    void Update() { }

    // Return Vector3 of current target (for nav mesh)
    //public Vector3 GetTarget(GameObject player, GameObject church) {
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit)) {
    //        if (hit.collider.gameObject == player) {
    //            cyclesSearching = CYCLES_SEARCHING;
    //            return player.transform.position;
    //        } else {
    //            if (cyclesSearching > 0) {
    //                cyclesSearching--;
    //            } else {
    //                return church.transform.position;
    //            }
    //        }
    //    }

    //    return Vector3.zero;
    //}

    // Actions to do when is moving to the current target
    public void IsMoving() {
        ec.Move();
    }

    // Actions to do when is near the target
    public void IsArrived(Vector3 target) {
        ec.anim.SetBool("Walk", false);
        transform.LookAt(target);
        ec.anim.SetBool("Attack", true);
    }

    // Callback from animation move when zombie makes a step
    private void Step() {
        _speed = speedStep;
    }

    // Callback from animation move when zombie stops
    private void Stop() {
        _speed = speedStop;
    }
}
