using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT WAS MADE USING ASSISTANCE FROM BRACKEYS TUTORIALS

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour {

    Color[] color;
    Vector3[] vertices;
    int[] triangles;

    private float minTerrain;
    private float maxTerrain;
    public int xSize = 80;
    public int zSize = 80;

    Mesh mesh;
    public Gradient gradient;

    public GameObject treeFab;

    //Creating new mesh, getting mesh component and calling functions
    void Start () {
        mesh = new Mesh(); 
        GetComponent<MeshFilter>().mesh = mesh;

        createShape();
        updateMesh();
	}

    /// <summary>
    /// Uses loops to create a grid of vertices to create a plane and fills in grid with triangles. Perlin noise is added to create random unevenness
    /// </summary>
    void createShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int z = 0, i = 0; i <= zSize; i ++)
        {
            for (int j = 0; j <= xSize; j ++)
            {
                float perlinY = Mathf.PerlinNoise(j * 0.2f, i * 0.2f) * 2f;
                vertices[z] = new Vector3(j, perlinY, i);
                z++;

                if (perlinY > maxTerrain) { maxTerrain = perlinY; }
                if (perlinY < minTerrain) { minTerrain = perlinY; }
            }
        }

        triangles = new int[6 * xSize * zSize];
        int triangs = 0;
        int vertex = 0;

        for (int y = 0; y < zSize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {

                triangles[0 + triangs] = vertex + 0;
                triangles[1 + triangs] = vertex + xSize + 1;
                triangles[2 + triangs] = vertex + 1;
                triangles[3 + triangs] = vertex + 1;
                triangles[4 + triangs] = vertex + xSize + 1;
                triangles[5 + triangs] = vertex + xSize + 2;

                vertex++;
                triangs += 6;
            }
            vertex++;
        }

        color = new Color[vertices.Length];
        int tempI = 0;
        for (int k = 0; k <= zSize; k++)
        {
            for (int l = 0; l <= xSize; l++)
            {
                float height = Mathf.InverseLerp(minTerrain, maxTerrain, vertices[tempI].y);
                color[tempI] = gradient.Evaluate(height);

                tempI++;
            }
        }
    }

    void updateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        mesh.colors = color;
    }
}
