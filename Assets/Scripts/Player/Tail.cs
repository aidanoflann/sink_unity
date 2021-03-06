﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tail : DynamicObject {

    public Player player;

    private float tailUpdateCooldown = 0.002f;
    private List<Vector2> _tailPositions = new List<Vector2>();
    private static int numTailPoints = 40;
    private Vector2[] _meshEdges = new Vector2[numTailPoints * 2 - 1];
    private float[] _tailWidth = new float[numTailPoints];
    private float tailWidthAtPlayer = 0.6f;
    private float timeSinceLastTailUpdate;

    public void SetPlayerPosition()
    {
        for (int i = 0; i < numTailPoints; i++)
        {
            if (i != numTailPoints - 1)
            {
                // set the position to the previous position of the next point
                this._tailPositions[i] = this._tailPositions[i + 1];
            }
            else
            {
                // set the position of the final point to the player's current position
                this._tailPositions[i] = new Vector2(this.player.transform.position.x,
                                                     this.player.transform.position.y);
            }

            // Update the edges of the mesh
            if (i == 0)
            {
                // tail point has zero width, just set its edge to the tail position
                _meshEdges[numTailPoints - 1] = _tailPositions[0];
            }
            else
            {
                // calculate normal vector to the line from the previous to current point
                // TODO these can be stored - only need to calculate one per update
                Vector2 fromPreviousToCurrent = _tailPositions[i] - _tailPositions[i - 1];
                Vector2 normalVector = new Vector2(-fromPreviousToCurrent.y,
                                                   fromPreviousToCurrent.x);

                _meshEdges[numTailPoints + i - 1] = _tailPositions[i] + normalVector.normalized * this._tailWidth[i];
                _meshEdges[numTailPoints - i - 1] = _tailPositions[i] - normalVector.normalized * this._tailWidth[i];
            }
        }
        this.updateMesh(this._meshEdges);
    }

    private void GenerateTail()
    {
        for (int i = 0; i < numTailPoints; i++)
        {
            Vector3 newPoint = new Vector2();
            this._tailPositions.Add(newPoint);
            this._tailWidth[i] = this.tailWidthAtPlayer * i / numTailPoints;
        }
        this.createMesh(this._meshEdges, Resources.Load<Material>("PlayerTailMaterial"));
    }

	// Use this for initialization
	void Awake () {
        this.currentColour = new Color(0.4f, 0.4f, 0.9f);
        this.GenerateTail();
        this.timeSinceLastTailUpdate = Time.time;
    }
	
	// Update is called once per frame
	public void UpdateTail () {

        if (Time.time - this.timeSinceLastTailUpdate > this.tailUpdateCooldown)
        {
            this.timeSinceLastTailUpdate = Time.time;
            this.SetPlayerPosition();
        }
	}
}
