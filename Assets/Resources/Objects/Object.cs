using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum ObjectType { Traps, Potions }


[System.Serializable]
public class Object {
    public string name;
    public Sprite icon;
    //public ObjectType type;


    //public Object(string name, Sprite icon, ObjectType type) {
    public Object(string name, Sprite icon) {
        this.name = name;
        this.icon = icon;
        //this.type = type;
    }

}
