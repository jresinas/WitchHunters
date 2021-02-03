using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IObject{
    bool Pickable();
    void PickObject(HunterController hc);
}
