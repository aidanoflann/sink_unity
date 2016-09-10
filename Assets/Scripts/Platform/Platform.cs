using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Platform : DynamicObject {

    //TODO: these are public for collision checks - would rather they weren't
    public float r_size;
    public float w_size;
    public float r_pos;
    public float r_vel;
    public float w_pos;
    public float w_vel;

    private int num_points;

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
		this.num_points = (int)(this.w_size / 5f) + 1;

		float[] d_w = new float[this.num_points];
		Vector2[] points = new Vector2 [this.num_points * 2];
        for (int x = 0; x < this.num_points; x++) {
            //TODO: disappearing issue starts exactly when w_pos gets > w_size
			d_w [x] = (x * 5 - this.w_size * 0.5f + this.w_pos) * Globals.degreesToRadians;
			points [x].x = (this.r_pos + this.r_size * 0.5f) * Mathf.Cos (d_w [x]);
			points [x].y = (this.r_pos + this.r_size * 0.5f) * Mathf.Sin (d_w [x]);

			points [this.num_points * 2 - 1 - x].x = (this.r_pos - this.r_size * 0.5f) * Mathf.Cos (d_w [x]);
			points [this.num_points * 2 - 1 - x].y = (this.r_pos - this.r_size * 0.5f) * Mathf.Sin (d_w [x]);
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
        this.oldColour = this.currentColour;
        this.currentColour = colour;
    }

    public void ResetColour()
    {
        this.currentColour = this.oldColour;
    }
}
