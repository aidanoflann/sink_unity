using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Utils
{
    public class Angle
    // morealess a float, but with certain rules constraining its value to within a circle and adding/subtracting correctly
    {
        private float value;
        private const float degreesInCircle = 360f;

        public Angle(float value)
        {
            // TODO: check what happens if value is negative
            this.SetValue(value % degreesInCircle);
        }

        public void SetValue(float value)
        {
            this.value = value;
        }

        public float GetValue()
        {
            return this.value;
        }

        public static Angle operator +(Angle left, Angle right)
        {
            return new Angle(left.value + right.value);
        }

        public static Angle operator -(Angle left, Angle right)
        {
            // needs to handle e.g. case where left is 5 degrees and right is 355 degrees (result should be 10)
            float newValue = left.value - right.value;
            if (Mathf.Abs(newValue) > degreesInCircle * 0.5)
            {
                // use 360 - result
                newValue = 360f - newValue;
                // flip the sign
                newValue = -newValue;
            }
            return new Angle(newValue);
        }

        public static Angle operator *(float left, Angle right)
        {
            return new Angle(right.value * left);
        }

        public static Angle operator *(Angle left, float right)
        {
            return right * left;
        }

        public static Angle operator /(Angle left, Angle right)
        {
            return new Angle(left.value / right.value);
        }

        public static Angle operator +(Angle left, float right)
        {
            return new Angle(left.value + right);
        }

        public static Angle operator +(float left, Angle right)
        {
            return left + right;
        }

        public static Angle operator -(Angle left, float right)
        {
            return new Angle(left.value - right);
        }

        public static Angle operator -(float left, Angle right)
        {
            return new Angle(left - right.value);
        }

        public static bool operator <(Angle left, Angle right)
        {
            return left.value < right.value;
        }

        public static bool operator >(Angle left, Angle right)
        {
            return right < left;
        }

        public float Sine()
        {
            return Mathf.Sin(this.value * Globals.degreesToRadians);
        }

        public float Cosine()
        {
            return Mathf.Cos(this.value * Globals.degreesToRadians);
        }
    }
}
