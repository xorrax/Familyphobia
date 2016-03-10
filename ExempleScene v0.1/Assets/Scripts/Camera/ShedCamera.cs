using UnityEngine;
using System.Collections;

public class ShedCamera : CameraMovement {
   
    private Vector2 max;
    private Vector2 min;
    private float cameraMove = 1f;
    public float cameraVelocity = 0.2f;
    private float cameraTrigger = 33f;
    public float zoomOutSize = 7f;
    Vector3 bottomLeft;
    
    float originalHeight;
    float originalWidth;
    private bool hasScaled = false;
    private float cameraSize = 0f;
    Vector2 minOffset;
    Vector2 maxOffset;

    protected override void Start() {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        thisCamera = GetComponent<Camera>();

        height = 2f * thisCamera.orthographicSize;
        width = height * thisCamera.aspect;

        originalHeight = 2f * thisCamera.orthographicSize;
        originalWidth = height * thisCamera.aspect;

        bottomLeft = new Vector3(background.transform.position.x - (background.GetComponent<Renderer>().bounds.size.x / 2),
            background.transform.position.y - (background.GetComponent<Renderer>().bounds.size.y / 2), 0);

        max.x = ((bottomLeft.x + background.GetComponent<Renderer>().bounds.size.x) - (width / 2));
        max.y = ((bottomLeft.y + background.GetComponent<Renderer>().bounds.size.y) - (height / 2));

        min.x = bottomLeft.x + (width / 2);
        min.y = bottomLeft.y + (height / 2);
        cameraSize = thisCamera.orthographicSize;
           
        StartCoroutine("ZoomOut");

    }

    public override float getMinY() {
        return min.y - (height / 2);
    }

    public override float getMaxY() {
        return max.y + (height / 2);
    }

    protected override void FixedUpdate() {
        height = 2f * thisCamera.orthographicSize;
        width = height * thisCamera.aspect;

        max.x = ((bottomLeft.x + background.GetComponent<Renderer>().bounds.size.x) - (width / 2));
        max.y = ((bottomLeft.y + background.GetComponent<Renderer>().bounds.size.y) - (height / 2));

        min.x = bottomLeft.x + (width / 2);
        min.y = bottomLeft.y + (height / 2);

        if (target.transform.position.x > cameraTrigger) {
            minOffset.x = thisCamera.transform.position.x - (width / 4);
            maxOffset.x = thisCamera.transform.position.x + (width / 4);

            minOffset.y = thisCamera.transform.position.y - (height / 4);
            maxOffset.y = thisCamera.transform.position.y + (height / 4);

            if (target.position.x < minOffset.x) {
                thisCamera.transform.position = new Vector3(thisCamera.transform.position.x + (target.transform.position.x - minOffset.x), thisCamera.transform.position.y, -10);
                if (thisCamera.transform.position.x < min.x) {
                    thisCamera.transform.position = new Vector3(min.x, thisCamera.transform.position.y, -10);
                }
            }

            if (target.position.x > maxOffset.x) {
                thisCamera.transform.position = new Vector3(thisCamera.transform.position.x + (target.transform.position.x - maxOffset.x), thisCamera.transform.position.y, -10);
                if (thisCamera.transform.position.x > max.x) {
                    thisCamera.transform.position = new Vector3(max.x, thisCamera.transform.position.y, -10);
                }
            }


            if (target.position.y < minOffset.y) {
                thisCamera.transform.position = new Vector3(thisCamera.transform.position.x, thisCamera.transform.position.y + (target.transform.position.y - minOffset.y), -10);
                if (thisCamera.transform.position.y < min.y) {
                    thisCamera.transform.position = new Vector3(thisCamera.transform.position.x, min.y, -10);
                }
            }

            if (target.position.y > minOffset.y) {
                thisCamera.transform.position = new Vector3(thisCamera.transform.position.x, thisCamera.transform.position.y + (target.transform.position.y - minOffset.y), -10);
                if (thisCamera.transform.position.y > max.y) {
                    thisCamera.transform.position = new Vector3(thisCamera.transform.position.x, max.y, -10);
                }
            }
        } 
        else {
            if (thisCamera.transform.position.x > min.x + cameraMove)
                thisCamera.transform.position = new Vector3(thisCamera.transform.position.x - cameraVelocity, thisCamera.transform.position.y, thisCamera.transform.position.z);
            else
                thisCamera.transform.position = new Vector3(min.x + cameraMove, thisCamera.transform.position.y, thisCamera.transform.position.z);
        }

        if (thisCamera.enabled)
        {
            //thisCamera.transform.position.x - (width / 2), thisCamera.transform.position.y - (height / 2)
            Inventory.invInstance.transform.position = new Vector3(thisCamera.transform.position.x,
                thisCamera.transform.position.y - (height / 2) + Inventory.invInstance.currentOffsetY + 0.3f,
                Inventory.invInstance.transform.position.z);

            if (hasScaled) {
                Inventory.invInstance.GetComponent<SpriteRenderer>().enabled = true;
                Inventory.invInstance.ShowItems();
                hasScaled = false;
            }
        }
    }

    IEnumerator ZoomOut(){
        while (thisCamera.orthographicSize <= zoomOutSize) {
            if (thisCamera.enabled)
            {
                target.SendMessage("CanWalk", false);
                if (Inventory.invInstance != null)
                {
                    Inventory.invInstance.HideItems();
                    Inventory.invInstance.GetComponent<SpriteRenderer>().enabled = false;
                }
                thisCamera.orthographicSize += 0.01f;
            }
            yield return 0;
        }
        if (thisCamera.orthographicSize >= zoomOutSize && !hasScaled)
        {
            Inventory.invInstance.transform.position = new Vector3(34.18703f, -5.91033f, Inventory.invInstance.transform.position.z);
            target.SendMessage("CanWalk", true);
            hasScaled = true;
        }

    }
}
