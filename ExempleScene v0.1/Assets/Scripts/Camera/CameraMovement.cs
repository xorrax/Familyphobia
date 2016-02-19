using UnityEngine;
using System.Collections;

public abstract class CameraMovement : MonoBehaviour {

    public GameObject background;
    public Transform target;

    protected virtual void Start() { }
    protected virtual void Update() { }
    public abstract float getMinY();
    public abstract float getMaxY();
}