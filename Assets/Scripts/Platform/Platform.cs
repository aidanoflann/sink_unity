using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Utils;

public class Platform : DynamicObject {

    //TODO: these are public for collision checks - would rather they weren't
    public float r_size;
    public Angle w_size;
    public float r_pos;
    public float r_vel;
    public Angle w_pos;
    public Angle w_vel;

    public bool hasPlayer;
    public bool hadPlayer;

    private int num_points;

    private Material material;

    // Use this for initialization
    void Start () {
		//static attributes
		r_size = 0.6f;

		Vector2[] points = CalculateAnnulusPoints ();
        this.material = Resources.Load<Material>("PlatformMaterial");

        createMesh (points, this.material);
	}

	protected Vector2[] CalculateAnnulusPoints()
	{
        //edge points of annulus with gap
        this.num_points = 100;
        
		Vector2[] points = new Vector2 [this.num_points * 2];
        for (int x = 0; x < this.num_points; x++) {
            //TODO: disappearing issue starts exactly when w_pos gets > w_size
            Angle d_w = ((this.w_size) * ((float)x / (float)this.num_points) - this.w_size * 0.5f + this.w_pos);

            // outer circle
			points [x].x = (this.r_pos + this.r_size * 0.5f) * d_w.Cosine();
			points [x].y = (this.r_pos + this.r_size * 0.5f) * d_w.Sine();

            // inner circle
			points [this.num_points * 2 - 1 - x].x = (this.r_pos - this.r_size * 0.5f) * d_w.Cosine();
			points [this.num_points * 2 - 1 - x].y = (this.r_pos - this.r_size * 0.5f) * d_w.Sine();
		}

		return points;
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

    public void RecalculateMesh() {
        // update annulus based on new points
        Vector2[] points = CalculateAnnulusPoints();
        updateMesh(points);
    }

    public void CatchPlayer(Player player)
    {
        this.SetColour(player.CurrentColour);
        this.hasPlayer = true;
        this.hadPlayer = true;
    }

    public void ReleasePlayer()
    {
        this.ResetColour();
        this.hasPlayer = false;
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
