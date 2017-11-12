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
            float d_w = (x * (this.w_size / this.num_points) - this.w_size * 0.5f + this.w_pos) * Globals.degreesToRadians;

            // outer circle
			points [x].x = (this.r_pos + this.r_size * 0.5f) * Mathf.Cos (d_w);
			points [x].y = (this.r_pos + this.r_size * 0.5f) * Mathf.Sin (d_w);

            // inner circle
			points [this.num_points * 2 - 1 - x].x = (this.r_pos - this.r_size * 0.5f) * Mathf.Cos (d_w);
			points [this.num_points * 2 - 1 - x].y = (this.r_pos - this.r_size * 0.5f) * Mathf.Sin (d_w);
		}

		return points;
	}

    public override int[] GenerateTriangles(Vector2[] points)
    {
        // first and the last vector are the leftmost inner and outer point
        // need to do first, last, first-1
        // then last, first-1, last-1
        // then first-1, last-1, first-2
        // number of triangles:
        // one for the leftmost three points
        // then one more for each point right up to the end
        // so num_points = num_vectors - 2
        int numPoints = points.Length;
        int numberOfTriangles = numPoints - 2;
        int[] triangles = new int[numberOfTriangles * 3];
        triangles[0] = 0;
        triangles[1] = numPoints - 1;
        triangles[2] = 1;
        for (int i = 1; i < numberOfTriangles - 1; i+=3)
        {
            if (i % 2 == 0)
            {
                triangles[i] = i;
                triangles[i + 1] = numPoints - i - 2;
                triangles[i + 2] = i + 1;
            }
            else
                triangles[i] = i + 1;
                triangles[i + 1] = numPoints - i - 1;
                triangles[i + 2] = numPoints - i - 2;
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
