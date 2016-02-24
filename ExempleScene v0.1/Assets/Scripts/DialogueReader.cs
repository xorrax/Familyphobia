using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DialogueReader : MonoBehaviour {

    public GameObject dialogueIn;
    Dialogue dialogue;
    public Vector2 padding;
    public QAC current;
    private AudioSource audioSource;
    private int playerChoise;
    private float ySpacing;

    void Start() {
        if (gameObject.GetComponent<AudioSource>() == null) {
            gameObject.AddComponent<AudioSource>();
        }
        audioSource = gameObject.GetComponent<AudioSource>();
        playerChoise = -1;
        dialogue = dialogueIn.GetComponent<Dialogue>();
        current = dialogue.qac[0];
        nextQAC(0);
    }

    void Update() {

        if (this.isActiveAndEnabled) {
            GameObject player = GameObject.Find("Jack");
            player.SendMessage("CanWalk", false);
        }
        if (current.type == dialogueProperty.start) {

            nextQAC(0);

        } else if (current.type == dialogueProperty.player) {

            if (playerChoise > -1) {
                playDialogueSound(playerChoise);
            } else if (audioSource.isPlaying) {
                audioSource.Stop();
            }

        } else if (current.type == dialogueProperty.NPC) {

            playDialogueSound(0);

        } else if (current.type == dialogueProperty.requirement) {
            bool match = false;

            for (int i = 0; i < Inventory.invInstance.itemList.Count; i++) {
                if (current.dialogueItem == Inventory.invInstance.itemList[i]) {
                    match = true;
                    if (current.destroyItem) {
                        Inventory.invInstance.RemoveItem(Inventory.invInstance.itemList[i]);
                    }
                }
            }

            if (match) {
                nextQAC(0);
            } else {
                nextQAC(1);
            }

        } else if (current.type == dialogueProperty.getDialogueItem) {

            Inventory.invInstance.AddItem(current.dialogueItem);

        } else if (current.type == dialogueProperty.end) {
            current = dialogue.qac[0];
            nextQAC(0);
            audioSource.clip = new AudioClip();
            audioSource.Stop();

            if(gameObject.name == "BlueFlower")
                gameObject.SendMessage("setDialogue");

            dialogue = dialogueIn.GetComponent<Dialogue>();
            current = dialogue.qac[0];
            GameObject player = GameObject.Find("Jack");
            player.SendMessage("CanWalk", true);
            this.enabled = false;
        }
    }



    void OnGUI() {

        GUI.Box(new Rect(0,
            Screen.height - ySpacing - 12,
            Screen.width,
            Screen.height),
            "");

        if (current.type == dialogueProperty.player) {

            ySpacing = 12;

            if (playerChoise < 0) {
                for (int i = current.texts.Count - 1; i >= 0; i--) {
                    ySpacing += spacing(current.texts[i]);

                    if (GUI.Button(new Rect(padding.x, Screen.height - ySpacing, Screen.width - padding.x, ySpacing), current.texts[i], "Label")) {
                        playerChoise = i;
                    }
                }
            } else {
                ySpacing = spacing(current.texts[playerChoise]);
                ySpacing += 25;

                GUI.Label(new Rect(padding.x, Screen.height - ySpacing, Screen.width - padding.x, Screen.height), current.texts[playerChoise]);
                if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), "", "Label")) {
                    nextQAC(playerChoise);
                    playerChoise = -1;
                }
            }

        } else if (current.type == dialogueProperty.NPC) {

            ySpacing = spacing(current.texts[0]);
            ySpacing += 25;

            GUI.Label(new Rect(padding.x, Screen.height - ySpacing, Screen.width - padding.x, Screen.height), current.texts[0]);
            if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), "", "Label")) {
                nextQAC(0);
            }
        }
    }

    // Räknar ut längden på en text och anpassar avståndet i Y-led beroende på längden
    float spacing(string text) {

        float textLength = GUI.skin.label.CalcSize(new GUIContent(text)).x;
        float temp = padding.y;

        while (true) {
            if (textLength > Screen.width) {

                temp += 15.4f;
                textLength -= Screen.width;
            } else {
                break;
            }
        }
        return temp;
    }

    void nextQAC(int index) {
        audioSource.Stop();
        current = dialogue.qac[current.next[index]];
    }

    void playDialogueSound(int index) {
        if (current.voices[index] != null && audioSource.clip != current.voices[index]) {
            audioSource.clip = current.voices[index];
            audioSource.Play();
        }
    }

}





//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;


//public class DialogueReader : MonoBehaviour {

//    public GameObject dialogueIn;
//    Dialogue dialogue;
//    public Vector2 padding;
//    public QAC current;
//    private AudioSource audioSource;
//    private int playerChoise;
//    private float ySpacing;

//    void Start() {
//        if (gameObject.GetComponent<AudioSource>() == null) {
//            gameObject.AddComponent<AudioSource>();
//        }
//        audioSource = gameObject.GetComponent<AudioSource>();
//        playerChoise = -1;
//        dialogue = dialogueIn.GetComponent<Dialogue>();
//        current = dialogue.qac[0];
//        nextQAC(0);
//    }

//    void Update() {
        
//        if (current.type == dialogueProperty.start) {

//            nextQAC(0);

//        } else if (current.type == dialogueProperty.player) {

//            if(playerChoise > -1){
//                playDialogueSound(playerChoise);
//            } else if(audioSource.isPlaying){
//                audioSource.Stop();
//            }

//        } else if (current.type == dialogueProperty.NPC) {

//                playDialogueSound(0);

//        } else if (current.type == dialogueProperty.requirement) {
//            bool match = false;

//            for (int i = 0; i < Inventory.invInstance.itemList.Count; i++ ) {
//                if (current.dialogueItem == Inventory.invInstance.itemList[i]) {
//                    match = true;
//                    if (current.destroyItem) {
//                       Inventory.invInstance.RemoveItem(Inventory.invInstance.itemList[i]);
//                    }
//                }
//            }

//            if(match){
//                nextQAC(0);
//            } else {
//                nextQAC(1);
//            }

//        } else if (current.type == dialogueProperty.getDialogueItem) {

//            Inventory.invInstance.AddItem(current.dialogueItem);

//        } else if (current.type == dialogueProperty.end) {
//            current = dialogue.qac[0];
//            nextQAC(0);
//            audioSource.clip = new AudioClip();
//            audioSource.Stop();
//            this.enabled = false;
//        } 
//    }



//    void OnGUI() {

//        GUI.Box(new Rect(0,
//            Screen.height - ySpacing - 12,
//            Screen.width,
//            Screen.height),
//            "");

//        if (current.type == dialogueProperty.player) {

//            ySpacing = 12;

//            if(playerChoise < 0){
//                for (int i = current.texts.Count - 1; i >= 0; i--) {
//                    ySpacing += spacing(current.texts[i]);

//                    if (GUI.Button(new Rect(padding.x, Screen.height - ySpacing, Screen.width - padding.x, ySpacing), current.texts[i], "Label")) {
//                        playerChoise = i;
//                    }
//                }
//            } 
//            else {
//                ySpacing = spacing(current.texts[playerChoise]);
//                ySpacing += 25;

//                GUI.Label(new Rect(padding.x, Screen.height - ySpacing, Screen.width - padding.x, Screen.height), current.texts[playerChoise]);
//                if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), "", "Label")) {
//                    nextQAC(playerChoise);
//                    playerChoise = -1;
//                }
//            }

//        } else if (current.type == dialogueProperty.NPC) {

//            ySpacing = spacing(current.texts[0]);
//            ySpacing += 25;
            
//            GUI.Label(new Rect(padding.x, Screen.height - ySpacing, Screen.width - padding.x, Screen.height), current.texts[0]);
//            if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), "", "Label")) {
//                nextQAC(0);
//            }
//        } 
//    }

//    // Räknar ut längden på en text och anpassar avståndet i Y-led beroende på längden
//    float spacing(string text) {

//        float textLength = GUI.skin.label.CalcSize(new GUIContent(text)).x;
//        float temp = padding.y;

//        while (true) {
//            if (textLength > Screen.width) {

//                temp += 15.4f;
//                textLength -= Screen.width;
//            } else {
//                break;
//            }
//        }
//        return temp;
//    }

//    void nextQAC(int index) {
//        audioSource.Stop();
//        current = dialogue.qac[current.next[index]];
//    }

//    void playDialogueSound(int index) {
//        if (current.voices[index] != null && audioSource.clip != current.voices[index]) {
//            audioSource.clip = current.voices[index];
//            audioSource.Play();
//        }
//    }

//}