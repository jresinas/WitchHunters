using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;

    public Trap[] traps = { };
    public Potion[] potions = { };
    public Weapon[] weapons = { };
    public Bolt[] bolts = { };

    public GameObject player;
    public PlayerController pc;
    public PlayerWeaponController wc;
    public PlayerObjectController oc;
    public InputManager input;

    public IScene scene;

    private bool pause = false;
    // 0: input disabled. 1 normal inputs. 2 input limited (need to select option)
    public int inputMode = 0;

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string scene) {
        SceneManager.LoadScene(scene);
    }
     
    public void MakeSelection() {
        scene.MakeSelection();
    }

    public void SelectUp() {
        scene.SelectUp();
    }

    public void SelectDown() {
        scene.SelectDown();
    }

    public void SetPause() {
        pause = !pause;
        Time.timeScale = pause ? 0 : 1;
        SoundManager.instance.Pause(pause);
        HudController.instance.Pause(pause);
    }

    public bool IsPaused() {
        return pause;
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void SetInputMode(int mode) {
        inputMode = mode;
    }
}