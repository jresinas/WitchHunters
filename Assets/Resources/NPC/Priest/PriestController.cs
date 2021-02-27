using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PriestController : MonoBehaviour, IInteractive {
    Animator anim;
    NavMeshAgent agent;
    [SerializeField] Transform churchDoors;

    void Awake() {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        if (agent.hasPath && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) {
            EnterChurch();
        }
    }

    public bool Available() {
        return true;
    }

    public string GetInteractionType() {
        return "NPC";
    }

    public void Interact(PlayerController pc) {
        StartTalkPlayer(pc.transform);
    }

    void StartTalkPlayer(Transform player) {
        transform.LookAt(player);
        anim.SetBool("Talk", true);
        switch (GameManager.instance.scene.GetType().ToString()) {
            case "Intro":
                Intro.instance.StartConversation("PriestDialog");
                break;
        }
        
    }

    public void StopTalkPlayer() {
        anim.SetBool("Talk", false);
        switch (GameManager.instance.scene.GetType().ToString()) {
            case "Intro":
                GoToChurch();
                break;
        }
    }

    void GoToChurch() {
        anim.SetBool("Walk", true);
        agent.SetDestination(churchDoors.position);
    }

    void EnterChurch() {
        Destroy(gameObject);
    }
}
