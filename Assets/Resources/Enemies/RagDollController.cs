using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollController : MonoBehaviour
{
    public GameObject blood;

    // Start is called before the first frame update
    void Start()
    {
            Instantiate(blood, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Push(Vector3 direction, float force) {
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rbs) {
            rb.AddForce(direction * force, ForceMode.Impulse);
            //rb.AddForceAtPosition(direction * 15f, direction * 15f, ForceMode.Impulse);
            //rb.AddExplosionForce(50f, transform.position, 15f);
        }
    }

}
