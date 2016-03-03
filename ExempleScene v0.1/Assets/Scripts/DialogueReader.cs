﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DialogueReader : MonoBehaviour {

    public static float scaling = 1;
    public GameObject dialogueIn;
    public Vector3 newPlayerPos;
    private Vector2 padding;
    public QAC current;
    public Font myFont;
    public bool fixedPosition;

    private AudioSource audioSource;
    private int playerChoice;
    private float ySpacing;
    Dialogue dialogue;
    Color jack, beck, linda;

    private Pathfinding pf;
    private Vector3 prewPlayerPos;
    private Vector3 prewCameraPos;

    void Start() {
        if (gameObject.GetComponent<AudioSource>() == null) {
            gameObject.AddComponent<AudioSource>();
        }

        padding.x = 20;
        fixedPosition = false;
        jack = new Vector4(0.278f, 0.419f, 0.259f, 1);
        beck = new Vector4(0.835f, 0.71f, 0.643f, 1);
        linda = new Vector4(0.988f, 0.459f, 0, 1);
        pf = GameObject.FindGameObjectWithTag("Player").GetComponent<Pathfinding>();
        audioSource = gameObject.GetComponent<AudioSource>();
        playerChoice = -1;
        dialogue = dialogueIn.GetComponent<Dialogue>();
        current = dialogue.qac[0];
    }

    void Update() {

        if (current.type == dialogueProperty.start) {
            if (fixedPosition) {
                prewPlayerPos = pf.transform.position;
                pf.transform.position = newPlayerPos;
                pf.Target = newPlayerPos;
            }

            pf.endPathfinding();
            pf.setIsActive(false);
            nextQAC(0);

        } else if (current.type == dialogueProperty.player) {

            if (playerChoice > -1) {
                playDialogueSound(playerChoice);
            } else if (audioSource.isPlaying) {
                audioSource.Stop();
            }

        } else if (current.type == dialogueProperty.NPC) {

            playDialogueSound(0);

        } else if (current.type == dialogueProperty.requirement) {
            bool match = false;

            for (int i = 0; i < Player.dialogueObjects.Count; i++) {
                if (current.dialogueItem == Player.dialogueObjects[i]) {
                    match = true;
                    if (current.destroyItem) {
                        Inventory.invInstance.RemoveItem(Player.dialogueObjects[i]);
                    }
                }
            }

            if (match) {
                nextQAC(0);
            } else {
                nextQAC(1);
            }

        } else if (current.type == dialogueProperty.getDialogueItem) {

            Player.dialogueObjects.Add(current.dialogueItem);
            nextQAC(0);
        } else if (current.type == dialogueProperty.warp) {
            warpToScene newSceneWarp;
            if (GetComponent<warpToScene>() != null) {
                gameObject.AddComponent<warpToScene>();
            }
            newSceneWarp = GetComponent<warpToScene>();

            current = dialogue.qac[0];
            audioSource.clip = new AudioClip();
            audioSource.Stop();
            pf.setIsActive(true);
            if (fixedPosition) {
                pf.transform.position = prewPlayerPos;
                pf.Target = prewPlayerPos;
            }
            this.enabled = false;
            newSceneWarp.LoadScene(newSceneWarp.nameOfScene);

        } else if (current.type == dialogueProperty.end) {
            current = dialogue.qac[0];
            audioSource.clip = new AudioClip();
            audioSource.Stop();
            pf.setIsActive(true);
            if (fixedPosition) {
                pf.transform.position = prewPlayerPos;
                pf.Target = prewPlayerPos;
            }
            this.enabled = false;
        }
    }


    void OnGUI() {

        var style = GUI.skin.GetStyle("Label");
        style.font = myFont;
        padding.y = Mathf.RoundToInt(40 * scaling);
        style.fontSize = Mathf.RoundToInt(30 * scaling);
        if (current.name == "Jack") {
            style.normal.textColor = jack;
        } else if (current.name == "Linda") {
            style.normal.textColor = linda;
        } else if (current.name == "Beck") {
            style.normal.textColor = beck;
        } else {
            style.normal.textColor = Color.white;
        }

        GUI.Box(new Rect(0,
            Screen.height - ySpacing - 12,
            Screen.width,
            Screen.height),
            "");

        // Multiple choice

        if (current.type == dialogueProperty.player) {

            ySpacing = 12;

            if (playerChoice < 0) {
                for (int i = current.texts.Count - 1; i >= 0; i--) {
                    ySpacing += spacing(current.texts[i]);

                    Rect shrekt = new Rect(padding.x, Screen.height - ySpacing, Screen.width - padding.x, ySpacing - 10);

                    Color colorSave = style.normal.textColor;
                    if (i < current.texts.Count - 1) {
                        float prewSpacing = spacing(current.texts[i + 1]);
                        shrekt = new Rect(padding.x, Screen.height - ySpacing, Screen.width - padding.x, prewSpacing - 5);
                    }

                    if (shrekt.Contains(new Vector2(Event.current.mousePosition.x, Event.current.mousePosition.y))) {
                        style.normal.textColor /= 1.5f;
                    }

                    if (GUI.Button(shrekt, current.texts[i], "Label")) {
                        playerChoice = i;
                    }
                    style.normal.textColor = colorSave;
                }

            } else {
                ySpacing = spacing(current.texts[playerChoice]);
                ySpacing += 25;

                GUI.Label(new Rect(padding.x, Screen.height - ySpacing, Screen.width - padding.x, Screen.height), current.texts[playerChoice]);
                if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), "", "Label")) {
                    nextQAC(playerChoice);
                    playerChoice = -1;
                }
            }

            // One Line

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