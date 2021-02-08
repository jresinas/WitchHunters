using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Trap : Object {
    public Sprite icon;
    public int amount;

    public Trap(string name, GameObject prefab, Sprite icon, int amount) : base(name, prefab) { 
        this.icon = icon;
        this.amount = amount;
    }
}
