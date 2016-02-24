using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;


public class DialogueEditor : EditorWindow {

    public List<DialogueNode> nodes = new List<DialogueNode>();

    public List<int> windowsToAttach = new List<int>();
    public List<int> stringsToAttach = new List<int>();
    public List<int> attachedWindows = new List<int>();
    public List<int> attachedStrings = new List<int>();
    
    public float clickTimer;
    public string newName;
    public int clicks;

    Vector2 positonOffset = new Vector2(0, 0);
    float lx = 0;
    float ly = 0;

    string save = "Test/filename";
    GameObject load;

    [MenuItem("Window/DilogueEditor")]
    static void ShowEditor() {

        DialogueEditor editor = EditorWindow.GetWindow<DialogueEditor>();

    }

    void Awake() {
        this.position = new Rect(0, 0, 500, 500);
        load = null;
        nodes.Add(new StartNode(this));
        nodes.Add(new EndNode(this));
        clickTimer = 0;
        newName = "";
        clicks = 0;
    }

    void OnGUI() {

        /*string temp = "";
        for (int i = 0; i < attachedWindows.Count; i += 2) {
            temp = temp + attachedWindows[i].ToString() + ",";
            temp = temp + attachedWindows[i + 1].ToString() + " | ";
        }

        GUILayout.TextArea(temp);

        string temp2 = "";
        for (int i = 0; i < attachedWindows.Count; i += 2) {
            temp2 = temp2 + attachedStrings[i].ToString() + ",";
            temp2 = temp2 + attachedStrings[i + 1].ToString() + " | ";
        }

        GUILayout.TextArea(temp2);
        GUILayout.TextArea(nodes.Count.ToString());*/

        if (clickTimer <= 10) {
            clickTimer += 0.05f;
        } else {
            clicks = 0;
            clickTimer = 0;
            Debug.Log("Tick");
        }

        if (Event.current.button == 2) {
            for (int i = 0; i < nodes.Count; i++) {
                nodes[i].rect.x += Event.current.mousePosition.x - lx;
                nodes[i].rect.y += Event.current.mousePosition.y - ly;
            }
        }

        if (windowsToAttach.Count == 1) {
            DrawNodeCurve(nodes[windowsToAttach[0]].outputRect(stringsToAttach[0]), new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 1, 1));
            if (Event.current.button == 1) {
                windowsToAttach = new List<int>();
                stringsToAttach = new List<int>();
            }
        }

        // Connects nodes
        if (windowsToAttach.Count == 2) {

            attachedWindows.Add(windowsToAttach[0]);
            attachedWindows.Add(windowsToAttach[1]);

            nodes[windowsToAttach[0]].next[stringsToAttach[0]] = windowsToAttach[1];

            attachedStrings.Add(stringsToAttach[0]);
            attachedStrings.Add(stringsToAttach[1]);

            windowsToAttach = new List<int>();
            stringsToAttach = new List<int>();
        }

        // Draw curves
        if (attachedWindows.Count >= 2) {
            for (int i = 0; i < attachedWindows.Count; i += 2) {
                DrawNodeCurve(nodes[attachedWindows[i]].outputRect(attachedStrings[i]), nodes[attachedWindows[i + 1]].inputRect());
                Repaint();
            }
        }

        // Everything will be saved in a separet class whitch will be saved inside a prefab
        if (GUILayout.Button("Save")) {
            saveDialogue();
        }
        save = GUILayout.TextArea(save);

        if (GUILayout.Button("Load")) {
            loadDialogue();
        }
        load = EditorGUILayout.ObjectField(load, typeof(GameObject), true) as GameObject;

        if (GUILayout.Button("Create NPC")) {
            nodes.Add(new NPCNode(this));
        }
        
        if (GUILayout.Button("Create Player")) {
            nodes.Add(new PlayerNode(this));
        }

        if (GUILayout.Button("Create Requirement")) {
            nodes.Add(new RequirementNode(this));
        }
        if (GUILayout.Button("Create Item")) {
            nodes.Add(new GiveItemNode(this));
        }

        BeginWindows();
        for (int i = 0; i < nodes.Count; i++) {
            nodes[i].rect = GUI.Window(i, nodes[i].rect, nodes[i].DrawNodeWindow, nodes[i].name);
        }
        EndWindows();

        Repaint();
        lx = Event.current.mousePosition.x;
        ly = Event.current.mousePosition.y;
    }
    void DrawNodeCurve(Rect start, Rect end) {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);

        for (int i = 0; i < 3; i++) {// Draw a shadow
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        }
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.red, null, 1);
    }

    void loadDialogue() {
        if(load != null){
            for (int i = nodes.Count - 1; i >= 0; i--) {
                nodes.RemoveAt(i);
                Debug.Log(nodes.Count);
            }
            Dialogue d = load.GetComponent<Dialogue>();
            attachedWindows = d.attachedWindows;
            attachedStrings = d.attachedStrings;
            for (int i = 0; i < d.qac.Count; i++ ) {
                if (d.qac[i].type == dialogueProperty.start) {
                    nodes.Add(new StartNode(this));
                } else if (d.qac[i].type == dialogueProperty.end) {
                    nodes.Add(new EndNode(this));
                } else if (d.qac[i].type == dialogueProperty.NPC) {
                    nodes.Add(new NPCNode(this));
                } else if (d.qac[i].type == dialogueProperty.player) {
                    nodes.Add(new PlayerNode(this));
                } else if (d.qac[i].type == dialogueProperty.requirement) {
                    nodes.Add(new RequirementNode(this));
                } else if (d.qac[i].type == dialogueProperty.getDialogueItem) {
                    nodes.Add(new GiveItemNode(this));
                }
                nodes[i].type = d.qac[i].type;
                nodes[i].texts = d.qac[i].texts;
                nodes[i].next = d.qac[i].next;
                nodes[i].voices = d.qac[i].voices;
                nodes[i].dialogueItem = d.qac[i].dialogueItem;
                nodes[i].destroyItem = d.qac[i].destroyItem;
                nodes[i].outputs = d.qac[i].outputs;
                nodes[i].rect = d.qac[i].rect;
            }
            load = null;
        }
    }
    void saveDialogue() {
        GameObject go = new GameObject();
        Dialogue d = go.AddComponent<Dialogue>();
        d.attachedWindows = attachedWindows;
        d.attachedStrings = attachedStrings;
        for (int i = 0; i < nodes.Count; i++) {

            d.qac.Add(new QAC());
            d.qac[i].id = i;
            d.qac[i].type = nodes[i].type;
            d.qac[i].texts = nodes[i].texts;
            d.qac[i].next = nodes[i].next;
            d.qac[i].voices = nodes[i].voices;
            d.qac[i].dialogueItem = nodes[i].dialogueItem;
            d.qac[i].destroyItem = nodes[i].destroyItem;
            d.qac[i].outputs = nodes[i].outputs;
            d.qac[i].rect = nodes[i].rect;
        }

        var prefab = PrefabUtility.CreateEmptyPrefab("Assets/" + save + ".prefab");
        PrefabUtility.ReplacePrefab(go, prefab, ReplacePrefabOptions.ReplaceNameBased);
    }

}

// OBS! Skapa aldrig denna bas klass
public class DialogueNode {

    public Rect rect;
    public Rect input;
    public List<Rect> outputs;
    public List<string> texts;
    public List<AudioClip> voices;
    public List<int> next;
    public string name;
    public GameObject dialogueItem;
    public bool destroyItem;
    public dialogueProperty type;
    public DialogueEditor script;

    public DialogueNode() {
        type = dialogueProperty.NPC;
        texts = new List<string>();
        outputs = new List<Rect>();
        voices = new List<AudioClip>();
        next = new List<int>();
        

        rect = new Rect(10, 300, 200, 150);
        input = new Rect(0, 18, 30, 18);
        name = "Node";
    }

    public virtual void DrawNodeWindow(int id) {
        
        if (GUI.Button(new Rect(rect.width / 4, 0, rect.width / 2, 16), "DoubleClickMe")) {
            script.clicks++;
            if (script.clicks > 1) {
                Debug.Log("Double click");
                script.clicks = 0;
            }
        }

        if (GUI.Button(new Rect(rect.width - 18, 0, 18, 16), "x")) {
            for (int i = 0; i < next.Count; i++) {
                removeOutputLinks(id, i);
            }
            removeInputLinks(id, 0);
            script.nodes[id] = new Empty();
        }

        if (GUI.Button(input, "In")) {
            if (script.windowsToAttach.Count == 1) {
                createLink(id, 0);
            }
            if (Event.current.button == 1) {
                removeInputLinks(id, 0);
            }
        }
    }

    public Rect inputRect() {
        Rect shrekt = new Rect(rect.x, rect.y + input.y, rect.width, input.height);
        return shrekt;
    }

    public Rect outputRect(int index) {
        Rect shrekt = new Rect(rect.x, rect.y + outputs[index].y, rect.width, outputs[index].height);
        return shrekt;
    }

    public void createLink(int i, int j) {
        if (Event.current.button == 0) {
            script.windowsToAttach.Add(i);
            script.stringsToAttach.Add(j);
        }
    }

    public void removeInputLinks(int idIndex, int idString) {
        for (int i = 1; i < script.attachedWindows.Count; i += 2) {
            if (idIndex == script.attachedWindows[i]) {
                script.attachedWindows.RemoveAt(i);
                script.attachedStrings.RemoveAt(i);
                script.attachedWindows.RemoveAt(i - 1);
                script.attachedStrings.RemoveAt(i - 1);
                next[idString] = -1;
                i = 1;
            }
        }
    }

    public void removeOutputLinks(int idIndex, int idString) {
        for (int i = 0; i < script.attachedWindows.Count; i += 2) {
            if (idIndex == script.attachedWindows[i] && idString == script.attachedStrings[i]) {
                script.attachedWindows.RemoveAt(i + 1);
                script.attachedStrings.RemoveAt(i + 1);
                script.attachedWindows.RemoveAt(i);
                script.attachedStrings.RemoveAt(i);
                next[idString] = -1;
                i = 0;
            }
        }
    }
}


public class NPCNode : DialogueNode {
    Vector2 scroll;
    public NPCNode(DialogueEditor script) {
        rect = new Rect(50, 300, 200, 100);
        outputs.Add(new Rect(170, 18, 30, 18));
        texts.Add("");
        voices.Add(new AudioClip());
        name = "NPC";
        next.Add(new int());

        this.script = script;
        scroll = new Vector2(0, 0);
    }

    public override void DrawNodeWindow(int id) {
        base.DrawNodeWindow(id);
        GUILayout.Label("");

        if (GUI.Button(outputs[0], "Out")) {
            if (script.windowsToAttach.Count < 1) {
                createLink(id, 0);
            }
            if (Event.current.button == 1) {
                removeOutputLinks(id, 0);
            }
        }

        for (int i = 0; i < texts.Count; i++) {
            scroll = EditorGUILayout.BeginScrollView(scroll);
            texts[i] = GUILayout.TextArea(texts[i]);
            voices[i] = EditorGUILayout.ObjectField(voices[i], typeof(AudioClip), true) as AudioClip;
            EditorGUILayout.EndScrollView();
        }
        GUI.DragWindow();
    }
}

public class PlayerNode : DialogueNode {
    public PlayerNode(DialogueEditor script) {
        rect = new Rect(50, 300, 200, 100);
        name = "Player";
        type = dialogueProperty.player;
        next.Add(new int());

        this.script = script;
        texts.Add("");
        voices.Add(new AudioClip());
        outputs.Add(new Rect(170, 60, 30, 34));
    }

    public override void DrawNodeWindow(int id) {
        base.DrawNodeWindow(id);
        GUILayout.Label("");
        GUILayout.Label("");

        if (GUI.Button(new Rect(0, 36, 100, 18), "Add Line")) {
            rect = new Rect(rect.x, rect.y, rect.width, rect.height + 36);
            texts.Add("");
            next.Add(-1);
            voices.Add(new AudioClip());
            outputs.Add(new Rect(170, 60 + ((texts.Count - 1) * 36), 30, 34));
        }

        if (GUI.Button(new Rect(100, 36, 100, 18), "Remove Line")) {
            if (outputs.Count > 1) {
                removeOutputLinks(id, outputs.Count - 1);
                rect = new Rect(rect.x, rect.y, rect.width, rect.height - 36);
                next.RemoveAt(next.Count - 1);
                texts.RemoveAt(texts.Count - 1);
                voices.RemoveAt(voices.Count - 1);
                outputs.RemoveAt(outputs.Count - 1);
            }
        }

        for (int i = 0; i < texts.Count; i++) {
            texts[i] = GUILayout.TextField(texts[i], GUILayout.Width(165));
            voices[i] = EditorGUILayout.ObjectField(voices[i], typeof(AudioClip), true, GUILayout.Width(165)) as AudioClip;
            if (GUI.Button(outputs[i], "Out")) {
                if (script.windowsToAttach.Count < 1) {
                    createLink(id, i);
                }
                if (Event.current.button == 1) {
                    removeOutputLinks(id, i);
                }
            }
        }
        GUI.DragWindow();
    }
}

public class RequirementNode : DialogueNode {

    public RequirementNode(DialogueEditor script) {
        rect = new Rect(50, 300, 200, 80);
        outputs.Add(new Rect(165, 27, 35, 18));
        outputs.Add(new Rect(165, 45, 35, 18));
        type = dialogueProperty.requirement;
        destroyItem = false;
        texts.Add("");
        texts.Add("");
        name = "Requirement";
        this.script = script;
        next.Add(-1);
        next.Add(-1);
    }

    public override void DrawNodeWindow(int id) {
        base.DrawNodeWindow(id);
        GUILayout.Label("");

        dialogueItem = EditorGUILayout.ObjectField(dialogueItem, typeof(GameObject), true, GUILayout.Width(160)) as GameObject;
        destroyItem = GUILayout.Toggle(destroyItem, "Destory");

        if (GUI.Button(outputs[0], "If")) {
            if (script.windowsToAttach.Count < 1) {
                createLink(id, 0);
            }
            removeOutputLinks(id, 0);
        }

        if (GUI.Button(outputs[1], "Else")) {
            if (script.windowsToAttach.Count < 1) {
                createLink(id, 1);
            }
            removeOutputLinks(id, 1);
        }

        GUI.DragWindow();
    }
}

public class GiveItemNode : DialogueNode {
    public GiveItemNode(DialogueEditor script) {
        rect = new Rect(50, 300, 200, 70);
        outputs.Add(new Rect(170, 18, 30, 18));
        type = dialogueProperty.getDialogueItem;
        texts.Add("");
        voices.Add(new AudioClip());
        name = "Give item";
        next.Add(new int());

        this.script = script;
    }

    public override void DrawNodeWindow(int id) {
        base.DrawNodeWindow(id);
        GUILayout.Label("");

        if (GUI.Button(outputs[0], "Out")) {
            if (script.windowsToAttach.Count < 1) {
                createLink(id, 0);
            }
            if (Event.current.button == 1) {
                removeOutputLinks(id, 0);
            }
        }

        dialogueItem = EditorGUILayout.ObjectField(dialogueItem, typeof(GameObject), true) as GameObject;

        GUI.DragWindow();
    }
}

public class StartNode : DialogueNode {

    public StartNode(DialogueEditor script) {
        rect = new Rect(0, Screen.height / 2, 80, 40);
        outputs.Add(new Rect(0, 18, 80, 22));
        type = dialogueProperty.start;
        name = "Start";
        texts.Add("");
        voices.Add(new AudioClip());

        this.script = script;
        next.Add(new int());
    }

    public override void DrawNodeWindow(int id) {
        if (GUI.Button(outputs[0], "Out")) {
            if (script.windowsToAttach.Count < 1) {
                createLink(id, 0);
                removeOutputLinks(id, 0);
            }
        }

        GUI.DragWindow();
    }
}
public class EndNode : DialogueNode {

    public EndNode(DialogueEditor script) {
        rect = new Rect(Screen.width - 80, Screen.height / 2, 80, 40);
        input = new Rect(0, 18, 80, 22);
        outputs.Add(new Rect(0, 18, 80, 22));
        texts.Add("");
        voices.Add(new AudioClip());
        type = dialogueProperty.end;
        name = "End";
        this.script = script;
        next.Add(new int());
    }

    public override void DrawNodeWindow(int id) {
        if (GUI.Button(input, "In")) {
            if (script.windowsToAttach.Count == 1) {
                createLink(id, 0);
            }
        }
        GUI.DragWindow();
    }
}

public class Empty : DialogueNode {
    public Empty() {
        rect = new Rect(-20, 0, 0, 0);
        input = new Rect(0, 0, 0, 0);
        outputs.Add(new Rect(0, 0, 0, 0));
        name = "";

    }
    public override void DrawNodeWindow(int id) {
        rect = new Rect(-20, 0, 0, 0);

        GUI.DragWindow();
    }
}