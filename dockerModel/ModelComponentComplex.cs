using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dockerModel
{
    public abstract class ModelComponentComplex:ModelComponentBase
    {
        public List<ModelComponentBase> children=new List<ModelComponentBase>();
        public void AddComponent(ModelComponentBase component)
        {
            children.Add(component);
        }
        public override void FillTimeline(float maxtime)
        {
            foreach (var child in children)
                child.FillTimeline(maxtime);
        }
        public override string GetFunctionalTotal(float time)
        {
            string res = "";
            foreach (var child in children)
                res += child.GetFunctionalTotal(time);
            return res;
        }
    }
}
