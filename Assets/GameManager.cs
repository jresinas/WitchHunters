using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;

    public Trap[] traps = { };
    public Potion[] potions = { };
    public Weapon[] weapons = { };

    public GameObject player;
    public HunterController hc;

    private bool pause = false;

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
        hc = player.GetComponent<HunterController>();
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
    }


    public void LoadScene(string scene) {
        SceneManager.LoadScene(scene);
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

}
