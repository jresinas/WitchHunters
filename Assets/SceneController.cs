using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public static SceneController instance = null;
    public HunterController hc;
    public WeaponsController wc;
    //private bool pause = false;


    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        hc.objs = new PlayerObject[10];
        for (int i = 0; i < GameManager.instance.traps.Length; i++) {
            hc.objs[i] = new PlayerObject(GameManager.instance.traps[i], 2); 
        }

        wc.weapons = new PlayerWeapon[2];
        for (int i = 0; i < GameManager.instance.weapons.Length; i++) {
            wc.weapons[i] = new PlayerWeapon(GameManager.instance.weapons[i],
                                Instantiate(GameManager.instance.weapons[i].handObject, wc.rightHand.transform),
                                Instantiate(GameManager.instance.weapons[i].bagObject, wc.bag.transform)
                            );
            //hc.weapons[i].weapon = GameManager.instance.weapons[i];
            //hc.weapons[i].handObject = Instantiate(GameManager.instance.weapons[i].handObject, hc.rightHand.transform);
            //hc.weapons[i].bagObject = Instantiate(GameManager.instance.weapons[i].bagObject, hc.bag.transform);
        }

        if (HudController.instance != null) {
            HudController.instance.RefreshObjectSlots();
        }
        
        wc.UnequipWeapon(1);
        wc.EquipWeapon(0,false);
    }

    // Update is called once per frame
    void Update() {
        
    }

    //public void SetPause() {
    //    pause = !pause;
    //    Time.timeScale = pause ? 0 : 1;
    //    SoundManager.instance.Pause(pause);
    //    HudController.instance.Pause(pause);
    //}

    //public bool IsPaused() {
    //    return pause;
    //}
}
