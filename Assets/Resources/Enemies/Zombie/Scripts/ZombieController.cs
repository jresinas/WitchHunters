using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    private GameObject church;
    private GameObject player;
    private float speedStep = 3.7f;
    private float speedStop = 0.8f;
    private float speed;
    private Rigidbody rb;
    private Animator anim;
    private EnemyController ec;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        church = GameObject.Find("ChurchDoor"); //GameObject.FindWithTag("Church");
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        ec = GetComponent<EnemyController>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        FindTarget();
        if (agent.remainingDistance > agent.stoppingDistance && !ec.dead && !ec.busy) {
            Move();
        } else {
            agent.isStopped = true;
        }
    }

    private void FindTarget() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit)) {
            if (hit.collider.gameObject == player) {
                agent.SetDestination(player.transform.position);
            } else {
                agent.SetDestination(church.transform.position);
            }
        }
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
