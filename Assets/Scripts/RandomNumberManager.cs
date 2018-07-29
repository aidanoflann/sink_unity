using Assets.Utils;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    class RandomNumberManager: SingletonBehaviour
    {
        System.Random random;
        int seed;

        public new void Awake()
        {
            base.Awake();
            this.seed = 42;
            this.Reset();
        }

        public int Range(int lowerBound, int upperBound)
        {
            return this.random.Next(lowerBound, upperBound);
        }

        public void Reset()
        {
            this.random = new System.Random(this.seed);
        }
    }
}
