using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ITrapController{
    bool Pickable();
    void PickTrap(HunterController hc);
}
