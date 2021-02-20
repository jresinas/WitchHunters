using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Night : MonoBehaviour {
    HunterController hc;

    // Start is called before the first frame update
    void Awake(){
        hc = GameManager.instance.hc;
        GameManager.instance.inputMode = 1;
        hc.EnableSound(true);
        hc.EnableListen(true);
        RefreshPlayerInventory();
    }

    private void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RefreshPlayerInventory() {
        HudController.instance.RefreshObjectSlots();
    }
}
