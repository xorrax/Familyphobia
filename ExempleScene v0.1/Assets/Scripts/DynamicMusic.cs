using UnityEngine;
using System.Collections;

public class DynamicMusic : MonoBehaviour {

    public float distance;
    public AudioClip newSong;
    private PlayAudio playAudio;
    private Transform player;
    private float clipTime;


	void Start () {
        playAudio = GameObject.Find("Music").GetComponent<PlayAudio>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        clipTime = 0;
    }

	void Update () {
        float distanceFromPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceFromPlayer <= distance) {
            playAudio.changeAudio(newSong, clipTime);

        } else if (playAudio.getCurrentClip() == newSong) {
            clipTime = playAudio.getCurrentClipTime();
            playAudio.changeAudio(playAudio.getLastClip(), playAudio.getLastClipTime());
        }
	}
}
