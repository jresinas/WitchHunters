using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltController : MonoBehaviour
{
    float damage = 1f;
    float speed = 40f;
    public float hunterMovement = 0;
    public Vector3 hunterMovementRotation = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set mask for bolt offset rotation
        Vector3 offsetRotationMask = new Vector3(Mathf.Abs(transform.right.x), Mathf.Abs(transform.right.y), Mathf.Abs(transform.right.z));
        transform.position += ((transform.forward * speed) + (Vector3.Scale(offsetRotationMask, hunterMovementRotation)*hunterMovement)) * Time.deltaTime;
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Enemy") {
            EnemyController enemy = collider.gameObject.GetComponent<EnemyController>();
            enemy.DamageReceived(damage);
            Destroy(gameObject);
        }
    }
}
