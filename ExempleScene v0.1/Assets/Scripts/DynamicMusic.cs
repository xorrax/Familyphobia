using UnityEngine;
using System.Collections;

public class DynamicMusic : MonoBehaviour {

    public float distance;
    public AudioClip newSong;
    private PlayAudio playAudio;
    private Transform player;

    void Start() {
        playAudio = GameObject.Find("Music").GetComponent<PlayAudio>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update() {
        float distanceFromPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceFromPlayer <= distance) {
            playAudio.changeAudio(newSong, 0);

        } else if (playAudio.getCurrentClip() == newSong) {
            playAudio.playLastClip();
        }
    }
}