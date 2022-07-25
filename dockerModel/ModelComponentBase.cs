using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dockerModel
{
    public abstract class ModelComponentBase
    {
        public string Name { get; set; }
        public abstract bool IsFunctional(float time);
        public abstract void FillTimeline(float maxtime);
        public abstract string GetFunctionalTotal(float time);
    }
}
