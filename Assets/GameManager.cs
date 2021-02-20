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
    public HunterController hc;
    public WeaponsController wc;
    public InputManager input;

    public IScene scene;

    private bool pause = false;
    // 0: input disabled. 1 normal inputs. 2 input limited (need to select option)
    public int inputMode = 0;

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
        hc = player.GetComponent<HunterController>();
        wc = player.GetComponent<WeaponsController>();
        input = player.GetComponent<InputManager>();
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
     

   public void MakeSelection() {
        scene.MakeSelection();
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
