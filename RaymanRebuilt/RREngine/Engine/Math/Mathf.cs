using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Math
{
    public static class Mathf
    {
        public const float PI = (float)System.Math.PI;
        public const float DegToRad = PI / 180f;
        public const float RadToDeg = 180f / PI;

        public static float Acos(float d) => (float)System.Math.Acos(d);
        public static float Asin(float d) => (float)System.Math.Asin(d);
        public static float Atan(float d) => (float)System.Math.Atan(d);
        public static float Atan2(float y, float x) => (float)System.Math.Atan2(y, x);
        public static float Ceiling(float a) => (float)System.Math.Ceiling(a);
        public static float Cos(float d) => (float)System.Math.Cos(d);
        public static float Cosh(float value) => (float)System.Math.Cosh(value);
        public static float Exp(float d) => (float)System.Math.Exp(d);
        public static float Floor(float d) => (float)System.Math.Floor(d);
        public static float IEEERemainder(float x, float y) => (float)System.Math.IEEERemainder(x, y);
        public static float Log(float d) => (float)System.Math.Log(d);
        public static float Log(float a, float newBase) => (float)System.Math.Log(a, newBase);
        public static float Log10(float d) => (float)System.Math.Log10(d);
        public static float Min(float val1, float val2) => System.Math.Min(val1, val2);
        public static float Max(float val1, float val2) => System.Math.Max(val1, val2);
        public static float Pow(float x, float y) => (float)System.Math.Pow(x, y);
        public static float Round(float a) => (float)System.Math.Round(a);
        public static float Round(float value, MidpointRounding mode) => (float)System.Math.Round(value, mode);
        public static float Round(float value, int digits) => (float)System.Math.Round(value, digits);
        public static float Round(float value, int digits, MidpointRounding mode) => (float)System.Math.Round(value, digits, mode);
        public static float Sin(float a) => (float)System.Math.Sin(a);
        public static float Sinh(float value) => (float)System.Math.Sinh(value);
        public static float Sqrt(float d) => (float)System.Math.Sqrt(d);
        public static float Tan(float a) => (float)System.Math.Tan(a);
        public static float Tanh(float value) => (float)System.Math.Tanh(value);
        public static float Truncate(float d) => (float)System.Math.Truncate(d);

        public static float Clamp(float value, float min, float max)
            => value < min ? min : (value > max) ? max : value;
    }
}
