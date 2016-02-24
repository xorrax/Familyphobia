using UnityEngine;
using System.Collections;

public class PanningCamera : CameraMovement {

    private Camera thisCamera;
    private Vector2 max;
    private Vector2 min;
    Vector3 bottomLeft;
    float height;
    float width;
    Vector2 minOffset;
    Vector2 maxOffset;
    public float moveSpeed = 0.5f;




    protected override void Start() {
        thisCamera = GetComponent<Camera>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

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
        if (Input.GetKey("left")) {
            thisCamera.transform.position = new Vector3(thisCamera.transform.position.x - moveSpeed, thisCamera.transform.position.y, -10);
        }
        if (Input.GetKey("right")) {
            thisCamera.transform.position = new Vector3(thisCamera.transform.position.x + moveSpeed, thisCamera.transform.position.y, -10);
        }

    }
}
