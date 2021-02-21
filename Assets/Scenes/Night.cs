using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Night : MonoBehaviour {
    PlayerController pc;

    // Start is called before the first frame update
    void Awake(){
        pc = GameManager.instance.pc;
        GameManager.instance.inputMode = 1;
        pc.EnableSound(true);
        pc.EnableListen(true);
        RefreshPlayerInventory();
    }

    private void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void RefreshPlayerInventory() {
        HudController.instance.RefreshObjectSlots();
    }
}
