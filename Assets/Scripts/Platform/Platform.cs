using UnityEngine;

public class Platform : AnnulusShapedObject {
    public GameObject platformTrailPrefab;
    private PlatformTrail platformTrail;

    public bool hasPlayer;
    public bool hadPlayer;

    //edge points of annulus with gap
    private static int numPoints = 100;

    // Use this for initialization
    void Start () {
		//static attributes
		r_size = 0.6f;

		Vector2[] points = CalculateAnnulusPoints ();
        this.material = Resources.Load<Material>("PlatformMaterial");

        createMesh (points, this.material);

        // generate the tail gameobject
        GameObject toInstantiate = platformTrailPrefab;
        GameObject instance = Instantiate(toInstantiate) as GameObject;
        // grab the behaviour
        this.platformTrail = instance.GetComponent<PlatformTrail>();
        this.platformTrail.SetPlatform(this);
        // set self as parent transform
        instance.transform.SetParent(this.transform);
    }

    public void RecalculateMesh() {
        // update annulus based on new points
        Vector2[] points = CalculateAnnulusPoints();
        updateMesh(points);
        this.platformTrail.UpdateTrail();
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
}
