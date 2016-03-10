using UnityEngine;
using System.Collections;

public class Flik : MonoBehaviour {


    private Camera thisCamera;
    private Camera lastCamera;

    public Sprite closedSprite;
    public Sprite openSprite;

    private bool state;

    void OnLevelWasLoaded(int level)
    {
        if(level != 0)
            thisCamera = Camera.main;

        lastCamera = thisCamera;
    }

    void Update()
    {
        if(thisCamera == null || thisCamera.enabled == false)
        {
            thisCamera = Camera.main;
            if (lastCamera != null)
            {
                if (thisCamera.name == "Shed_Camera" && lastCamera.name != "Shed_Camera")
                {
                    transform.position = new Vector3(transform.position.x - 0.09f, transform.position.y, transform.position.z);
                }
                else if(lastCamera.name == "Shed_Camera")
                {
                    transform.position = new Vector3(transform.position.x + 0.09f, transform.position.y, transform.position.z);
                }
            }

            lastCamera = thisCamera;
        }
    }

    void ChangeSprite()
    {
        if(Inventory.invInstance.activateInv)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = openSprite;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = closedSprite;
        }
    }

    void OnMouseOver()
    {
        Debug.Log("On flik");
        if (Input.GetMouseButtonDown(0))
        {
            ChangeSprite();
            Inventory.invInstance.ActivateInventory();
        }
    }
}
