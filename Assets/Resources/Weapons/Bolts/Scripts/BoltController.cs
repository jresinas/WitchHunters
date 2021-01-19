using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltController : MonoBehaviour
{
    //Rigidbody rb;
    float speed = 40f;
    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        //rb.MovePosition(rb.position + Vector3.forward * speed * Time.deltaTime);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Enemy") {
            EnemyController ec = collider.gameObject.GetComponent<EnemyController>();
            ec.Damage(1);
            Destroy(gameObject);
        }
    }
}
