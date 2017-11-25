using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Utils;

public class PlatformTrail : DynamicObject {

    private Material material;
    private float timeSinceLastTailUpdate;
    private static float trailUpdateCoolDown = 1f;

    // The platform this trail is following
    public Platform platform;

    // Use this for initialization
    void Start () {
        this.material = Resources.Load<Material>("PlatformMaterial");
	}

    public void UpdateTrail(Mesh mesh)
    {

        if (Time.time - this.timeSinceLastTailUpdate > trailUpdateCoolDown)
        {
            this.timeSinceLastTailUpdate = Time.time;
            this.meshFilter.mesh = mesh;
        }
    }
}
