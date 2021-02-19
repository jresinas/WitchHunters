using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour {
    HunterController hc;
    private void Awake() {
        hc = GameManager.instance.hc;
        hc.EnableInputs(true);
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
}
