using Assets.Scripts;
using Assets.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedInputBehaviour : MonoBehaviour {
    
    public InputField inputField;

    private RandomNumberManager randomNumberManager;

    public void Awake()
    {
        this.randomNumberManager = SingletonBehaviour.GetSingletonBehaviour<RandomNumberManager>();
    }

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
                // this.randomNumberManager is null if editing the Seed after returning to menu (but doesn't throw a nullref error...)
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
