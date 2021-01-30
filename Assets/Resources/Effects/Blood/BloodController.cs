using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodController : MonoBehaviour
{
    private float DESTROY_TIME = 2f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, DESTROY_TIME);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
