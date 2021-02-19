using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    public HunterController hc;

    private void Awake() {
        LoadPlayerInventory();
        EquipWeapon(0);
    }
    // Start is called before the first frame update
    void Start()
    {
        
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

        hc.weapons = new PlayerWeapon[2];
        for (int i = 0; i < GameManager.instance.weapons.Length; i++) {
            hc.weapons[i] = new PlayerWeapon(GameManager.instance.weapons[i],
                                Instantiate(GameManager.instance.weapons[i].handObject, hc.rightHand.transform),
                                Instantiate(GameManager.instance.weapons[i].bagObject, hc.bag.transform)
                            );
        }

    }

    private void EquipWeapon(int weaponIndex) {
        for (int i = 0; i < hc.weapons.Length; i++) {
            if (i != weaponIndex) hc.UnequipWeapon(i);
        }

        hc.EquipWeapon(weaponIndex, false);
    }
}
