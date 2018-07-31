using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedInputBehaviour : MonoBehaviour {
    
    public InputField inputField;
    public RandomNumberManager randomNumberManager;

    public void OnTextEditComplete()
    {
        string inputValue = this.inputField.text;
        if(inputValue == "")
        {
            this.inputField.text = "RND";
            this.randomNumberManager.Reset(true);
        }
        else
        {
            int newSeed;
            bool isNumeric = int.TryParse(inputValue, out newSeed);
            if(isNumeric)
            {
                this.randomNumberManager.SetSeed(newSeed);
                this.randomNumberManager.Reset(false);
            }
            else
            {
                this.inputField.text = "RND";
                this.randomNumberManager.Reset(true);
            }
        }
    }

}
