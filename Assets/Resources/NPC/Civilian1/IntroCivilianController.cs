using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class IntroCivilianController : MonoBehaviour {
    public Transform churchDoors;

    NavMeshAgent agent;
    Animator anim;
    Transform player;
    [SerializeField] GameObject speechBubble;
    GameObject sb;
    SpeechBubbleController sbc;

    int talkStage = 0;


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
        //Intro.instance.StartConversation();
        Intro.instance.StartConversation("InitialDialog");
        //OpenBubble("¡¿Que estás haciendo aquí?!");
    }

    public void StopTalkPlayer() {
        agent.SetDestination(churchDoors.position);
        agent.stoppingDistance = 1;
        anim.SetBool("Talk", false);
        agent.isStopped = false;
    }

    //public void NextTalk() {
    //    Debug.Log("NextTalk");
    //    if (sbc != null) sbc.EndBubble();
    //    talkStage++;
    //    switch (talkStage) {
    //        case 1:
    //            Debug.Log("TalkStage 1");
    //            OpenBubble("¡Rápido!¡Que ya vienen!");
    //            break;
    //        case 2:
    //            Debug.Log("TalkStage 2");
    //            OpenBubble("¡Tienes que refugiarte en la iglesia si quieres seguir viviendo!");
    //            break;
    //        case 3:
    //            Debug.Log("TalkStage 3");
    //            StopTalkPlayer();
    //            Intro.instance.SetStage(2);
    //            break;
    //    }
    //}

    ////void CloseBubble(string text) {
    ////    sb = Instantiate(speechBubble);
    ////    SpeechBubbleController sbc = sb.GetComponent<SpeechBubbleController>();
    ////    sbc.SetCharacter(transform, text);
    ////    sbc.FlipX();
    ////}

    //void OpenBubble(string text) {
    //    sb = Instantiate(speechBubble);
    //    sbc = sb.GetComponent<SpeechBubbleController>();
    //    sbc.SetBubble(transform, text);
    //    sbc.FlipX();
    //    sbc.StartBubble();
    //}

    //void StopTalkPlayer() {
    //    agent.SetDestination(churchDoors.position);
    //    agent.stoppingDistance = 1;
    //    anim.SetBool("Talk", false);
    //    agent.isStopped = false;
    //    Destroy(sb);
    //}
}
