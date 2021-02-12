using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public static SceneController instance = null;
    public HunterController hc;
    private bool pause = false;


    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        hc.objs = new PlayerObject[10];
        for (int i = 0; i < GameManager.instance.traps.Length; i++) {
            hc.objs[i] = new PlayerObject(GameManager.instance.traps[i], 2); 
        }
        hc.weapons = new Weapon[2];
        for (int i = 0; i < GameManager.instance.weapons.Length; i++) {
            hc.weapons[i] = GameManager.instance.weapons[i];
        }

        HudController.instance.RefreshObjectSlots();
        hc.EquipWeapon(0);
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void SetPause() {
        pause = !pause;
        Time.timeScale = pause ? 0 : 1;
        SoundManager.instance.Pause(pause);
        HudController.instance.Pause(pause);
    }

    public bool IsPaused() {
        return pause;
    }
}
