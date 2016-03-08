using UnityEngine;
using System.Collections;

public class SharedVariables : MonoBehaviour {
    private static string newRoom;
    private static string newScene;
    private static bool outFromDreamworld;
    private static bool firstTimeDreamworld;

    public static bool OutFromDreamworld {
        set {
            outFromDreamworld = value;
        }
        get {
            return outFromDreamworld;
        }
    }
    public static string NewRoom {
        set {
            newRoom = value;
        }
        get {
            return newRoom;
        }
    }

    public static string NewScene {
        set {
            newScene = value;
        }
        get {
            return newScene;
        }
    }
    public static bool FirstTimeDreamworld {
        set {
            firstTimeDreamworld = value;
        }
        get {
            return firstTimeDreamworld;
        }
    }
}
