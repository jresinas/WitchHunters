using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour, IScene {
    HunterController hc;
    public IntroCivilianController icc;
    public int stage = 0;
    public static Intro instance = null;

    private void Awake() {
        instance = this;
        GameManager.instance.scene = this;
        hc = GameManager.instance.hc;
        hc.EnableSound(true);
        hc.EnableListen(true);
        RefreshPlayerInventory();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RefreshPlayerInventory() {
        HudController.instance.RefreshObjectSlots();
    }

    public void SetStage(int num) {
        stage = num;
        switch (stage) {
            // conversation
            case 1:
                GameManager.instance.inputMode = 2;
                break;
            // end conversation
            case 2:
                GameManager.instance.inputMode = 1;
                break;
        }
    }

    public void MakeSelection() {
        if (stage == 1) {
            icc.StopTalkPlayer();
            SetStage(2);
        }
    }
}
