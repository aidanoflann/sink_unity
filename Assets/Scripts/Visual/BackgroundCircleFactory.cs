using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundCircleFactory : MonoBehaviour {

    public GameObject originalCircle;

    private List<GameObject> _copies = new List<GameObject>();
    private int _numCopies = 1;
    
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
    }

    private void SetPositionAndColour(GameObject copy, Color colour)
    {
        // move it to a random location
        Vector3 randomPosition = new Vector3(Random.value * 1000, Random.value * 1000, 0);
        copy.transform.position = randomPosition;

        // TODO: randomise its size

        // make it visible and set its colour
        Renderer copyRenderer = copy.GetComponent<Renderer>();
        copyRenderer.enabled = true;
        copyRenderer.material.color = colour;
    }

}
