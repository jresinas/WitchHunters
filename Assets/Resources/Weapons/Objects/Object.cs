using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Object {
    public string name;
    public GameObject prefab;

    public Object(string name, GameObject prefab) {
        this.name = name;
        this.prefab = prefab;
    }

}
