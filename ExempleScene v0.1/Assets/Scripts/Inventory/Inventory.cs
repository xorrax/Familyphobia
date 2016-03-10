using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{

    public List<GameObject> itemList = new List<GameObject>();
    public List<string> existingItem = new List<string>();
    public Sprite longSprite;
    public Sprite shortSprite;
    public static Inventory invInstance;
    public float temp;
    public GameObject tab;
    private Camera thisCamera;

    private Vector3 startPos;
    private float distance;

    private float cameraOffsetY = 0f;
    public float currentOffsetY = 0f;
    private float width;

    private float cameraWidth, newScale;
    public float cameraStartWidth;

    public bool activateInv = false;
    public bool holdingItem = false;

    private bool inBouquet = false;

    float fps = 0.0f;
    float deltaTime = 0.0f;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void OnLevelWasLoaded(int level)
    {
        invInstance = this;
        width = gameObject.GetComponent<SpriteRenderer>().sprite.rect.width;
        if (level != 0)
        {
            invInstance = this;
            thisCamera = Camera.main;

        }
        foreach (string o in existingItem)
        {
            GameObject[] tempObjects = GameObject.FindGameObjectsWithTag("Item");
            foreach(GameObject g in tempObjects)
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (g.name == o && g != itemList[i])
                    {
                        if (i == itemList.Count - 1)
                        {
                            if(g.name == "BlueFlower" || g.name == "RedFlower" || g.name == "YellowFlower")
                            {
                                g.SendMessage("CheckFlower");
                                if (inBouquet)
                                {
                                    inBouquet = false;
                                }
                                else
                                    Destroy(g);
                            }
                            else
                                Destroy(g);
                        }
                        else
                            continue;
                    }
                    else if(g == itemList[i])
                    {
                        break; 
                    }
                }
            }


        }
    }

    void Update()
    {
        if (thisCamera == null || thisCamera.enabled == false)
        {
            thisCamera = Camera.main;
            SetPositions();
        }

        if (thisCamera.name == "Shed_Camera")
        {
            cameraWidth = 2f * thisCamera.orthographicSize * thisCamera.aspect;
            newScale = cameraWidth / cameraStartWidth;
            transform.localScale = new Vector3(1.241f, 1.241f, transform.localScale.z);
            gameObject.GetComponent<SpriteRenderer>().sprite = longSprite;
            gameObject.GetComponent<BoxCollider>().size = new Vector3(19.2f, gameObject.GetComponent<BoxCollider>().size.y, gameObject.GetComponent<BoxCollider>().size.z);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = shortSprite;
            gameObject.GetComponent<BoxCollider>().size = new Vector3(19, gameObject.GetComponent<BoxCollider>().size.y, gameObject.GetComponent<BoxCollider>().size.z);
            transform.localScale = new Vector3(1, 1, 1);
        }
    }


    void FixedUpdate()
    {
        SetInventory();
    }

    public void GetFlower(bool value)
    {
        inBouquet = value;
    }

    public void ActivateInventory()
    {
        activateInv = !activateInv;
    }

    public void SetInventory(bool value)
    {
        activateInv = value;
        if(value == false)
        {
            thisCamera = Camera.main;
            if (thisCamera.name == "Shed_Camera")
            {
                transform.position = new Vector3(transform.position.x, -thisCamera.orthographicSize - 2 * gameObject.GetComponent<SpriteRenderer>().bounds.size.y + temp, transform.position.z);
                SetPositions();
            }
            else
            {
                transform.position = new Vector3(transform.position.x, -thisCamera.orthographicSize - gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2 - 0.05f, transform.position.z);
                SetPositions();
            }
        }
    }

    public void AddExistingOnly(GameObject item)
    {
        if (existingItem.Count == 0)
        {
            existingItem.Add(item.name);
        }
        else
        {
            for (int e = 0; e < existingItem.Count; e++)
            {
                if (existingItem[e] == item.name)
                {
                    break;
                }
                else
                {
                    if (e == existingItem.Count - 1)
                    {
                        existingItem.Add(item.name);
                    }
                    continue;
                }
            }
        }
    }

    public void AddItem(GameObject item)
    {
        bool checkItem = true;
        for (int i = 0; i < itemList.Count; i++)
        {
            if(item == itemList[i])
            {
                Vector2 tempPosI = new Vector2(transform.position.x - width / 200 * transform.localScale.x - 0.6f + i, transform.position.y);

                item.GetComponent<Transform>().position = tempPosI;
                item.SendMessage("SetPosition", tempPosI);
                item.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder + 2;
                item.transform.SetParent(gameObject.transform);
                checkItem = false;
                break;
            }
        }
        if (checkItem)
        {
            itemList.Add(item);

            Vector2 tempPos = new Vector2(transform.position.x - width / 200 * transform.localScale.x - 0.6f + itemList.Count, transform.position.y);

            item.GetComponent<Transform>().position = tempPos;
            item.SendMessage("SetPosition", tempPos);
            item.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder + 2;
            item.transform.SetParent(gameObject.transform);
        }
        if (existingItem.Count == 0)
        {
            existingItem.Add(item.name);
            Debug.Log("added " + item.name + " to existing");
        }
        else
        {
            for (int e = 0; e < existingItem.Count; e++)
            {
                if (existingItem[e] == item.name)
                {
                    break;
                }
                else
                {
                    if (e == existingItem.Count -1)
                    {
                        existingItem.Add(item.name);
                        Debug.Log("added " + item.name + " to existing");
                    }
                    continue;
                }
            }
        }
    }

    public void HideItems()
    {
        tab.GetComponent<SpriteRenderer>().enabled = false;
        foreach(GameObject i in itemList)
        {
            i.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void ShowItems()
    {
        tab.GetComponent<SpriteRenderer>().enabled = true;
        foreach (GameObject i in itemList)
        {
            i.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void RemoveItem(GameObject item)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] == item)
            {
                itemList.Remove(item);
                break;
            }
        }
    }

    void SetPositions()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            Vector2 tempPos = new Vector2(transform.position.x - width / 200 * transform.localScale.x + 0.4f + i, transform.position.y);
            itemList[i].GetComponent<Transform>().position = tempPos;
            itemList[i].SendMessage("SetPosition", tempPos);
        }
    }

    void SetInventory()
    {
        if (thisCamera != null)
        {
            if (activateInv)
            {
                if (thisCamera.name == "Shed_Camera")
                {
                    if (transform.position.y < -thisCamera.orthographicSize - gameObject.GetComponent<SpriteRenderer>().bounds.size.y + 0.35f) //Ändra inte värden!!
                    {
                        currentOffsetY += 0.03f;
                        transform.Translate(0, 0.03f, 0);
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                        SetPositions();
                    }

                }
                else
                {
                    if (transform.position.y < -thisCamera.orthographicSize + gameObject.GetComponent<SpriteRenderer>().bounds.size.y/2 - 0.05f)
                    {
                        currentOffsetY += 0.03f;
                        transform.Translate(0, 0.03f, 0);
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                        SetPositions();
                    }
                }
            }
            else
            {
                if (thisCamera.name == "Shed_Camera")
                {
                    if (transform.position.y > -thisCamera.orthographicSize - 2 * gameObject.GetComponent<SpriteRenderer>().bounds.size.y + 0.45f)
                    {
                        currentOffsetY -= 0.03f;
                        transform.Translate(0, -0.03f, 0);
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                        SetPositions();
                    }
                }
                else
                {
                    if (transform.position.y > -thisCamera.orthographicSize - gameObject.GetComponent<SpriteRenderer>().bounds.size.y/2 - 0.05f)
                    {
                        currentOffsetY -= 0.03f;
                        transform.Translate(0, -0.03f, 0);
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                        SetPositions();
                    }
                }
            }
        }
    }

    public void ChangeCamera(Camera camera)
    {
        thisCamera = camera;
    }

    IEnumerator wait()
    {
        yield return new WaitForEndOfFrame();
    }
}
