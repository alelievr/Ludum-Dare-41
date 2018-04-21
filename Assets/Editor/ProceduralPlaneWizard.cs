using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ProceduralPlaneWizard : ScriptableWizard
{
    public Material     material;

    [Space]
	public int			sizeX;
	public int			sizeY;

	[MenuItem("GameObject/Procedural Plane", false, 20)]
	static void CreateWizard()
	{
        var wizard = ScriptableWizard.DisplayWizard< ProceduralPlaneWizard >("Procedural", "Create");
        wizard.material = new Material(Shader.Find("Diffuse"));
	}

	void OnWizardCreate()
    {
        GameObject go = new GameObject("Terrain");
		MeshRenderer mr = go.AddComponent< MeshRenderer >();
        MeshFilter mf = go.AddComponent< MeshFilter >();

		mf.sharedMesh = GeneratePlane();
        mr.sharedMaterial = material;
    }
	Mesh GeneratePlane()
    {
        Mesh mesh = new Mesh();

        float length = sizeX;
        float width = sizeY;

        #region Vertices		
        Vector3[] vertices = new Vector3[sizeX * sizeY];
        for (int z = 0; z < sizeY; z++)
        {
            // [ -length / 2, length / 2 ]
            float zPos = ((float)z / (sizeY - 1) - .5f) * length;
            for (int x = 0; x < sizeX; x++)
            {
                // [ -width / 2, width / 2 ]
                float xPos = ((float)x / (sizeX - 1) - .5f) * width;
                vertices[x + z * sizeX] = new Vector3(xPos, 0f, zPos);
            }
        }
        #endregion

        #region Normales
        Vector3[] normales = new Vector3[vertices.Length];
        for (int n = 0; n < normales.Length; n++)
            normales[n] = Vector3.up;
        #endregion

        #region UVs		
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int v = 0; v < sizeY; v++)
        {
            for (int u = 0; u < sizeX; u++)
            {
                uvs[u + v * sizeX] = new Vector2((float)u / (sizeX - 1), (float)v / (sizeY - 1));
            }
        }
        #endregion

        #region Triangles
        int nbFaces = (sizeX - 1) * (sizeY - 1);
        int[] triangles = new int[nbFaces * 6];
        int t = 0;
        for (int face = 0; face < nbFaces; face++)
        {
            // Retrieve lower left corner from face ind
            int i = face % (sizeX - 1) + (face / (sizeY - 1) * sizeX);

            triangles[t++] = i + sizeX;
            triangles[t++] = i + 1;
            triangles[t++] = i;

            triangles[t++] = i + sizeX;
            triangles[t++] = i + sizeX + 1;
            triangles[t++] = i + 1;
        }
        #endregion

        mesh.vertices = vertices;
        mesh.normals = normales;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();

		MeshUtility.Optimize(mesh);

		return mesh;
    }


}
