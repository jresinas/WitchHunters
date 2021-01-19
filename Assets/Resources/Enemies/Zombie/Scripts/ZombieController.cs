using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    private GameObject target;
    private float speedStep = 3.7f;
    private float speedStop = 0.8f;
    private float speed;
    private Rigidbody rb;
    private Animator anim;
    private EnemyController ec;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        ec = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ec.dead && !ec.busy) {
            transform.LookAt(target.transform.position);
            //if (Physics.Raycast)
            anim.SetBool("Walk", true);

            Move();
        }
    }

    private void Move() {
        rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }

    private void Step() {
        speed = speedStep;
    }

    private void Stop() {
        speed = speedStop;
    }
}
