using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BubbleType { Speech, Cloud, Scream }

public class SpeechBubbleController : MonoBehaviour {
    private float ALPHA_INC = 0.1f;

    Transform character;
    [SerializeField] SpriteRenderer bubble;
    [SerializeField] TextMeshPro text;
    [SerializeField] Sprite speechBubble;
    [SerializeField] Sprite cloudBubble;
    [SerializeField] Sprite screamBubble;


    Vector3 offset = new Vector3(-0.5f, 2.2f, 0f);
    // Bubble is attached to character
    bool attached = false;
    float fadeTime = 0.25f;


    // Start is called before the first frame update
    void Start() {
        transform.position = character.position + offset;
        transform.rotation = Quaternion.identity;
        UpdateAlpha(0);
    }

    // Update is called once per frame
    void Update() {
        if (attached) transform.position = character.position + offset;
    }

    public void SetBubble(Transform character, string text, float time = 0f, bool attached = false) {
        this.character = character;
        this.text.SetText(text);
        if (time > 0) Destroy(gameObject, time);
        this.attached = attached;
    }

    public void Resize(Vector2 size) {
        bubble.size = size;
        text.rectTransform.sizeDelta = size;
    }

    public void FlipX() {
        bubble.flipX = true;
        text.rectTransform.pivot = new Vector2(1, text.rectTransform.pivot.y);
        offset = new Vector3(-offset.x, offset.y, offset.z);
    }

    public void FadeTime(float time) {
        fadeTime = time;
    }

    public void FontSize(float size) {
        text.fontSize = size;
    }

    //public void AutoClose(float time) {

    //}
    
    public void AttachedBubble() {
        attached = true;
    }

    public void SetBubbleType(BubbleType type) {
        switch (type) {
            case BubbleType.Speech:
                bubble.sprite = speechBubble;
                break;
            case BubbleType.Cloud:
                bubble.sprite = cloudBubble;
                break;
            case BubbleType.Scream:
                bubble.sprite = screamBubble;
                break;
        }
    }

    //public void FlipY() {
    //    bubble.flipY = true;
    //    text.rectTransform.pivot = new Vector2(text.rectTransform.pivot.x, 1);
    //    offset = new Vector3(offset.x, -offset.y, offset.z);
    //}


    public void StartBubble() {
        StartCoroutine(FadeOn(0));
    }

    public void EndBubble() {
        StartCoroutine(FadeOff(1));
    }

    IEnumerator FadeOn(float alpha) {
        alpha += ALPHA_INC;
        UpdateAlpha(alpha);
        yield return new WaitForSeconds(fadeTime*ALPHA_INC);
        if (alpha < 1) StartCoroutine(FadeOn(alpha));
    }

    IEnumerator FadeOff(float alpha) {
        alpha -= ALPHA_INC;
        UpdateAlpha(alpha);
        yield return new WaitForSeconds(fadeTime*ALPHA_INC);
        if (alpha > 0) {
            StartCoroutine(FadeOff(alpha));
        } else {
            Destroy(gameObject);
        }
    }

    void UpdateAlpha(float alpha) {
        Color bubbleColor = bubble.color;
        bubbleColor.a = alpha;
        bubble.color = bubbleColor;
        text.alpha = alpha;
    }

    //public void Settings(bool flipX = false, bool flipY = false) {
    //    sr.flipX = flipX;
    //    sr.flipY = flipY;
    //    if (flipX) offset -= 2 * offset.x * Vector3.right;
    //    if (flipY) offset -= 2 * offset.y * Vector3.up;
    //}

    //public void DestroyBubble(float time = 0f) {
    //    Destroy(gameObject, time);
    //}
}
