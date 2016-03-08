using UnityEngine;
using System.Collections;

public abstract class CameraMovement : MonoBehaviour {

    public GameObject background;
    public Transform target;
    protected Camera thisCamera;
    protected float height;
    protected float width;

    protected virtual void Start() { }
    protected virtual void FixedUpdate() { }
    public abstract float getMinY();
    public abstract float getMaxY();

    public bool isSeenByCamera(GameObject gameobject) {
        Renderer renderer = gameobject.GetComponent<Renderer>();
        float gWidth, gHeight;
        gWidth = renderer.bounds.size.x;
        gHeight = renderer.bounds.size.y;
        Rect gRect = new Rect(gameobject.transform.position, new Vector2(gWidth, gHeight));
        Rect cameraRect = new Rect(new Vector2(thisCamera.transform.position.x - (width / 2), thisCamera.transform.position.y - (height / 2)), 
            new Vector2(width, height));

        if (cameraRect.Overlaps(gRect))
            return true;
        else 
            return false;
    }

    public bool isSeenByCamera(GameObject gameobject, Vector3 newPosition) {
        Renderer renderer = gameobject.GetComponent<SpriteRenderer>();
        float gWidth, gHeight;
        gWidth = renderer.bounds.size.x;
        gHeight = renderer.bounds.size.y;
        Rect gRect = new Rect(newPosition, new Vector2(gWidth, gHeight));
        Rect cameraRect = new Rect(new Vector2(thisCamera.transform.position.x - (width / 2), thisCamera.transform.position.y - (height / 2)),
            new Vector2(width, height));

        if (cameraRect.Overlaps(gRect))
            return true;
        else
            return false;
    }
}