using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractive{
    bool Available();
    void Interact(PlayerController pc);

    string GetInteractionType();
}
