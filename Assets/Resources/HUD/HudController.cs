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
    float y = 0;
    // Start is called before the first frame update
    void Start()
    {
        hc = player.GetComponent<HunterController>();
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
    }
}
