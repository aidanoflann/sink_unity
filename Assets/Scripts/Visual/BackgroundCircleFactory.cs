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
            this.GenerateRandomCircles(colour);
        }
        else
        {
            this.SetPositionsAndColours(colour);
        }
    }

    private void GenerateRandomCircles(Color colour)
    {
        for (int i = 0; i < _numCopies; i++)
        {
            // generate a copy
            GameObject copy = GameObject.Instantiate(this.originalCircle);

            // cache it
            this._copies.Add(copy);
            copy.transform.SetParent(this.backgroundCircleHolder);
        }
        this.SetPositionsAndColours(colour);
    }

    private void SetPositionsAndColours(Color colour)
    {
        // move it to a random location
        // TODO: do more, smaller circles near the center, then larger and larger further away (to a much larger distance)

        for (int i = 0; i < this._copies.Count; i++)
        {
            GameObject copy = this._copies[i];
            Vector3 randomPosition = new Vector3(Random.value * 200 - 100, Random.value * 200 - 100, 1);
            copy.transform.position = randomPosition;

            // give it a random size
            float randomScalar = (Random.value * 3) + 1;
            Vector3 randomScale = new Vector3(randomScalar, randomScalar, 0);
            copy.transform.localScale = randomScale;


            // make it visible and set its colour
            SpriteRenderer copyRenderer = copy.GetComponent<SpriteRenderer>();
            copyRenderer.enabled = true;
            copyRenderer.color = colour;
        }
        // TODO: start from r = 0 and extend out to r = 1000 in increasingly large stepsZ
    }

}
