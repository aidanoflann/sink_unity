using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Platform : DynamicObject {

	//TODO: these are public for collision checks - would rather they weren't
	public float r_size { get; private set;}
	public float w_size { get;  set;}
	public float r_pos { get; set;}
	public float r_vel { get; set;}
	public float w_pos { get; set;}
	public float w_vel { get; set;}

    private Color oldColour;

	// Use this for initialization
	void Start () {
		//static attributes
		r_size = 0.2f;

		Vector2[] points = CalculateAnnulusPoints ();

		createMesh (points, Resources.Load<Material>("PlatformMaterial"));
	}

	protected Vector2[] CalculateAnnulusPoints()
	{
		//edge points of annulus with gap
		int num_points = (int)(w_size / 5f) + 1;

		float[] d_w = new float[num_points];
		Vector2[] points = new Vector2 [num_points * 2];

		for (int x = 0; x < num_points; x++) {
			d_w [x] = (x * 5 - w_size / 2f + w_pos) * Globals.degreesToRadians;
			points [x].x = (r_pos + r_size * 0.5f) * Mathf.Cos (d_w [x]);
			points [x].y = (r_pos + r_size * 0.5f) * Mathf.Sin (d_w [x]);

			points [num_points * 2 - 1 - x].x = (r_pos - r_size * 0.5f) * Mathf.Cos (d_w [x]);
			points [num_points * 2 - 1 - x].y = (r_pos - r_size * 0.5f) * Mathf.Sin (d_w [x]);
		}

		return points;
	}
    
    public void UpdateMesh() {
		// update annulus based on new points
		Vector2[] points = CalculateAnnulusPoints ();
		updateMesh (points);
	}

    public void SetColour(Color colour)
    {
        this.oldColour = this.meshRenderer.material.GetColor("_Color");
        this.meshRenderer.material.SetColor("_Color", colour);
    }

    public void ResetColour()
    {
        this.meshRenderer.material.SetColor("_Color", this.oldColour);
    }
}
