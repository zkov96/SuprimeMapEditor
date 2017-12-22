using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MyTearrain;
using UnityEngine;

public class MyTerrain : MonoBehaviour
{
    public const int maxChankSize = 254;
    
    [SerializeField] private Mesh mesh;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshCollider meshCollider;
    [SerializeField] private MeshRenderer meshRenderer;
    
    public Material[] materials
    {
        get { return meshRenderer.materials; }
        set { meshRenderer.materials = value; }
    }
    
    public float this[int x, int y]
    {
        get { return vertices[x+y*(int)rect.width].y; }
        set
        {
            vertices[x+y*(int)rect.width] = new Vector3(x,value,y);
//            Debug.LogFormat("Setted ({0},{1})", transform.position.x/254+pos.x, transform.position.z/254+pos.y);
        }
    }

    private Vector2 pos;
    
    private List<Vector3> vertices;

    public Rect rect;

    private void Start()
    {
        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
        }

        if (meshCollider == null)
        {
            meshCollider = GetComponent<MeshCollider>();
        }
        
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        if (mesh == null)
        {
            mesh = new Mesh();
        }
    }

    public void Init(float[,] heights, Vector3 size)
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.MarkDynamic();
        }
        
        vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        int width = (int) size.x - 1;

        for (int k = 0; k < size.x; k++)
        {
            for (int i = 0; i < size.z; i++)
            {
                vertices.Add(new Vector3(i, heights[i, k] * size.y, k));
                uvs.Add(new Vector2(i + transform.position.x, k + transform.position.z));
            }
        }

        for (int k = 0; k < size.z - 1; k++)
        {
            for (int i = 0; i < size.x - 1; i++)
            {
                triangles.AddRange(new[] {width * k + i, width * (k + 1) + i, width * k + i + 1, width * k + i + 1, width * (k + 1) + i, width * (k + 1) + i + 1});
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    public void Init(float[,] heights, Rect rect)
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.MarkDynamic();
        }

        this.rect = rect;
        pos = new Vector2(Mathf.RoundToInt(rect.x/maxChankSize), Mathf.RoundToInt(rect.y/maxChankSize));
        
        vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();
        
        int width = (int) (rect.max.x - rect.min.x);

        for (int k = 0; k < rect.max.y - rect.min.y; k++)
        {
            for (int i = 0; i < rect.max.x - rect.min.x; i++)
            {
                vertices.Add(new Vector3(i, heights[(int) (i + rect.min.x), (int) (k + rect.min.y)], k));
                uvs.Add(new Vector2(i + transform.position.x, k + transform.position.z));
            }
        }

        for (int kt = 0; kt < rect.max.y - rect.min.y - 1; kt++)
        {
            for (int it = 0; it < rect.max.x - rect.min.x - 1; it++)
            {
                triangles.AddRange(new[] {width * kt + it, width * (kt + 1) + it, width * kt + it + 1, width * kt + it + 1, width * (kt + 1) + it, width * (kt + 1) + it + 1});
            }
        }
        
        mesh.SetVertices(vertices);

        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    public void Apply()
    {
        mesh.SetVertices(vertices);
//        mesh.UploadMeshData(false);
        mesh.RecalculateNormals();
        Debug.LogFormat("Applyed ({0},{1})", transform.position.x/254, transform.position.z/254);
    }
}