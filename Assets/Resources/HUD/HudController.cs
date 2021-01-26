using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public GameObject player;
    public Image lifeBar;
    public Image staminaBar;
    private HunterController hc;
    float y = 0;
    // Start is called before the first frame update
    void Start()
    {
        hc = player.GetComponent<HunterController>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeBar.fillAmount = hc.life/hc.MAX_LIFE;
        staminaBar.fillAmount = hc.stamina/hc.MAX_STAMINA;

        y -= 0.5f*Time.deltaTime;
        lifeBar.GetComponentInChildren<RawImage>().uvRect = new Rect(0f,y,1f,1f);
        staminaBar.GetComponentInChildren<RawImage>().uvRect = new Rect(0f,y+0.5f,1f,1f);
    }
}
