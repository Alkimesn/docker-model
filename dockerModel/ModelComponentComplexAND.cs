using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dockerModel
{
    public class ModelComponentComplexAND:ModelComponentComplex
    {
        public override bool IsFunctional(float time)
        {
            foreach (var child in children)
                if (!child.IsFunctional(time))
                    return false;
            return true;
        }
    }
}
