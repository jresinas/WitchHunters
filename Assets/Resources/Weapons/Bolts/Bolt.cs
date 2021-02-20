using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum ObjectType { Traps, Potions }


[System.Serializable]
public class Bolt {
    public string name;
    public Sprite icon;
    public GameObject prefab;

    //public Trap(string name, GameObject prefab, Sprite icon, ObjectType type) : base(name, icon, type) {
    public Bolt(string name, Sprite icon, GameObject prefab) {
        this.name = name;
        this.icon = icon;
        this.prefab = prefab;
    }

}
