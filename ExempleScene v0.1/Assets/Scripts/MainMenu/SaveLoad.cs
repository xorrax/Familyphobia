using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour
{

     //int leg = 2;
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerinfo1.dat");

        PlayerInfo info = new PlayerInfo();
        //info.leg = leg;
        //stuff

        bf.Serialize(file, info);
        file.Close();
    }

    public void Save2()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerinfo2.dat");

        PlayerInfo info = new PlayerInfo();
        //info.leg = leg;
        //stuff

        bf.Serialize(file, info);
        file.Close();
    }

    public void Save3()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerinfo3.dat");

        PlayerInfo info = new PlayerInfo();
        //info.leg = leg;
        //stuff

        bf.Serialize(file, info);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerinfo1.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerinfo1.dat", FileMode.Open);
            PlayerInfo info = (PlayerInfo)bf.Deserialize(file);
            file.Close();

           // leg = info.leg;
            //stuff

        }
    }
    public void Load2()
    {
        if (File.Exists(Application.persistentDataPath + "/playerinfo2.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerinfo2.dat", FileMode.Open);
            PlayerInfo info = (PlayerInfo)bf.Deserialize(file);
            file.Close();

            //leg = info.leg;
            //stuff

        }
    }
    public void Load3()
    {
        if (File.Exists(Application.persistentDataPath + "/playerinfo3.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerinfo3.dat", FileMode.Open);
            PlayerInfo info = (PlayerInfo)bf.Deserialize(file);
            file.Close();

           // leg = info.leg;
            //stuff

        }
    }

   

}
[Serializable]
class PlayerInfo{
   // public int leg;
    //stuff
}
