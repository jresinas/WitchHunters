using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public GameObject player;
    public Image lifeOrb;
    public Image staminaOrb;
    public HunterController hc;
    public GameObject[] objectSlots;
    public Image selectedObject;
    public MinimapCameraController minimapCamera;
    public GameObject minimapSmall;
    public GameObject minimapLarge;
    public GameObject pausePanel;
    public static bool pause = false;
    float y = 0;
    public static HudController instance = null;
    public Image boltReloadImage;

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
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
        for (int i = 0; i< hc.objs.Length; i++) {
            if (hc.objs[i] != null && objectSlots[i] != null) {
                objectSlots[i].GetComponentInChildren<Text>().text = hc.objs[i].amount.ToString();
            }
        }

        // Update selected trap
        selectedObject.rectTransform.anchoredPosition = new Vector3(110f * hc.selectedObj, selectedObject.rectTransform.anchoredPosition.y);

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

        // Reload bolt
        boltReloadImage.transform.parent.gameObject.SetActive(hc.boltReloadProgress < hc.BOLT_RELOAD_TIME);
        boltReloadImage.fillAmount = hc.boltReloadProgress / hc.BOLT_RELOAD_TIME;
    }

    public void RefreshObjectSlots() {
        Image img = null;
        Text txt = null;

        foreach (GameObject slot in objectSlots) {
            slot.SetActive(false);
        }

        for (int i = 0; i < hc.objs.Length; i++) {
            img = objectSlots[i].GetComponent<Image>();
            txt = objectSlots[i].GetComponentInChildren<Text>();

            if (img != null && txt != null && hc.objs[i] != null && hc.objs[i].obj != null) {
                img.sprite = hc.objs[i].obj.icon;
                txt.text = hc.objs[i].amount.ToString();
                objectSlots[i].SetActive(true);
            }
        }

        selectedObject.gameObject.SetActive(hc.objs.Length > 0);
    }
}
