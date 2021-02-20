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
    public WeaponsController wc;
    public GameObject[] objectSlots;
    public Image selectedObject;
    public MinimapCameraController minimapCamera;
    public GameObject minimapSmall;
    public GameObject minimapLarge;
    public GameObject pausePanel;
    float y = 0;
    public static HudController instance = null;
    public Image boltReloadImage;

    private CanvasGroup canvGroup = null;

    private void Awake() {
        instance = this;
        canvGroup = GetComponent<CanvasGroup>();
        player = GameManager.instance.player;
        hc = GameManager.instance.hc;
        wc = GameManager.instance.wc;
    }

    // Start is called before the first frame update
    void Start() {
        //player = GameManager.instance.player;
        //hc = GameManager.instance.hc;
    }

    private void FixedUpdate() {
        if (canvGroup.alpha < 1) {
            canvGroup.alpha += 0.05f;
        }
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

        // Reload bolt
        boltReloadImage.transform.parent.gameObject.SetActive(wc.boltReloadProgress < wc.BOLT_RELOAD_TIME);
        boltReloadImage.fillAmount = wc.boltReloadProgress / wc.BOLT_RELOAD_TIME;
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

            if (img != null && txt != null && hc.objs[i] != null && hc.objs[i].obj != null && hc.objs[i].obj.name != null) {
                img.sprite = hc.objs[i].obj.icon;
                txt.text = hc.objs[i].amount.ToString();
                objectSlots[i].SetActive(true);
            }
        }

        selectedObject.gameObject.SetActive(hc.objs.Length > 0);
    }


    public void ResizeMinimap() {
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

    public void Pause(bool pause) {
        pausePanel.SetActive(pause);
    }
}
