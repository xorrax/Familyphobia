using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{

    public List<GameObject> itemList = new List<GameObject>();
    public static Inventory invInstance;
    private Camera thisCamera;

    private Vector3 startPos;
    private float distance;

    private float cameraOffsetY = 0f;
    private float currentOffsetY = 0f;

    private bool activateInv = false;

    float fps = 0.0f;
    float deltaTime = 0.0f;
    /// <summary>
    /// oiashdfåsdgthas¨äådfghuasdfpui
    /// </summary>

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void OnLevelWasLoaded(int level)
    {
        if (level != 0)
        {
            invInstance = this;
<<<<<<< HEAD
            
=======
            thisCamera = Camera.main;
>>>>>>> origin/master
        }
    }
    void Update()
    {
        if(thisCamera == null)
            thisCamera = Camera.main;

        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            activateInv = !activateInv;
        }
        
    }

    void FixedUpdate() {
        SetInventory();
    }

    public void AddItem(GameObject item)
    {
        itemList.Add(item);

        Vector2 tempPos = new Vector2(transform.position.x - gameObject.GetComponent<SpriteRenderer>().sprite.rect.width / 100 - 0.6f + itemList.Count, transform.position.y);

        item.GetComponent<Transform>().position = tempPos;
        item.SendMessage("SetPosition", tempPos);
        item.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder + 2;
        item.transform.SetParent(gameObject.transform);
    }

    public void RemoveItem(GameObject item)
    {
        itemList.Remove(item);
    }

    void SetPositions()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            Vector2 tempPos = new Vector2(transform.position.x - gameObject.GetComponent<SpriteRenderer>().sprite.rect.width / 100 + 0.4f + i, transform.position.y);
            itemList[i].GetComponent<Transform>().position = tempPos;
            itemList[i].SendMessage("SetPosition", tempPos);
        }
    }

    void SetInventory()
    {
<<<<<<< HEAD
        if (thisCamera != null) {
            
            if (activateInv) {
                if (transform.position.y < -thisCamera.orthographicSize - gameObject.GetComponent<SpriteRenderer>().bounds.size.y + 1.4f) {
                    transform.Translate(0, 0.05f, 0);
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    SetPositions();
                }
            } else {
                if (transform.position.y > -thisCamera.orthographicSize - 2 * gameObject.GetComponent<SpriteRenderer>().bounds.size.y + 1.4f) {
                    transform.Translate(0, -0.05f, 0);
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    SetPositions();
                }
=======
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            activateInv = !activateInv;
        }
        if (activateInv)
        {
            if (transform.position.y < -thisCamera.orthographicSize - gameObject.GetComponent<SpriteRenderer>().bounds.size.y + 1.4f)
            {
                transform.Translate(0, 0.05f, 0);
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                SetPositions();
            }
        }
        else
        {
            if (transform.position.y > -thisCamera.orthographicSize - 2 * gameObject.GetComponent<SpriteRenderer>().bounds.size.y + 1.4f)
            {
                transform.Translate(0, -0.05f, 0);
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                SetPositions();
>>>>>>> origin/master
            }
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForEndOfFrame();
    }
}
