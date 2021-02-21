using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour {
    private float STAMINA_WALK_SPEND = 0.1f;
    private float STAMINA_RUN_SPEND = 1f;

    Rigidbody rb;
    Animator anim;
    PlayerController pc;

    private float walkSpeed = 4f;
    private float runSpeed = 8f;
    // Next movement state (0:idle, 1:walk, 2:run)
    public float nextState = 0f;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        pc = GetComponent<PlayerController>();
    }

    void FixedUpdate() {
        if (!pc.dead) {
            ChangeState();
        }
    }

    public void Idle() {
        nextState = 0f;
    }

    // Movement and rotation controller
    public void Move(Vector3 move, Vector3 view, bool run) {
        // If no view vector, character look at movement direction
        if (view == Vector3.zero) {
            view = move;
        }

        // Rotate character to view vector direction
        transform.LookAt(transform.position + view);
        //transform.LookAt(transform.position + view - new Vector3(0,0,-0.5f));

        // Set MoveRotation animation parameter in function of move and view vectors
        // MoveRotation controls foot blend animations (WalkBlend:WalkBack/WalkLeft/WalkForward/WalkRight and RunBlend:RunBack/RunLeft/RunForward/RunRight)
        Vector3 aux = Vector3.Cross(move, view);
        int sign = (aux.y < 0) ? 1 : -1;
        float angle = Vector3.Angle(move, view);
        anim.SetFloat("MoveRotation", sign * angle / 90);

        if (move != Vector3.zero) {
            if (!run || (nextState == 2 && pc.stamina <= 0.1) || (nextState != 2 && pc.stamina < STAMINA_RUN_SPEND / 4)) {
                // Walk
                nextState = 1f;
                rb.MovePosition(rb.position + move * walkSpeed * Time.deltaTime);
                pc.UpdateStamina(STAMINA_WALK_SPEND, true);
            } else {
                // Run
                nextState = 2f;
                rb.MovePosition(rb.position + move * runSpeed * Time.deltaTime);
                pc.UpdateStamina(-STAMINA_RUN_SPEND, true);
            }
        } else {
            // Idle
            Idle();
        }
    }

    // Set param State gradually to make smooth transitions between states (idle, walk and run)
    private void ChangeState() {
        anim.SetFloat("StateArm", nextState);
        if (anim.GetFloat("StateFoot") - 0.1f <= nextState) {
            anim.SetFloat("StateFoot", anim.GetFloat("StateFoot") + 0.1f);
        }
        if (anim.GetFloat("StateFoot") + 0.1f >= nextState) {
            anim.SetFloat("StateFoot", anim.GetFloat("StateFoot") - 0.1f);
        }
    }

    private void Step(AnimationEvent evt) {
        if (evt.animatorClipInfo.weight > 0.4) {
            SoundManager.instance.Play("Step", pc.audioFoots);
        }
    }
}