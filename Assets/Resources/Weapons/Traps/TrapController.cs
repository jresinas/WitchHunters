using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    Collider col;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Enemy") {
            anim.SetBool("Active", true);
            EnemyController ec = collider.GetComponent<EnemyController>();
            ec.Trapped(transform.position);
        }
    }
}
