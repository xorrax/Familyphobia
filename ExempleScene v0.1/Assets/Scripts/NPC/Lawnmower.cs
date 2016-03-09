using UnityEngine;
using System.Collections;

public class Lawnmower : MonoBehaviour {
    ChangeMesh changeMesh;
    GameObject grass;
    public Color color;
    public Sprite warpBlock;
    public AudioClip soundEffect;
    public float fadeValue = 0.05f;
    public int CuttingTime = 2;
    private AudioSource audioSource;
    private float alpha;
    private GameObject warpFade;


    void Start(){
        changeMesh = GameObject.Find("Shed_Background").GetComponent<ChangeMesh>();
        grass = GameObject.Find("Shed_Grass");

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundEffect;

	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            cutGrass();
        }
    }

    public void cutGrass() {

        if (GameObject.Find("Shed_Grass").activeSelf) {
            warpFade = new GameObject();
            warpFade.name = ("WarpFade");
            warpFade.AddComponent<SpriteRenderer>();
            warpFade.AddComponent<BoxCollider>();
            warpFade.tag = "warpFade";
            warpFade.GetComponent<SpriteRenderer>().sprite = warpBlock;
            warpFade.GetComponent<SpriteRenderer>().color = color;
            alpha = color.a;

            //Transformerar Fade objektet
            warpFade.transform.position = this.transform.position;
            warpFade.transform.position = new Vector3(warpFade.transform.position.x, warpFade.transform.position.y, warpFade.transform.position.z + 1f);
            warpFade.GetComponent<SpriteRenderer>().sortingOrder = 1000;
            warpFade.transform.localScale += new Vector3(200 * transform.localScale.x, 100 * transform.localScale.y, transform.localScale.z);

            StartCoroutine("FadeIn");
        }
    }
    IEnumerator FadeIn() {
        while (alpha <= 1) {
            Color newColor = warpFade.GetComponent<SpriteRenderer>().color;
            warpFade.GetComponent<SpriteRenderer>().color = new Color(newColor.r, newColor.g, newColor.b, newColor.a + fadeValue);
            alpha = warpFade.GetComponent<SpriteRenderer>().color.a;
            if (alpha >= 1) {
                yield return new WaitForSeconds(CuttingTime);
                changeMesh.changeMesh();
                grass.SetActive(false);
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
                StopCoroutine("FadeOut");
            }
            yield return null;
        }
    }

}
