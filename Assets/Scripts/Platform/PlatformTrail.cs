using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Utils;

public class PlatformTrail : AnnulusShapedObject
{
    private float timeSinceLastTailUpdate;
    private static float trailUpdateCoolDown = 1f;

    // Use this for initialization
    void Awake() {
        // TODO: change to specific trail material
        this.material = Resources.Load<Material>("PlatformMaterial");
        this.meshFilter = GetComponent<MeshFilter>();
    }

    public void UpdateTrail(Vector2[] annulusPoints)
    {
        // if the set duration of refreshing the trail has passed...
        if (Time.time - this.timeSinceLastTailUpdate > trailUpdateCoolDown)
        {
            // reset the last refresh time and trigger the actual trail recalculation
            this.timeSinceLastTailUpdate = Time.time;
            this.updateMesh(annulusPoints);
        }
    }

    public void SetPlatform(Vector2[] initialAnnulusPoints)
    {
        this.createMesh(initialAnnulusPoints, this.material);
    }
}
