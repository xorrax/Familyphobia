using UnityEngine;
using System.Collections;
public class Warp : MonoBehaviour
{

    public Player player;
    public Transform warpTo;
    public GameObject newBackground;
    public Camera newCamera;
    public Color color;
    public Sprite warpBlock;
    public AudioClip warpSound;
    public float fadeValue;
    public Texture2D cursorTexture;
    private AudioSource audioSource;
    private float alpha;
    private GameObject warpFade;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = warpSound;
    }
    public int scene = 1;

    IEnumerator FadeIn()
    {
        while (alpha <= 1)
        {
            Color newColor = warpFade.GetComponent<SpriteRenderer>().color;
            warpFade.GetComponent<SpriteRenderer>().color = new Color(newColor.r, newColor.g, newColor.b, newColor.a + fadeValue);
            alpha = warpFade.GetComponent<SpriteRenderer>().color.a;
            if (alpha >= 1)
            {
                newBackground.GetComponent<Grid>().gameObject.SetActive(true);
                player.pathfinding.setGrid(newBackground);
                player.transform.position = warpTo.transform.position;
                player.pathfinding.endPathfinding();


                player.anim.SetBool("Walking", false);
                player.anim.SetFloat("hSpeed", 0);
                player.anim.SetFloat("vSpeed", 0);

                Inventory.invInstance.SendMessage("ChangeCamera", newCamera);
                GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
                newCamera.gameObject.SetActive(true);
                warpFade.transform.position = new Vector3(newCamera.transform.position.x, newCamera.transform.position.y, newCamera.transform.position.z + 1);
                StartCoroutine("FadeOut");
                StopCoroutine("FadeIn");


                if(newCamera.name == "Shed_Camera")
                {
                    GameObject tempObject = GameObject.Find("FrameTree");
                    tempObject.SendMessage("SetSortingOrder");
                }

            }
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        while (alpha >= 0)
        {
            Color newColor = warpFade.GetComponent<SpriteRenderer>().color;
            warpFade.GetComponent<SpriteRenderer>().color = new Color(newColor.r, newColor.g, newColor.b, newColor.a - fadeValue);
            alpha = warpFade.GetComponent<SpriteRenderer>().color.a;
            if (alpha <= 0)
            {
                GameObject.Destroy(warpFade);
                StopCoroutine("FadeOut");
            }
            yield return null;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            player.pathfinding.endPathfinding();
            if (!GameObject.Find("WarpFade"))
            {

                audioSource.Play();

                //Skapar Fade objektet
                warpFade = new GameObject();
                warpFade.name = ("WarpFade");
                warpFade.AddComponent<SpriteRenderer>();
                warpFade.GetComponent<SpriteRenderer>().sprite = warpBlock;
                warpFade.GetComponent<SpriteRenderer>().color = color;
                alpha = color.a;

                //Transformerar Fade objektet
                warpFade.transform.position = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
                warpFade.transform.position = new Vector3(warpFade.transform.position.x, warpFade.transform.position.y, warpFade.transform.position.z + 1f);
                warpFade.GetComponent<SpriteRenderer>().sortingOrder = 1000;
                warpFade.transform.localScale += new Vector3(8 * transform.localScale.x, 8 * transform.localScale.y, transform.localScale.z);

                StartCoroutine("FadeIn");
            }

        }
    }

    void OnMouseOver()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    void OnMouseExit(){
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}




