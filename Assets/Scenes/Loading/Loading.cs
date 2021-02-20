using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour {
    public string scene;
    public HunterController hc;
    public WeaponsController wc;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.LoadScene(scene);
        LoadPlayerInventory();
        EquipWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadPlayerInventory() {
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
        }

        wc.bolts = new PlayerBolt[3];
        for (int i = 0; i < GameManager.instance.bolts.Length; i++) {
            wc.bolts[i] = new PlayerBolt(GameManager.instance.bolts[i], 10);
        }

    }

    private void EquipWeapon(int weaponIndex) {
        for (int i = 0; i < wc.weapons.Length; i++) {
            if (i != weaponIndex) wc.UnequipWeapon(i);
        }

        wc.EquipWeapon(weaponIndex, false);
    }
}
