using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dockerModel
{
    static class ModelUtils
    {
        static Random r = new Random();
        static float delta = 0.001f;
        public static float GetNextWithProbability(float prob, float maxTime)
        {
            float cur = 0;
            float newprob = prob * delta;
            while(cur<maxTime)
            {
                if (r.NextDouble() < newprob) return cur;
                cur += delta;
            }
            return -1;
        }
    }
}
