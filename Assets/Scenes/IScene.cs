using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScene{
    // Start is called before the first frame update
    public void MakeSelection();
    public void SelectUp();
    public void SelectDown();
}
