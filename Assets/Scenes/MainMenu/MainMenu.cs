using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour, IScene {
    [System.Serializable]
    struct MainMenuOption {
        public string name;
        public Image button;
        public Sprite image;
        public Sprite selectedImage;
    }

    public static MainMenu instance = null;
    [SerializeField] MainMenuOption[] options;
    int currentOption = 0;
    [SerializeField] MainMenuCameraController mainMenuCamera;

    private void Awake() {
        instance = this;
        GameManager.instance.scene = this;
    }

    // Start is called before the first frame update
    void Start(){
        GameManager.instance.SetInputMode(2);
        ChangeOption(0);
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void MakeSelection() {
        switch (options[currentOption].name) {
            case "NewGame":
                mainMenuCamera.StartFalling();
                break;
            case "Options":
                break;
            case "Exit":
                GameManager.instance.ExitGame();
                break;
            default:
                Debug.Log("Option " + options[currentOption].name + " hasn't been defined");
                break;
        }
    }

    public void SelectUp() {
        ChangeOption(-1);
    }

    public void SelectDown() {
        ChangeOption(1);
    }

    void ChangeOption(int change) {
        // Set old selected button as unselected
        if (currentOption < options.Length) options[currentOption].button.sprite = options[currentOption].image;
        // Get new option
        currentOption = currentOption + change;
        if (currentOption < 0) currentOption = options.Length - 1;
        if (currentOption >= options.Length) currentOption = 0;
        // Set current button as selected
        if (currentOption < options.Length) options[currentOption].button.sprite = options[currentOption].selectedImage;
    }
}
