using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    // Time for refresh pathfinding
    private float SEARCH_TIME = 1f;
    // Number of SEARCH_TIME cycles that enemy continues searching player since stop seeing him
    private int CYCLES_SEARCHING = 10;

    private GameObject church;
    private GameObject player;
    private float speedStep = 3.7f;
    private float speedStop = 0.8f;
    private float speed;
    private Rigidbody rb;
    private Animator anim;
    private EnemyController ec;
    private NavMeshAgent agent;
    private int cyclesSearching;

    // Start is called before the first frame update
    void Start()
    {
        church = GameObject.Find("ChurchDoor"); //GameObject.FindWithTag("Church");
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        ec = GetComponent<EnemyController>();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(player.transform.position);

        StartCoroutine(FindTarget());

    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance > agent.stoppingDistance && !ec.dead && !ec.busy) {
            Move();
        } else {
            agent.isStopped = true;
            anim.SetBool("Walk", false);
        }
    }

    private IEnumerator FindTarget() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit)) {
            if (hit.collider.gameObject == player) {
                agent.SetDestination(player.transform.position);
                cyclesSearching = CYCLES_SEARCHING;
            } else {
                if (cyclesSearching > 0) {
                    cyclesSearching--;
                } else {
                    agent.SetDestination(church.transform.position);
                }
            }
        }

        yield return new WaitForSeconds(SEARCH_TIME);
        StartCoroutine(FindTarget());

    }

    private void Move() {
        agent.isStopped = false;
        anim.SetBool("Walk", true);
        agent.speed = speed / 3.5f;
    }

    //private void FollowPlayer() {
    //    if (!ec.dead && !ec.busy) {
    //        transform.LookAt(target.transform.position);
    //        anim.SetBool("Walk", true);
    //        rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    //    }
    //}

    private void Step() {
        speed = speedStep;
    }

    private void Stop() {
        speed = speedStop;
    }
}
