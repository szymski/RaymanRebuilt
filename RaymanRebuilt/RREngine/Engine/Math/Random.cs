using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Math {
  class Random {

    System.Random sysRandom;

    public Random() {
      this.sysRandom = new System.Random();
    }

    public Random(int seed) {
      this.sysRandom = new System.Random(seed);
    }

    public bool getBool() {
      return this.sysRandom.Next() % 2 == 0;
    }

    public int getInt(int max) {
      return this.sysRandom.Next(max);
    }

    public int getInt(int a, int b) {
      return this.sysRandom.Next(a, b);
    }

    public byte getByte() {
      return (byte) (this.sysRandom.Next() % 256);
    }

    public float getFloat(float min, float max) {
      return min + (float)(this.sysRandom.NextDouble() * (max - min));
    }

    public float getFloat(float max) {
      return (float)this.sysRandom.NextDouble() * max;
    }

  }
}
