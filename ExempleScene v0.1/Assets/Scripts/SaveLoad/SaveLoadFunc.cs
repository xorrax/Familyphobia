using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class SaveLoadFunc{

    public static List<object> saveList = new List<object>();

    public static void Save() {
        saveList.Add(Inventory.invInstance.itemList);
        saveList.Add(Inventory.invInstance.existingItem);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveList.gd");
        bf.Serialize(file, SaveLoadFunc.saveList);
        file.Close();
    }

    public static void Load() {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            SaveLoadFunc.saveList = (List<object>)bf.Deserialize(file);
            file.Close();
        }
    }
}
