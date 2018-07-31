using Assets.Utils;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class RandomNumberManager: SingletonBehaviour
    {
        System.Random random;
        int seed;

        public int Seed
        {
            get
            {
                return this.seed;
            }
        }

        public void SetSeed(int seedInt)
        {
            this.seed = seedInt;
        }

        public new void Awake()
        {
            base.Awake();
            this.Reset(true);
        }

        public int Range(int lowerBound, int upperBound)
        {
            return this.random.Next(lowerBound, upperBound);
        }

        private void SetNewSeed()
        {
            // random int: https://stackoverflow.com/questions/1785744/how-do-i-seed-a-random-class-to-avoid-getting-duplicate-random-values
            this.seed = Math.Abs(Guid.NewGuid().GetHashCode()) / 1000000;
        }

        public void Reset(bool newSeed = false)
        {
            if (newSeed)
            {
                this.SetNewSeed();
            }
            this.random = new System.Random(this.seed);
        }
    }
}
