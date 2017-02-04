using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TailFactory : MonoBehaviour {

    private Vector3 _playerPosition;
    private List<GameObject> _tailSpriteCopies = new List<GameObject>();
    private int numTailSprites = 100;
    private int indexOfOldestSprite = 0;
    private float timeSinceLastTailUpdate;
    private float tailUpdateCooldown = 0.0001f;

    public GameObject originalTailSprite;

    public void Awake()
    {
        this.GenerateAllTailSprites();
    }

    public void SetPlayerPosition(Vector3 playerPosition)
    {
        this._playerPosition = playerPosition;
        if (Time.time - this.timeSinceLastTailUpdate > this.tailUpdateCooldown)
        {
            this.UpdateTail();
            this.timeSinceLastTailUpdate = Time.time;
        }
    }

    private void UpdateTail()
    {
        // keep track of an iterator that is always the index of the least-recently-changed sprite
        // set the position of this sprite to the player's current position
        this._tailSpriteCopies[indexOfOldestSprite].transform.position = this._playerPosition;
        this.indexOfOldestSprite = (this.indexOfOldestSprite + 1) % this.numTailSprites;
    }

    private void GenerateAllTailSprites()
    {
        for (int i = 0; i < numTailSprites; i++)
        {
            GameObject copy = GameObject.Instantiate(this.originalTailSprite);
            copy.transform.position = new Vector3();
            //copy.transform.SetParent(this.transform);
            _tailSpriteCopies.Add(copy);
        }
    }
}
