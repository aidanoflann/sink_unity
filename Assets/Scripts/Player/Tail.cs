using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tail : DynamicObject {

    public Player player;

    private List<Vector2> _tailPositions = new List<Vector2>();
    private static int numTailPoints = 5;
    private Vector2[] _meshEdges = new Vector2[numTailPoints * 2 + 1];

public void SetPlayerPosition()
    {
        for (int i = 0; i < numTailPoints - 1; i++)
        {
            _tailPositions[i] = _tailPositions[i + 1];
        }
        _tailPositions[numTailPoints -1] = new Vector2(player.transform.position.x, player.transform.position.y);
        this.updateMesh(this._meshEdges);
    }

    private void GenerateTail()
    {
        for (int i = 0; i < numTailPoints; i++)
        {
            Vector3 newPoint = new Vector2();
            _tailPositions.Add(newPoint);
        }
        this.createMesh(this._meshEdges, Resources.Load<Material>("PlayerTailMaterial"));
    }

	// Use this for initialization
	void Awake () {
        this.currentColour = new Color();
        this.GenerateTail();
    }
	
	// Update is called once per frame
	void Update () {
        this.SetPlayerPosition();
	}
}
