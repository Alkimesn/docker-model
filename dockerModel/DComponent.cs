using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dockerModel
{
    class DComponent
    {
        public List<DComponent> outputs { get; private set; } = new List<DComponent>();
        public string name { get; set; }
        public string id { get; set; }

        public DRequest ModifyRequest(DRequest request)
        {
            return request;
        }

        public DResponse ModifyResponse(DResponse response)
        {
            return response;
        }

        public virtual async Task<DResponse> GetResponse(DRequest request)
        {
            if (outputs.Count == 0) return DResponse.CreateErrorResponseNoOutputs(this);
            DRequest newRequest = ModifyRequest(request);
            foreach(var output in outputs)
            {
                DResponse response = await output.GetResponse(newRequest);
                if (!response.isError)
                {
                    DResponse newResponse = ModifyResponse(response);
                    return response;
                }
            }
            return DResponse.CreateErrorResponseAllErrors(this);
        }
    }
}
