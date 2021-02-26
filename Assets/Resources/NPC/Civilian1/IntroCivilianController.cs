using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class IntroCivilianController : MonoBehaviour {
    public Transform churchDoors;

    NavMeshAgent agent;
    Animator anim;
    Transform player;

    private void Awake() {
        player = GameManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start() {
        agent.SetDestination(player.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Intro.instance != null) {
            switch (Intro.instance.stage) {
                case 0:
                    if (agent.remainingDistance <= agent.stoppingDistance) {
                        Intro.instance.SetStage(1);
                        StartTalkPlayer();
                    }
                    break;
                case 2:
                    anim.SetBool("LastSprint",true);

                    if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) {
                        Destroy(gameObject);
                    }
                    break;
            }
        }
    }

    void StartTalkPlayer() {
        agent.isStopped = true;
        transform.LookAt(player);
        anim.SetBool("Talk", true);
        Intro.instance.StartConversation("InitialDialog");
    }

    public void StopTalkPlayer() {
        agent.SetDestination(churchDoors.position);
        agent.stoppingDistance = 1;
        anim.SetBool("Talk", false);
        agent.isStopped = false;
    }
}
