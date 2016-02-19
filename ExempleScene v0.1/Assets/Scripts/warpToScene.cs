using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class warpToScene : MonoBehaviour
{
    public string nameOfScene;
    public string spawnRoom;
    public bool outFromDreamWorld;

    public void LoadScene(string level)
    {
        SharedVariables.NewRoom = spawnRoom;
        SharedVariables.OutFromDreamworld = outFromDreamWorld;
        SceneManager.LoadScene(level);
    }
}


