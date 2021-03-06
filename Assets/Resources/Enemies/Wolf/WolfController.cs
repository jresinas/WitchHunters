using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfController : MonoBehaviour, IEnemy {
    // Time for refresh pathfinding
    public float SEARCH_TIME { get => 0.05f; }
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

    // Zombie melee damage
    private float _meleeDamage = 1;

    private EnemyController ec;
    private AudioSource audioSource;
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
        audioSource = GetComponent<AudioSource>();
        ec.anim.SetFloat("SpeedMultiplier", Random.Range(MIN_SPEED_MULT, MAX_SPEED_MULT));
    }

    public void IsSeeingPlayer(GameObject player) {
        if (Random.Range(0f, 1f) > 0.95f) {
            //SoundManager.instance.Play("ZombieSearch", audioSource);
        }
    }

    public void IsSearchingPlayer(int cyclesRemaining) {

    }

    public void IsGoingToChurch() {
        if (Random.Range(0f, 1f) > 0.95f) {
            //SoundManager.instance.Play("ZombieSearch", audioSource);
        }
    }

    // Actions to do when is moving to the current target
    public void IsMoving() {
        _speed = 10f; //24f;
        ec.Move();
    }

    // Actions to do when is near the target
    public void IsArrived(Vector3 target) {
        ec.anim.SetBool("Walk", false);
        transform.LookAt(target);
        ec.anim.SetBool("Attack", true);
        //SoundManager.instance.Play("ZombieAttack", audioSource);
    }

    public void IsDamaged(float amount) {
        //SoundManager.instance.Play("ZombieDamaged", audioSource);
    }

}
