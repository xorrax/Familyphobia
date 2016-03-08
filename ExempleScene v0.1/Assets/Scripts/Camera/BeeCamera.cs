using UnityEngine;
using UnityEngine;
using System.Collections;

public class BeeCamera : CameraMovement {
    private Vector2 max;
    private Vector2 min;
    public float zoomOutSpeed = 0.05f;
    public float offSet = 14;
    public float zoomOutSize = 5.35f;
    Vector3 bottomLeft;
    Vector2 bottomRight;
    Vector3 center;
    float originalHeight;
    float originalWidth;
    private bool hasScaled = false;
    private float cameraSize = 0f;
    Vector2 minOffset;
    Vector2 maxOffset;
    bool followTarget = true;

    protected override void Start() {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        thisCamera = GetComponent<Camera>();

        height = 2f * thisCamera.orthographicSize;
        width = height * thisCamera.aspect;

        originalHeight = 2f * thisCamera.orthographicSize;
        originalWidth = height * thisCamera.aspect;

        bottomLeft = new Vector3(background.transform.position.x - (background.GetComponent<Renderer>().bounds.size.x / 2),
            background.transform.position.y - (background.GetComponent<Renderer>().bounds.size.y / 2), 0);

        bottomRight = new Vector2(background.transform.position.x + (background.GetComponent<Renderer>().bounds.size.x / 2),
            background.transform.position.y - (background.GetComponent<Renderer>().bounds.size.y / 2));
        center = new Vector3(bottomLeft.x + background.GetComponent<Renderer>().bounds.size.x / 2, 
            bottomLeft.y + background.GetComponent<Renderer>().bounds.size.y / 2, -10);

        max.x = ((bottomLeft.x + background.GetComponent<Renderer>().bounds.size.x) - (width / 2));
        max.y = ((bottomLeft.y + background.GetComponent<Renderer>().bounds.size.y) - (height / 2));

        min.x = bottomLeft.x + (width / 2);
        min.y = bottomLeft.y + (height / 2);
        cameraSize = thisCamera.orthographicSize;

    }

    public void forceCameraBorder() {
        if (thisCamera.transform.position.x + (width / 2) > bottomRight.x) {
            thisCamera.transform.position = new Vector3(max.x, thisCamera.transform.position.y, -10);
        }
        if (thisCamera.transform.position.x - (width / 2) < bottomLeft.x) {
            thisCamera.transform.position = new Vector3(min.x, thisCamera.transform.position.y, -10);
        }
        if (thisCamera.transform.position.y + (height / 2) > bottomRight.y + background.GetComponent<Renderer>().bounds.size.y) {
            thisCamera.transform.position = new Vector3(thisCamera.transform.position.x, max.y, -10);
        }
        if (thisCamera.transform.position.y - (height / 2) < bottomRight.y) {
            thisCamera.transform.position = new Vector3(thisCamera.transform.position.x, min.y, -10);
        }
    }
    public void zoomOut() {
        followTarget = false;
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
        minOffset.x = thisCamera.transform.position.x - (width / offSet);
        maxOffset.x = thisCamera.transform.position.x + (width / offSet);

        minOffset.y = thisCamera.transform.position.y - (height / offSet);
        maxOffset.y = thisCamera.transform.position.y + (height / offSet);

        if (followTarget) {

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

        if (thisCamera.enabled) {
            //Inventory.invInstance.transform.position = new Vector3(thisCamera.transform.position.x - thisCamera.orthographicSize * thisCamera.aspect +
            //    Inventory.invInstance.GetComponent<SpriteRenderer>().sprite.rect.width / 100 - 0.05f,
            //    thisCamera.transform.position.y - thisCamera.orthographicSize * thisCamera.aspect / 2 -
            //    Inventory.invInstance.GetComponent<SpriteRenderer>().sprite.rect.height / 50 + 0.1f + Inventory.invInstance.currentOffsetY,
            //    Inventory.invInstance.transform.position.z);

            Inventory.invInstance.transform.position = new Vector3(thisCamera.transform.position.x,
                (-thisCamera.orthographicSize * Screen.width / Screen.height) / 2 + Inventory.invInstance.currentOffsetY + 0.3f,
                Inventory.invInstance.transform.position.z);

            Debug.Log(thisCamera.pixelWidth);
        }
    }
    IEnumerator ZoomOut() {
        while (thisCamera.orthographicSize <= zoomOutSize) {
            if (thisCamera.enabled) {
                if (Inventory.invInstance != null) {
                    Inventory.invInstance.SetInventory(false);
                    Inventory.invInstance.GetComponent<SpriteRenderer>().enabled = false;
                    //Inventory.invInstance.transform.localScale = new Vector3(Inventory.invInstance.transform.localScale.x + 0.01f, Inventory.invInstance.transform.localScale.y + 0.01f, Inventory.invInstance.transform.localScale.z);
                }
                thisCamera.transform.position = Vector3.MoveTowards(thisCamera.transform.position, center, 1.2f * Time.deltaTime);
                thisCamera.orthographicSize += zoomOutSpeed;
                forceCameraBorder();
            }
            yield return 0;
        }
        followTarget = true;
        if (thisCamera.orthographicSize >= zoomOutSize && !hasScaled) {
            float newScale = (width / originalWidth);
            Inventory.invInstance.transform.localScale = new Vector3(Inventory.invInstance.transform.localScale.x * newScale - 0.01f, Inventory.invInstance.transform.localScale.y * newScale, transform.localScale.z);
            //Inventory.invInstance.SendMessage("SetPositions");
            Inventory.invInstance.GetComponent<SpriteRenderer>().enabled = true;
            hasScaled = true;
        }

    }
}
