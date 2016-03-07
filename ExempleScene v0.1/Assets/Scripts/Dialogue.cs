using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum dialogueProperty { start, player, NPC, requirement, getDialogueItem, warp, empty, end }

[ExecuteInEditMode]
public class Dialogue : MonoBehaviour {

    public List<QAC> qac = new List<QAC>();
    public List<int> attachedWindows = new List<int>();
    public List<int> attachedStrings = new List<int>();

    void Start() {
        DestroyImmediate(gameObject);
    }
}

[System.Serializable]
public class QAC {

    public int id;
    public string name;
    public dialogueProperty type;
    public List<string> texts;
    public List<int> next;
    public GameObject dialogueItem;
    public bool destroyItem;
    public List<AudioClip> voices;

    public Rect rect;
    public List<Rect> outputs;


    public QAC() {
        id = 0;
        name = "";
        type = dialogueProperty.NPC;
        texts = new List<string>();
        next = new List<int>();
        voices = new List<AudioClip>();
        destroyItem = false;

        rect = new Rect();
        outputs = new List<Rect>();
    }
}