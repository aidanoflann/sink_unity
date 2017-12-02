using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Utils;

public class AnnulusShapedObject : DynamicObject {

    //TODO: these are public for collision checks - would rather they weren't
    public float r_size;
    public Angle w_size;
    public float r_pos;
    public float r_vel;
    public Angle w_pos;
    public Angle w_vel;

    // originals, handy to store for certain behaviours
    private float original_r_size;
    private Angle original_w_size = new Angle(0f);
    private float original_r_vel;
    private Angle original_w_vel = new Angle(0f);

    public float OriginalRsize { get { return this.original_r_size; } }
    public Angle OriginalWsize { get { return this.original_w_size; } }
    public float OriginalRvel { get { return this.original_r_vel; } }
    public Angle OriginalWvel { get { return this.original_w_vel; } }

    //edge points of annulus with gap
    private static int numPoints = 100;
    protected Vector2[] annulusPoints;

    protected Material material;

	protected void CalculateAnnulusPoints()
	{
		Vector2[] points = new Vector2 [numPoints * 2];
        for (int x = 0; x < numPoints; x++) {
            //TODO: disappearing issue starts exactly when w_pos gets > w_size
            Angle d_w = ((this.w_size) * ((float)x / (float)numPoints) - this.w_size * 0.5f + this.w_pos);

            // outer circle
			points [x].x = (this.r_pos + this.r_size * 0.5f) * d_w.Cosine();
			points [x].y = (this.r_pos + this.r_size * 0.5f) * d_w.Sine();

            // inner circle
			points [numPoints * 2 - 1 - x].x = (this.r_pos - this.r_size * 0.5f) * d_w.Cosine();
			points [numPoints * 2 - 1 - x].y = (this.r_pos - this.r_size * 0.5f) * d_w.Sine();
		}

		this.annulusPoints = points;
	}

    public override int[] GenerateTriangles(Vector2[] points)
    {
        // first and the last vector are the leftmost inner and outer point
        // number of triangles:
        // one for the leftmost three points
        // then one more for each point right up to the end
        // so num_points = num_vectors - 2
        int numPoints = points.Length;
        int numberOfTriangles = numPoints - 2;
        int[] triangles = new int[numberOfTriangles * 3];
        // draw two triangles (one square) per iteration
        for (int i = 0; i < numberOfTriangles / 2; i++)
        {
            triangles[i * 6] = i;
            triangles[i * 6 + 1] = numPoints - i - 1;
            triangles[i * 6 + 2] = numPoints - i - 2;
            triangles[i * 6 + 3] = i;
            triangles[i * 6 + 4] = numPoints - i - 2;
            triangles[i * 6 + 5] = i + 1;
        }
        return triangles;
    }

    public void SetOriginalValues()
    {
        this.original_r_size = this.r_size;
        this.original_w_size.SetValue(this.w_size.GetValue());
        this.original_r_vel = this.r_vel;
        this.original_w_vel.SetValue(this.w_vel.GetValue());
    }

    public void Log()
    {
        Debug.LogFormat("r_size: {0}, w_size: {1}, r_pos: {2}, r_vel: {3}, w_pos: {4}, w_vel: {5}",
            this.r_size, this.w_size.GetValue(), this.r_pos, this.r_vel, this.w_pos.GetValue(), this.w_vel.GetValue());
    }
}
