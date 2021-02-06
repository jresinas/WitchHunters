using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public GameObject player;
    public Image lifeOrb;
    public Image staminaOrb;
    private HunterController hc;
    public Text[] trapsNumber;
    public Image selectedTrap;
    private RectTransform selectedTrapFrame;
    public MinimapCameraController minimapCamera;
    public GameObject minimapSmall;
    public GameObject minimapLarge;
    public GameObject pausePanel;
    public static bool pause = false;
    float y = 0;
    // Start is called before the first frame update
    void Start()
    {
        hc = player.GetComponent<HunterController>();
        selectedTrapFrame = selectedTrap.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update orbs fill
        lifeOrb.fillAmount = hc.life/hc.MAX_LIFE;
        staminaOrb.fillAmount = hc.stamina/hc.MAX_STAMINA;

        // Animate orbs
        y -= 0.5f*Time.deltaTime;
        lifeOrb.GetComponentInChildren<RawImage>().uvRect = new Rect(0f,y,1f,1f);
        staminaOrb.GetComponentInChildren<RawImage>().uvRect = new Rect(0f,y+0.5f,1f,1f);

        // Update traps number
        for (int i = 0; i< trapsNumber.Length; i++) {
            trapsNumber[i].text = hc.trapsNumber[i].ToString();
        }

        // Update selected trap
        selectedTrap.rectTransform.anchoredPosition = new Vector3(600f + 100f * hc.selectedTrap, selectedTrap.rectTransform.anchoredPosition.y);

        // Switch minimap size
        if (Input.GetButtonDown("ResizeMinimap")) {
            if (minimapCamera.size == 0) {
                minimapCamera.size = 1;
                minimapSmall.SetActive(false);
                minimapLarge.SetActive(true);
            } else {
                minimapCamera.size = 0;
                minimapSmall.SetActive(true);
                minimapLarge.SetActive(false);
            }

        }

        // Pause Game
        if (Input.GetButtonDown("Pause")) {
            pause = !pause;
            pausePanel.SetActive(pause);
            Time.timeScale = pause? 0 : 1;
            SoundManager.instance.Pause(pause);
        }
    }
}
