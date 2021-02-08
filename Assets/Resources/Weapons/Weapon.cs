using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Melee, Range }


[System.Serializable]
public class Weapon {
    public string name;
    public GameObject handPrefab;
    public GameObject keepPrefab;
    public float damage;
    public float speed;
    public WeaponType type;

    public Weapon(string name, GameObject handPrefab, GameObject keepPrefab, float damage, float speed, WeaponType type) {
        this.name = name;
        this.handPrefab = handPrefab;
        this.keepPrefab = keepPrefab;
        this.damage = damage;
        this.speed = speed;
        this.type = type;
    }
}
