using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LevelManager : MonoBehaviour {

	// prepare the player and platform prefabs
	public GameObject playerPrefab;
	public GameObject platformPrefab;

    // cached GameObjects
    private List<GameObject> platformList;
    private GameObject player;

	// this is used to child all of the gameObjects for better control/organisation in the inspector.
	private Transform levelHolder;

    // platform parameter ranges
    private float[] wPosRange;
    private float[] wVelRange;
    private float[] wSizeRange;

    private void GeneratePlatformRanges()
    {
        wPosRange = Enumerable.Range(0, 359).Select(i => (float)i).ToArray();
        wVelRange = Enumerable.Range(4, 12).Select(i => (float)i * 10f).ToArray();
        wSizeRange = Enumerable.Range(3, 30).Select(i => (float)i*10f).ToArray();
    }

	private void SpawnPlatforms(int numPlatforms)
	{
		bool clockwise = true;
		for (int p=0; p < numPlatforms; p++) {
			
			GameObject toInstantiate = platformPrefab;
			GameObject instance = Instantiate (toInstantiate) as GameObject;

			// grab the script
			Platform platform = instance.GetComponent<Platform> ();

            // TODO: Find a way to get these into an init function - doing so as normal changes the values of the prefab, not the instance
            platform.w_size = wSizeRange[Random.Range(0, wSizeRange.Length - 1)];
			platform.w_pos = wPosRange[Random.Range(0, wPosRange.Length - 1)];
            platform.w_vel = wVelRange[Random.Range(0, wVelRange.Length - 1)];
            if (clockwise)
				platform.w_vel *= 1f;
			else
				platform.w_vel *= -1f;

            platform.r_pos = 2f * (float)(p + 1);
            platform.r_vel = -0.5f;
            clockwise = !clockwise;

			instance.transform.SetParent (levelHolder);
            platformList.Add(instance);
        }
	}

	private void SpawnPlayer()
	{
		GameObject toInstantiate = playerPrefab;
		player = Instantiate (toInstantiate);
        player.transform.SetParent (levelHolder);
	}

    public void Awake()
    {
        levelHolder = new GameObject("Level").transform;
        platformList = new List<GameObject>();
        GeneratePlatformRanges();
    }

	public void SetupScene(int numPlatforms)
	{
        SpawnPlatforms (numPlatforms);
		SpawnPlayer ();
	}

    public void Update()
    {
        // despawn platforms as required
        for (int i = 0; i < platformList.Count; i++)
        {
            GameObject platformObject = platformList[i];
            Platform platform = platformObject.GetComponent<Platform>();
            if (platform.r_pos <= 0)
            {
                Destroy(platformObject);
                platformList.Remove(platformObject);
            }
        }
    }

    public void Clear()
    {
        // clear out all platforms & player
        for (int i = 0; i < platformList.Count; i++)
        {
            GameObject platformObject = platformList[i];
            Destroy(platformObject);
        }
        platformList.Clear();
        Destroy(player);
    }
}
