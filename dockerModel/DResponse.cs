using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dockerModel
{
    class DResponse
    {
        public bool isError { get; private set; } = false;
        public List<string> errorMessages { get; private set; } = new List<string>();

        public string data { get; set; }

        public static DResponse CreateErrorResponse(DComponent component, string msg, DResponse errorOrig=null)
        {
            if (errorOrig == null)
            {
                DResponse response = new DResponse();
                response.isError = true;
                response.errorMessages.Add(String.Format("At component id={0}, name={1} error: {2}", component.name, component.id, msg));
                return response;
            }
            else
            {
                errorOrig.errorMessages.Add(String.Format("At component id={0}, name={1} error: {2}", component.name, component.id, msg));
                return errorOrig;
            }
        }
        public static DResponse CreateErrorResponseNoOutputs(DComponent component)
        {
            return CreateErrorResponse(component, "No outputs from this component");
        }
        public static DResponse CreateErrorResponseAllErrors(DComponent component)
        {
            return CreateErrorResponse(component, "No outputs produced valid response");
        }
        public static DResponse CreateResponse(string data)
        {
            DResponse response = new DResponse();
            response.data = data;
            return response;
        }
    }
}
