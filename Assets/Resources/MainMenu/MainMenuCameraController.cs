using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraController : MonoBehaviour {
    private float FINAL_Y = 25f;
    private float BRAKE_Y = 80;
    private float FALL_FORCE = 100f;
    private float BRAKE_FORCE = 50f;
    private float LOOP_NUMBER = 10f;
    private float LOOP_START = 200f;
    private float LOOP_END = 100f;

    Rigidbody rb;
    Camera cam;
    bool fall = false;

    float loops = 0;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Submit")) {
            fall = true;
        }
    }

    private void FixedUpdate() {
        if (fall) {
            if (transform.position.y > BRAKE_Y) {
                //rb.useGravity = true;
                rb.AddForce(0f, -FALL_FORCE, 0f);
                transform.localEulerAngles = UpdateAngle(transform.position.y - FINAL_Y);
                cam.orthographicSize = UpdateSize(transform.position.y - FINAL_Y);

                if (transform.position.y <= LOOP_END && loops < LOOP_NUMBER) {
                    rb.transform.position = new Vector3(0f, LOOP_START, 0f);
                    loops++;
                }

            } else if (transform.position.y <= BRAKE_Y && transform.position.y > FINAL_Y) {
                rb.AddForce(0f, BRAKE_FORCE, 0f);
                transform.localEulerAngles = UpdateAngle(transform.position.y - FINAL_Y);
                cam.orthographicSize = UpdateSize(transform.position.y - FINAL_Y);
            } else if (transform.position.y <= FINAL_Y) {
                rb.isKinematic = true;
                transform.position = new Vector3(0f, FINAL_Y, 0f);
                transform.localEulerAngles = new Vector3(30f, 0f, 0f);
                cam.orthographicSize = 8;
            }
        }
    }


    private Vector3 UpdateAngle(float m) {
        return new Vector3(30 - m / 7, 0f, 0f);
    }

    private float UpdateSize(float m) {
        return 8 - m / 42;
    }
}
