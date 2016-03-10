﻿using UnityEngine;
using System.Collections;
public class Warp : MonoBehaviour {
    public string currentRoom;
    public string newRoom;
    public float newScale = 0.5f;
    public float newDepth = 1f;
    public float newDepthOffset;
    public bool activated = true;
    private bool clicked = false;
    private Player player;
    private Transform warpTo;
    private GameObject newBackground;
    private GameObject newCamera;
    public Color color;
    public Sprite warpBlock;
    public AudioClip warpSound;
    public AudioClip newFootsteps;
    public float fadeValue = 0.05f;
    public Texture2D cursorTexture;
    private AudioSource audioSource;
    private float alpha;
    private GameObject warpFade;

    void OnLevelWasLoaded(int level) {
        if (level != 0) {
            //Måste fixa så att det går att hitta med tag
            player = GameObject.Find("Jack").GetComponent<Player>();
            warpTo = GameObject.Find(newRoom + "_" + currentRoom + "Spawn").GetComponent<Transform>();
            newBackground = GameObject.Find(newRoom + "_Background");
            newCamera = GameObject.Find(newRoom + "_Camera");

            audioSource = GetComponent<AudioSource>();
            audioSource.clip = warpSound;
        }
    }

    IEnumerator FadeIn() {
        while (alpha <= 1) {
            Color newColor = warpFade.GetComponent<SpriteRenderer>().color;
            warpFade.GetComponent<SpriteRenderer>().color = new Color(newColor.r, newColor.g, newColor.b, newColor.a + fadeValue);
            alpha = warpFade.GetComponent<SpriteRenderer>().color.a;
            if (alpha >= 1) {
                newBackground.GetComponent<Grid>().gameObject.SetActive(true);
                player.pathfinding.setGrid(newBackground);
                player.transform.position = warpTo.transform.position;
                player.currentRoom = newRoom;
                if (GameObject.Find("BekNpc") != null)
                GameObject.Find("BekNpc").GetComponent<Bek>().updateSpawnPoints();
                player.gameObject.GetComponent<FakePerspective>().setStartScale(new Vector3(newScale, newScale, player.transform.localScale.z));
                player.gameObject.GetComponent<FakePerspective>().depth = newDepth;
                player.gameObject.GetComponent<FakePerspective>().depthOffset = newDepthOffset;
                player.GetComponent<Footsteps>().soundEffect = newFootsteps;
                player.pathfinding.endPathfinding();


                GameObject.Find(currentRoom + "_Camera").GetComponent<Camera>().enabled = false;
                newCamera.GetComponent<Camera>().enabled = true;
                warpFade.transform.position = new Vector3(newCamera.transform.position.x, newCamera.transform.position.y, newCamera.transform.position.z + 1);
                StartCoroutine("FadeOut");
                StopCoroutine("FadeIn");

            }
            yield return null;
        }
        
    }

    IEnumerator FadeOut() {
        while (alpha >= 0) {
            Color newColor = warpFade.GetComponent<SpriteRenderer>().color;
            warpFade.GetComponent<SpriteRenderer>().color = new Color(newColor.r, newColor.g, newColor.b, newColor.a - fadeValue);
            alpha = warpFade.GetComponent<SpriteRenderer>().color.a;
            if (alpha <= 0) {
                GameObject.Destroy(warpFade);
                player.pathfinding.setIsActive(true);
                if (SharedVariables.firstTimeBeeRoom && newRoom == "Birthday") {
                    GameObject.Find("FirstTimeBeeRoom").GetComponent<NPC>().interact();
                }

                if (SharedVariables.firstTimeShedToEntrance && currentRoom == "Shed" && newRoom == "Entrance") {
                    GameObject.Find("FirstTimeShedToEntrance").GetComponent<NPC>().interact();
                }
                StopCoroutine("FadeOut");
            }
            yield return null;
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Player" && clicked) {
            if (!GameObject.Find("WarpFade")) {
                player.pathfinding.endPathfinding();
                player.pathfinding.setIsActive(false);
                audioSource.Play();

                //Skapar Fade objektet
                warpFade = new GameObject();
                warpFade.name = ("WarpFade");
                warpFade.AddComponent<SpriteRenderer>();
                warpFade.AddComponent<BoxCollider>();
                warpFade.tag = "warpFade";
                warpFade.GetComponent<SpriteRenderer>().sprite = warpBlock;
                warpFade.GetComponent<SpriteRenderer>().color = color;
                alpha = color.a;

                //Transformerar Fade objektet
                warpFade.transform.position = GameObject.Find(currentRoom + "_Camera").transform.position;
                warpFade.transform.position = new Vector3(warpFade.transform.position.x, warpFade.transform.position.y, warpFade.transform.position.z + 1f);
                warpFade.GetComponent<SpriteRenderer>().sortingOrder = 1000;
                warpFade.transform.localScale += new Vector3(20 * transform.localScale.x, 8 * transform.localScale.y, transform.localScale.z);

                clicked = false;
                StartCoroutine("FadeIn");
            }

        }
    }

    void OnMouseOver() {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        if (Input.GetMouseButton(0)) {
            clicked = true;
        }
    }

    void OnMouseExit() {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}

