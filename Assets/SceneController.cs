using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public static SceneController instance = null;
    public HunterController hc;


    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        hc.objs = new PlayerObject[10];
        for (int i = 0; i < GameManager.instance.traps.Length; i++) {
            hc.objs[i] = new PlayerObject(GameManager.instance.traps[i], 2); 
        }

        HudController.instance.RefreshObjectSlots();
    }

    // Update is called once per frame
    void Update() {
        
    }
}
