using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundCircleFactory : MonoBehaviour {

    public GameObject originalCircle;

    private List<GameObject> _copies = new List<GameObject>();
    private Transform backgroundCircleHolder;
    private int _numCopies = 200;
    
    void Awake()
    {
        this.backgroundCircleHolder = new GameObject("BackgroundCircles").transform;
    }

    public void GenerateBackgroundCircles(Color colour)
    {
        if (this._copies.Count == 0)
        {
            for (int i = 0; i < _numCopies; i++)
            {
                this.GenerateRandomCircle(colour);
            }
        }
        else
        {
            for(int i = 0; i < this._copies.Count; i++)
            {
                this.SetPositionAndColour(this._copies[i], colour);
            }
        }
    }

    private void GenerateRandomCircle(Color colour)
    {
        // generate a copy
        GameObject copy = GameObject.Instantiate(this.originalCircle);

        this.SetPositionAndColour(copy, colour);
        
        // cache it
        this._copies.Add(copy);
        copy.transform.SetParent(this.backgroundCircleHolder);
    }

    private void SetPositionAndColour(GameObject copy, Color colour)
    {
        // move it to a random location
        Vector3 randomPosition = new Vector3(Random.value * 200 - 100, Random.value * 200 - 100, 0);
        copy.transform.position = randomPosition;

        // give it a random size
        float randomScalar = (Random.value * 3) + 1;
        Vector3 randomScale = new Vector3(randomScalar, randomScalar, 0);
        copy.transform.localScale = randomScale;

        // TODO: randomise its size

        // make it visible and set its colour
        SpriteRenderer copyRenderer = copy.GetComponent<SpriteRenderer>();
        copyRenderer.enabled = true;
        copyRenderer.color = colour;
    }

}
