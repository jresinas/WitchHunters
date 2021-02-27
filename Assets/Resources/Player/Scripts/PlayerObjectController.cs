using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectController : MonoBehaviour {
    //private float PICK_DISTANCE = 1.8f;
    private float PUT_DISTANCE = 1f;

    Animator anim;
    PlayerController pc;

    // List of inventory objects
    public PlayerObject[] objs;
    // Current inventory object selected
    public int selectedObj = 0;

    // Trap selected to pick from the floor
    private IInteractive trapToPick = null;

    private void Awake() {
        pc = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }


    public void UseObject() {
        if (objs[selectedObj] != null && objs[selectedObj].amount > 0) {
            if (objs[selectedObj].isTrap()) {
                PutTrap();
            } else if (objs[selectedObj].isPotion()) {
                DrinkPotion();
            } else {
                Debug.Log("Object type undefined");
            }
        }
    }

    private void PutTrap() {
        anim.SetBool("PutTrap", true);
    }

    private void DrinkPotion() {

    }

    private void PutTrapCallback() {
        Vector3 offset = transform.forward * PUT_DISTANCE;

        if (objs[selectedObj] != null && objs[selectedObj].isTrap() && objs[selectedObj].amount > 0) {
            GameObject trap = ((Trap)objs[selectedObj].obj).prefab;

            if (trap != null) {
                Instantiate(trap, transform.position + offset, Quaternion.identity);
                objs[selectedObj].amount--;
            }
        }
    }

    public void PickTrap(IInteractive trap) {
        trapToPick = trap;
        anim.SetBool("PickTrap", true);
    }

    private void PickTrapCallback() {
        if (trapToPick != null) {
            trapToPick.Interact(pc);
            trapToPick = null;
        }
    }

    public void ChangeSelectedObject(int num) {
        if (!anim.GetBool("PutTrap")) {
            selectedObj = selectedObj + num;
            if (selectedObj < 0) {
                selectedObj = objs.Length - 1;
            }
            if (selectedObj >= objs.Length) {
                selectedObj = 0;
            }
            SoundManager.instance.Play("SelectObject", pc.audioHands);
        }
    }

    

    public void StopPickTrap() {
        anim.SetBool("PutTrap", false);
        anim.SetBool("PickTrap", false);
        trapToPick = null;
    }

    public void StopPutTrap() {
        throw new NotImplementedException();
    }
}
