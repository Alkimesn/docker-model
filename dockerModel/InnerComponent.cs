using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dockerModel
{
    class InnerComponent:DComponent
    {
        public Func<DRequest, DResponse> evaluator;
        public override async Task<DResponse> GetResponse(DRequest request)
        {
            return evaluator(request);
        }
    }
}
