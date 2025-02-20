using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour, IScene {
    PlayerController pc;
    public IntroCivilianController icc;
    public PriestController priest;
    // 1: intro dialog, 2: goes to church, 3: major dialog 
    public int stage = 0;
    public static Intro instance = null;
    public ConversationController conversation;

    private void Awake() {
        instance = this;
        GameManager.instance.scene = this;
        GameManager.instance.SetInputMode(0);
        pc = GameManager.instance.pc;
        pc.EnableSound(true);
        pc.EnableListen(true);
        RefreshPlayerInventory();
    }

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void RefreshPlayerInventory() {
        HudController.instance.RefreshObjectSlots();
    }

    public void SetStage(int num) {
        stage = num;
        switch (stage) {
            // conversation
            case 1:
                //GameManager.instance.inputMode = 2;
                break;
            // end conversation
            case 2:
                //GameManager.instance.inputMode = 1;
                break;
        }
    }

    public void MakeSelection() {
        //if (stage == 1) {
            conversation.NextDialog(this);
        //}
    }

    public void SelectUp() {

    }

    public void SelectDown() {

    }

    public void StartConversation(string name) {
        GameManager.instance.SetInputMode(2);
        pc.GetComponent<PlayerMoveController>().Idle();
        conversation.StartDialog(name);
        conversation.NextDialog(this);
    }

    public void FinishConversation(string conversation) {
        GameManager.instance.SetInputMode(1);
        switch (conversation) {
            case "InitialDialog":
                icc.StopTalkPlayer();
                SetStage(2);
                break;
            case "PriestDialog":
                priest.StopTalkPlayer();
                break;
        }
        
    }
}
