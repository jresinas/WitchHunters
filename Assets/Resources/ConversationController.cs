using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Actor {
    public string name;
    public Transform actor;
}

[System.Serializable]
public class DialogOptions {
    public bool flipX;
    public Vector2 size = new Vector2(5f, 5f);
    public float fadeTime = 0.25f;
    public bool attachedBubble;
    public float fontSize = 4f;
    public BubbleType type = BubbleType.Speech;
}

[System.Serializable]
public class Phrase {
    public string text;
    public string actorName;
    public DialogOptions options;
}

[System.Serializable]
public class Dialog {
    public string name;
    public Phrase[] phrases;
}

//[System.Serializable]
//public class Step {
//    public Dialog[] dialog;
//}


public class ConversationController : MonoBehaviour {
    [SerializeField] GameObject speechBubble;
    public Actor[] actors;
    //public Step[] steps;
    public Dialog[] dialogs;

    [HideInInspector] public bool endConversation = false;

    Dictionary<string, Transform> actorsList = new Dictionary<string, Transform>();
    Dictionary<string, Phrase[]> dialogsList = new Dictionary<string, Phrase[]>();
    Phrase[] phrases;
    int phraseIndex = 0;

    string currentDialog;

    // Avoid pass to next dialog while is fading on/off
    bool isBusy = false;

    SpeechBubbleController newBubble;
    SpeechBubbleController oldBubble;

    void Start() {
        foreach (Actor actor in actors) {
            actorsList[actor.name] = actor.actor;
        }

        foreach (Dialog dialog in dialogs) {
            dialogsList[dialog.name] = dialog.phrases;
        }
    }

    // Allow to add actor transform in runtime
    public void SetActor(string name, Transform actor) {
        if (actorsList.ContainsKey(name)) {
            actorsList[name] = actor;
        } else {
            Debug.Log("Actor name '"+name+"' cannot be found");
        }
    }

    public void StartDialog(string name) {
        currentDialog = name;
        if (dialogsList.ContainsKey(name)) {
            phrases = dialogsList[name];
            phraseIndex = 0;
            endConversation = false;
        } else {
            Debug.Log("Dialog name '"+name+"' cannot be found");
        }
    }

    public void NextDialog(MonoBehaviour caller) {
        if (!endConversation && !isBusy) {
            if (phraseIndex == 0) {
                newBubble = CreateBubble(phrases[phraseIndex]);
                newBubble.StartBubble();
            } else if (phraseIndex == phrases.Length) {
                oldBubble = newBubble;
                newBubble = null;
                StartCoroutine(SwitchBubble());
                endConversation = true;
                caller.SendMessage("FinishConversation", currentDialog);
            } else {
                oldBubble = newBubble;
                newBubble = CreateBubble(phrases[phraseIndex]);
                StartCoroutine(SwitchBubble());
            }
            phraseIndex++;
        } 
    }

    IEnumerator SwitchBubble() {
        isBusy = true;
        if (oldBubble != null) oldBubble.EndBubble();
        yield return new WaitForSeconds(phrases[phraseIndex-1].options.fadeTime);
        //yield return new WaitForSeconds(0.5f);
        if (newBubble != null) newBubble.StartBubble();
        isBusy = false;
    }

    SpeechBubbleController CreateBubble(Phrase phrase) {
        GameObject bubble = Instantiate(speechBubble);
        SpeechBubbleController sbc = bubble.GetComponent<SpeechBubbleController>();
        sbc.SetBubble(actorsList[phrase.actorName], phrase.text);
        if (phrase.options.size != Vector2.zero) sbc.Resize(phrase.options.size);
        if (phrase.options.fadeTime != 0) sbc.FadeTime(phrase.options.fadeTime);
        if (phrase.options.fontSize != 0) sbc.FontSize(phrase.options.fontSize);
        if (phrase.options.flipX) sbc.FlipX();
        if (phrase.options.attachedBubble) sbc.AttachedBubble();
        sbc.SetBubbleType(phrase.options.type);

        return sbc;
    }
}


