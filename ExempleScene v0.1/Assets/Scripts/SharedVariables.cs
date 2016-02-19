﻿using UnityEngine;
using System.Collections;

public class SharedVariables : MonoBehaviour
{
    private static string newRoom;
    private static bool outFromDreamworld;

    public static bool OutFromDreamworld
    {
        set
        {
            outFromDreamworld = value;
        }
        get
        {
            return outFromDreamworld;
        }
    }
    public static string NewRoom
    {
        set
        {
            newRoom = value;
        }
        get
        {
            return newRoom;
        }
    }
}


