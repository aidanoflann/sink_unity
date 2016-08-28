using UnityEngine;
using System.Collections;

public class DynamicObject : MonoBehaviour {
    
    protected MeshFilter meshFilter;
    protected MeshRenderer meshRenderer;

    protected void createMesh(Vector2[] points, Material material)
	{
		int pointCount = points.Length;

		// add a MeshRenderer (cannot share these between prefab clones)
		this.meshRenderer = gameObject.AddComponent<MeshRenderer>();
        this.meshRenderer.material = material;

		this.meshFilter = GetComponent<MeshFilter>();
		Mesh mesh = this.meshFilter.mesh;
		Vector3[] vertices = new Vector3[pointCount];

		for(int j=0; j<pointCount; j++){
			Vector2 actual = points[j];
			vertices[j] = new Vector3(actual.x, actual.y, 0);
		}

		Triangulator tr = new Triangulator(points);
		int [] triangles = tr.Triangulate();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
        this.meshFilter.mesh = mesh;
	}

	protected void updateMesh(Vector2[] points)
	{
		int pointCount = points.Length;

		MeshFilter mf = GetComponent<MeshFilter>();
		Mesh mesh = mf.mesh;
		Vector3[] vertices = new Vector3[pointCount];

		for(int j=0; j<pointCount; j++){
			Vector2 actual = points[j];
			vertices[j] = new Vector3(actual.x, actual.y, 0);
		}

		Triangulator tr = new Triangulator(points);
		int [] triangles = tr.Triangulate();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mf.mesh = mesh;
	}

	protected void updateTransform (float r_pos, float w_pos)
	{
		// update x, y position
		Vector3 pos = transform.position;
		pos.x = r_pos * Mathf.Cos(w_pos * Globals.degreesToRadians);
		pos.y = r_pos * Mathf.Sin(w_pos * Globals.degreesToRadians);
		transform.position = pos;

		// update rotation
		Quaternion rot = transform.rotation;
		rot.eulerAngles = new Vector3 (0, 0, w_pos);
		transform.rotation = rot;
	}
}
