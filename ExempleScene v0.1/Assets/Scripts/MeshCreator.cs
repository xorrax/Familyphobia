using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class MeshCreator : MonoBehaviour
{

    public int tileSize;
    public Vector2 size = new Vector2(1, 1);
    public Vector3 offset;
    public float sphereSize = 0.3f;
    public bool save;
    public bool destroy;
    public string meshName = "Mesh Name";

    private Vector3[] newVertices = new Vector3[0];
    public int[] newTriangles = new int[0];
    private Vector2 sizeCheck = Vector2.zero;
    private Vector3 offsetCheck = Vector3.zero;
    private GameObject[] spheres = new GameObject[0];

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (tileSize <= 0)
        {
            tileSize = 1;
        }

        tileCreator();

        tileEditor();

        if (save)
        {
            saveAsset();
            save = !save;
        }
        if (destroy)
        {
            destroyThis();
        }

    }

    void tileCreator()
    {

        if (newVertices.Length != (tileSize - 1) * 2 + 4 || size != sizeCheck || offset != offsetCheck)
        {

            newVertices = new Vector3[(tileSize - 1) * 2 + 4];
            newTriangles = new int[tileSize * 6];

            // Starting square cordinates
            newVertices[0] = new Vector3(0, size.y, 0);
            newVertices[1] = new Vector3(0, 0, 0);
            newVertices[2] = new Vector3(size.x, size.y, 0);
            newVertices[3] = new Vector3(size.x, 0, 0);

            /*
            newVertices[0] = new Vector3(0, 0, 0);
            newVertices[1] = new Vector3(0, size.y, 0);
            newVertices[2] = new Vector3(size.x, 0, 0);
            newVertices[3] = new Vector3(size.x, size.y, 0);
            */

            // Original square
            newTriangles[0] = 0;
            newTriangles[1] = 1;
            newTriangles[2] = 2;
            newTriangles[3] = 1;
            newTriangles[4] = 2;
            newTriangles[5] = 3;

            for (int i = 4; i < newVertices.Length; i++)
            {
                newVertices[i] = newVertices[i - 2] + new Vector3(size.x, 0, 0);

            }

            for (int i = 6; i < newTriangles.Length; i += 3)
            {

                newTriangles[i] = newTriangles[i - 2];
                newTriangles[i + 1] = newTriangles[i - 1];
                newTriangles[i + 2] = newTriangles[i - 1] + 1;


                /*
                newTriangles[i] = newTriangles[i - 2];
                newTriangles[i + 1] = newTriangles[i - 1];
                newTriangles[i + 2] = newTriangles[i - 1] + 1;
                */

            }

            for (int i = 0; i < newTriangles.Length; i += 6)
            {
                int temp = 0;
                temp = newTriangles[i];
                newTriangles[i] = newTriangles[i + 2];
                newTriangles[i + 2] = temp;
            }

            for (int i = 0; i < newVertices.Length; i++)
            {
                newVertices[i] += offset;

            }

            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = newVertices;
            mesh.triangles = newTriangles;
            GetComponent<MeshCollider>().sharedMesh = mesh;

        }

    }

    void tileEditor()
    {

        if (spheres.Length != newVertices.Length || size != sizeCheck || offset != offsetCheck)
        {

            for (int i = 0; i < spheres.Length; i++)
            {
                DestroyImmediate(spheres[i]);
            }

            spheres = new GameObject[newVertices.Length];

            for (int i = 0; i < newVertices.Length; i++)
            {
                spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                spheres[i].transform.localScale = new Vector3(sphereSize, sphereSize, sphereSize);
                spheres[i].transform.position = newVertices[i];
                //spheres[i].GetComponent<MeshRenderer>().enabled = false;
                spheres[i].transform.parent = gameObject.transform;

            }

            sizeCheck = size;
            offsetCheck = offset;
        }

        /*
        * Fixat postionering av meshen genom att modifiera sfärens position så att det håller sig relativt scalingen
        * Just kill me... bytte ut transform.position med transform.localPosition
        */

        for (int i = 0; i < spheres.Length; i++)
        {
            if (newVertices[i] != spheres[i].transform.localPosition)
            {
                newVertices[i] = spheres[i].transform.localPosition;
            }
        }



        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
        GetComponent<MeshCollider>().sharedMesh = mesh;

    }
    //kuk
    void saveAsset()
    {
        Mesh m1 = GetComponent<MeshCollider>().sharedMesh;
       AssetDatabase.CreateAsset(m1, "Assets/" + meshName + ".asset"); // saves to "assets/"
    }

    void destroyThis()
    {
        for (int i = 0; i < spheres.Length; i++)
        {
            DestroyImmediate(spheres[i].gameObject);
        }
        DestroyImmediate(this);
    }

}





