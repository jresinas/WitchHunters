using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Trap : Object {
    public GameObject prefab;

    //public Trap(string name, GameObject prefab, Sprite icon, ObjectType type) : base(name, icon, type) {
    public Trap(string name, GameObject prefab, Sprite icon) : base(name, icon) {
        this.prefab = prefab;
    }
}
