using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Math {
  public class Rng {

    static Rng defaultRng;

    public static Rng GetDefaultRng() {
      if (defaultRng == null) {
        defaultRng = new Rng();
      }
      return defaultRng;
    }

    System.Random sysRandom;

    public Rng() {
      this.sysRandom = new System.Random();
    }

    public Rng(int seed) {
      this.sysRandom = new System.Random(seed);
    }

    public void Seed(int seed) {
      this.sysRandom = new System.Random(seed);
    }

    public bool GetBool() {
      return this.sysRandom.Next() % 2 == 0;
    }

    public int GetInt(int max) {
      return this.sysRandom.Next(max);
    }

    public int GetInt(int a, int b) {
      return this.sysRandom.Next(a, b);
    }

    public byte GetByte() {
      return (byte) (this.sysRandom.Next() % 256);
    }

    public float GetFloat(float min, float max) {
      return min + (float)(this.sysRandom.NextDouble() * (max - min));
    }

    public float GetFloat(float max) {
      return (float)this.sysRandom.NextDouble() * max;
    }

  }
}
