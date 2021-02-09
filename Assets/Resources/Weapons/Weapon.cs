using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Melee, Range }


[System.Serializable]
public class Weapon {
    public string name;
    public GameObject handObject;
    public GameObject bagObject;
    public float damage;
    public float speed;
    public string equipSound;
    public WeaponType type;

    public Weapon(string name, GameObject handObject, GameObject bagObject, float damage, float speed, string equipSound, WeaponType type) {
        this.name = name;
        this.handObject = handObject;
        this.bagObject = bagObject;
        this.damage = damage;
        this.speed = speed;
        this.equipSound = equipSound;
        this.type = type;
    }
}
