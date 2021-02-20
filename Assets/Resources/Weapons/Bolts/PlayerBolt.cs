using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerBolt {
    public Bolt bolt;
    public int amount;

    public PlayerBolt(Bolt bolt, int amount) {
        this.bolt = bolt;
        this.amount = amount;
    }
}
