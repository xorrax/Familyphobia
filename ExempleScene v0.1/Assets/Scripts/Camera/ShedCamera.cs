using UnityEngine;
using System.Collections;

public class ShedCamera : CameraMovement {
    private Camera thisCamera;
    private Vector2 max;
    private Vector2 min;
    private float cameraMove = 1.5f;
    private float cameraTrigger = 33f;
    Vector3 bottomLeft;
    float height;
    float width;
    Vector2 minOffset;
    Vector2 maxOffset;

    protected override void Start() {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        thisCamera = GetComponent<Camera>();

        height = 2f * thisCamera.orthographicSize;
        width = height * thisCamera.aspect;

        bottomLeft = new Vector3(background.transform.position.x - (background.GetComponent<Renderer>().bounds.size.x / 2),
            background.transform.position.y - (background.GetComponent<Renderer>().bounds.size.y / 2), 0);

        max.x = ((bottomLeft.x + background.GetComponent<Renderer>().bounds.size.x) - (width / 2));
        max.y = ((bottomLeft.y + background.GetComponent<Renderer>().bounds.size.y) - (height / 2));

        min.x = bottomLeft.x + (width / 2);
        min.y = bottomLeft.y + (height / 2);


    }

    public override float getMinY() {
        return min.y - (height / 2);
    }

    public override float getMaxY() {
        return max.y + (height / 2);
    }

    protected override void FixedUpdate() {
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
                thisCamera.transform.position = new Vector3(thisCamera.transform.position.x - 0.5f, thisCamera.transform.position.y, thisCamera.transform.position.z);
            else
                thisCamera.transform.position = new Vector3(min.x + cameraMove, thisCamera.transform.position.y, thisCamera.transform.position.z);
        }

        if (thisCamera.enabled)
        {
            Inventory.invInstance.transform.position = new Vector3(thisCamera.transform.position.x - thisCamera.orthographicSize * thisCamera.aspect +
                Inventory.invInstance.GetComponent<SpriteRenderer>().sprite.rect.width / 100 - 0.05f,
                Inventory.invInstance.transform.position.y, Inventory.invInstance.transform.position.z);

            Debug.Log(thisCamera.name);
        }
    }
}
