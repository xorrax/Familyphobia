using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundOptions : MonoBehaviour {

    private float soundVol;
    public string slidername;
    public Slider slajder;
    public AudioMixer masterMixer;

    public void SetMasterLvl(float masterLvl) {
        masterMixer.SetFloat("master", masterLvl);
    }
    public void SetMusicLvl(float musicLvl) {
        masterMixer.SetFloat("music", musicLvl);
    }

    public void SetSfxLvl(float sfxLvl) {
        masterMixer.SetFloat("sfx", sfxLvl);
    }

    public void SetDialogLvl(float dialogLvl) {
        masterMixer.SetFloat("dialog", dialogLvl);
    }

    void Start() {
        
      
        masterMixer.GetFloat(slidername, out soundVol);
        slajder.value = soundVol;
            
    }
        

}