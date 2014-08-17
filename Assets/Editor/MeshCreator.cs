using UnityEngine;
using UnityEditor;
using System.Collections;

public static class MeshCreator {

	[MenuItem("Custom/Gen8")]
	public static void Gen8() { GenMesh(8); }
	
	[MenuItem("Custom/Gen32")]
	public static void Gen32() { GenMesh(32); }
	
	[MenuItem("Custom/Gen128")]
	public static void Gen128() { GenMesh(128); }
	
	public static void GenMesh(int size) {
		var nEdges = size - 1;
		var vertices = new Vector3[size * size];
		var triangles = new int[6 * nEdges * nEdges];
		var uv = new Vector2[vertices.Length];

		var counter = 0;
		var dx = 1f / nEdges;
		for (var y = 0; y < size; y++) {
			for (var x = 0; x < size; x++) {
				vertices[counter] = new Vector3(x * dx, y * dx, 0f);
				uv[counter] = new Vector2(x * dx, y * dx);
				counter++;
			}
		}

		counter = 0;
		for (var y = 0; y < nEdges; y++) {
			for (var x = 0; x < nEdges; x++) {
				var v = x + y * size;
				triangles[counter++] = v;
				triangles[counter++] = v + size + 1;
				triangles[counter++] = v + 1;
				triangles[counter++] = v;
				triangles[counter++] = v + size;
				triangles[counter++] = v + size + 1;
			}
		}

		var mesh = new Mesh();
		mesh.name = string.Format("Plane {0}x{0}", size);
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		AssetDatabase.CreateAsset(mesh, string.Format("Assets/{0}.asset", mesh.name));
	}
}
