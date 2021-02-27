using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestController : MonoBehaviour, IInteractive {
    Animator anim;

    void Awake() {
        anim = GetComponent<Animator>();
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
        Intro.instance.StartConversation("PriestDialog");
    }

    public void StopTalkPlayer() {
        anim.SetBool("Talk", false);
    }
}
