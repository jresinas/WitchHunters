using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerObject {
    public Object obj;
    public int amount;

    public PlayerObject(Object obj, int amount) {
        this.obj = obj;
        this.amount = amount;
    }

    public bool isTrap() {
        return obj != null && obj is Trap;
    }

    public bool isPotion() {
        return obj != null && obj is Potion;
    }
}
