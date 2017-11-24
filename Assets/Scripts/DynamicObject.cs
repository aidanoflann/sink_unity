using UnityEngine;
using System.Collections;
using Assets.Utils;

public class DynamicObject : MonoBehaviour {
    
    protected MeshFilter meshFilter;
    protected MeshRenderer meshRenderer;

    private int pointCount;
    private Vector3[] vertices;
    private Triangulator triangulator;
    
    protected Color oldColour;
    protected Color currentColour;

    public Color CurrentColour
    {
        get
        {
            return currentColour;
        }
    }

    protected void createMesh(Vector2[] points, Material material)
	{
		this.pointCount = points.Length;

		// add a MeshRenderer (cannot share these between prefab clones)
		this.meshRenderer = gameObject.AddComponent<MeshRenderer>();
        material.SetColor("_Color", this.currentColour);
        this.meshRenderer.material = material;

		this.meshFilter = GetComponent<MeshFilter>();
		Mesh mesh = this.meshFilter.mesh;
		this.vertices = new Vector3[pointCount];

		for(int j=0; j<pointCount; j++){
			Vector2 actual = points[j];
			vertices[j] = new Vector3(actual.x, actual.y, -0.1f);
		}

		this.triangulator = new Triangulator(points);
		int [] triangles = triangulator.Triangulate();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
        this.meshFilter.mesh = mesh;
    }

    public virtual int[] GenerateTriangles(Vector2[] points)
    // Use an overkill method to generate triangles. Ideally for each subclass, this is overridden.
    {
        this.triangulator.UpdatePoints(points);
        return this.triangulator.Triangulate();
    }

	protected void updateMesh(Vector2[] points)
	{
		Mesh mesh = this.meshFilter.mesh;

		for(int j=0; j<pointCount; j++){
			Vector2 actual = points[j];
            this.vertices[j].x = actual.x;
            this.vertices[j].y = actual.y;
		}

		mesh.vertices = this.vertices;
		mesh.triangles = GenerateTriangles(points);
        mesh.RecalculateBounds();
        this.meshFilter.mesh = mesh;
        this.meshRenderer.material.SetColor("_Color", this.currentColour);
    }

	public void UpdateTransform (float r_pos, Angle w_pos)
	{
		// update x, y position
		Vector3 pos = transform.position;
		pos.x = r_pos * w_pos.Cosine();
		pos.y = r_pos * w_pos.Sine();
		transform.position = pos;

		// update rotation
		Quaternion rot = transform.rotation;
		rot.eulerAngles = new Vector3 (0, 0, w_pos.GetValue());
		transform.rotation = rot;
	}
}
