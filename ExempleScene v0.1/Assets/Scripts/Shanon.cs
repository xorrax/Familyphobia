using UnityEngine;
using System.Collections;

public class Shanon : NPC {

    public string newScene;
    public string newRoom;
    bool oneClick = true;
    const float MIN_TIME = 0.00f;
    const float MAX_TIME = 2f;
    float time = 0;
    public warpToScene warp;

    private float alpha;
    private GameObject warpFade;
    private AudioSource audioSource;
    public Color fadeColor;
    public Sprite newSceneWarpBlock;
    public float newScenefadeValue = 0.05f;

	// Use this for initialization
	void Start () {
	
	}

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {

            if (!oneClick && time < MAX_TIME && time > MIN_TIME) {
                SharedVariables.NewRoom = newRoom;
                if (!GameObject.Find("WarpFade")) {

                   // audioSource.Play();

                    //Skapar Fade objektet
                    warpFade = new GameObject();
                    warpFade.name = ("WarpFade");
                    warpFade.AddComponent<SpriteRenderer>();
                    warpFade.AddComponent<BoxCollider>();
                    warpFade.tag = "warpFade";
                    warpFade.GetComponent<SpriteRenderer>().sprite = newSceneWarpBlock;
                    warpFade.GetComponent<SpriteRenderer>().color = fadeColor;
                    alpha = fadeColor.a;
                    warpFade.GetComponent<SpriteRenderer>().sortingOrder = 1000;
                    warpFade.transform.localScale += new Vector3(30 * transform.localScale.x, 30 * transform.localScale.y, transform.localScale.z);


                    warp.LoadScene(newScene);

                }
               
            }
            if (oneClick) {
                oneClick = false;
            }
        }
        time += Time.deltaTime;
        if (time > MAX_TIME) {
            oneClick = true;
            time = 0;
        }
    }

    void OnMouseExit() {
        oneClick = true;
        time = 0;
    }

    void FixedUpdate() {

    }

    IEnumerator loadSceneFadeIn() {
        while (alpha <= 1) {
            Color newColor = warpFade.GetComponent<SpriteRenderer>().color;
            warpFade.GetComponent<SpriteRenderer>().color = new Color(newColor.r, newColor.g, newColor.b, newColor.a + newScenefadeValue);
            alpha = warpFade.GetComponent<SpriteRenderer>().color.a;
            if (alpha >= 1) {

                Camera thisCamera = Camera.main;
                warpFade.transform.position = new Vector3(thisCamera.transform.position.x, thisCamera.transform.position.y, thisCamera.transform.position.z + 1);
                
                
                
                StartCoroutine("loadSceneFadeOut");
                StopCoroutine("loadSceneFadeIn");
                warp.LoadScene(newScene);
            }
            yield return null;
        }
    }

    IEnumerator loadSceneFadeOut() {
        while (alpha >= 0) {

        }
        yield return null;
    }
}
