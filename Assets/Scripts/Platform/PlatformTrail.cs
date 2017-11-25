using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Utils;

public class PlatformTrail : AnnulusShapedObject
{
    private float timeSinceLastTailUpdate;
    private static float trailUpdateCoolDown = 1f;

    // The platform this trail is following
    private Platform platform;

    // Use this for initialization
    void Start () {
        // TODO: change to specific trail material
        this.meshRenderer = gameObject.AddComponent<MeshRenderer>();
        this.meshRenderer.material = Resources.Load<Material>("PlatformMaterial");
        this.meshFilter = GetComponent<MeshFilter>();
    }

    public void UpdateTrail()
    {
        if (Time.time - this.timeSinceLastTailUpdate > trailUpdateCoolDown)
        {
            this.timeSinceLastTailUpdate = Time.time;
            this.meshFilter.mesh = this.platform.GetMeshFilter().mesh;
        }
    }

    public void SetPlatform(Platform platform)
    {
        this.platform = platform;
    }
}
