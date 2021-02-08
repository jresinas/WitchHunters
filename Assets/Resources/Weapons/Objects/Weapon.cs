using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Melee, Range }


[System.Serializable]
public class Weapon : Object {
    public GameObject keepPrefab;
    public float damage;
    public float speed;
    public WeaponType type;

    public Weapon(string name, GameObject prefab, GameObject keepPrefab, float damage, float speed, WeaponType type) : base(name, prefab) { 
        this.keepPrefab = keepPrefab;
        this.damage = damage;
        this.speed = speed;
        this.type = type;
    }
}
