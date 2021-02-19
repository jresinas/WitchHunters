using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour {
    public string scene;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.LoadScene(scene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
