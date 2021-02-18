using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerWeapon {
    public Weapon weapon;
    public GameObject handObject;
    public GameObject bagObject;

    public PlayerWeapon(Weapon weapon, GameObject handObject, GameObject bagObject) {
        this.weapon = weapon;
        this.handObject = handObject;
        this.bagObject = bagObject;
    }

}
