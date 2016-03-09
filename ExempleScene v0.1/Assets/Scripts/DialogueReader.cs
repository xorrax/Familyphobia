using UnityEngine;
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
    Color cJack, cBek, cLinda, cBee, cShannon;
    public static Animator aJack, aBek, aLinda, aBee;

    private Pathfinding pf;
    private Vector3 prewPlayerPos;

    private Vector3 prewCameraPos;

    private float clipTimePlayed;

    Test test = new Test();

    void OnLevelWasLoaded(int scene) {

        if (scene != 0) {
            if (gameObject.GetComponent<AudioSource>() == null) {
                gameObject.AddComponent<AudioSource>();
            }

            if (GameObject.Find("Linda")/*.GetComponent<Animator>() */!= null) {
                aLinda = GameObject.Find("Linda").GetComponent<Animator>();
                Debug.Log("Linda Not NUll");
            }
            if (GameObject.Find("Bek")/*.GetComponent<Animator>()*/ != null) {
                aBek = GameObject.Find("Bek").GetComponent<Animator>();
                Debug.Log("Bek Not NUll");
            }
            if (GameObject.Find("Bee")/*.GetComponent<Animator>()*/ != null) {
                aBee = GameObject.Find("Bee").GetComponent<Animator>();
                Debug.Log("Bee Not NUll");
            }

            padding.x = 20;
            fixedPosition = false;
            cJack = new Vector4(0.278f, 0.419f, 0.259f, 1);
            cBek = new Vector4(0.835f, 0.71f, 0.643f, 1);
            cLinda = new Vector4(0.988f, 0.459f, 0, 1);
            cBee = new Vector4(0.796f, 0.847f, 0.055f, 1);
            cShannon = new Vector4(0.420f, 0.450f, 0.714f, 1);

            pf = aJack.GetComponent<Pathfinding>();
            // audioSource = gameObject.GetComponent<AudioSource>();
            audioSource = GameObject.Find("Dialouge").GetComponent<AudioSource>();
            playerChoice = -1;
            dialogue = dialogueIn.GetComponent<Dialogue>();
            current = dialogue.qac[0];
        }
    }
    IEnumerator playAnimation(Test test) {
            test.anim.SetFloat("dB", 1f);
            yield return new WaitForSeconds(test.time);
            clipTimePlayed = 0;
            test.anim.SetFloat("dB", 0);
            test.done = true;
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
            style.normal.textColor = cJack;
            if (aJack != null) {
                if (audioSource.clip != null) {
                    if (!test.done) {
                        test.anim = aJack;
                        test.time = audioSource.clip.length;
                        StartCoroutine("playAnimation", test);
                    }
                }
            }
        } else if (current.name == "Linda") {
            style.normal.textColor = cLinda;
            if (aLinda != null) {
                if (audioSource.clip != null) {
                    if (!test.done) {
                        test.anim = aLinda;
                        test.time = audioSource.clip.length;
                        StartCoroutine("playAnimation", test);
                    }
                }
            }
        } else if (current.name == "Bek") {
            style.normal.textColor = cBek;
            if (aBek != null) {
                if (audioSource.clip != null) {
                    if (!test.done) {
                        test.anim = aBek;
                        test.time = audioSource.clip.length;
                        StartCoroutine("playAnimation", test);
                    }
                   
                }
            }
        } else if (current.name == "Shannon") {
            style.normal.textColor = cShannon;
           // if (aShannon != null) {
                if (audioSource.clip != null) {
                    if (!test.done) {
                        //test.anim = aShannon;
                        test.time = audioSource.clip.length;
                        StartCoroutine("playAnimation", test);
                    }

                }
           // }
        } else if (current.name == "Bee") {
            style.normal.textColor = cBee;
            if (aBee != null) {
                if (audioSource.clip != null) {
                    if (!test.done) {
                        test.anim = aBee;
                        test.time = audioSource.clip.length;
                        StartCoroutine("playAnimation", test);
                    }

                }
            }
        } 
        else {
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
        if (test.anim != null) {
            test.anim.SetFloat("dB", 0f);
        }
        test = new Test();
    }

    void playDialogueSound(int index) {
        if (current.voices[index] != null && audioSource.clip != current.voices[index]) {
            audioSource.clip = current.voices[index];
            audioSource.Play();
        }
    }
}

public class Test{

    public Animator anim;
    public float time;
    public bool done;

    public Test() {
        done = false;
    }

}