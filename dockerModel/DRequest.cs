using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dockerModel
{
    class DRequest
    {
        public string data { get; set; }
        public static DRequest CreateRequest(string data)
        {
            DRequest request = new DRequest();
            request.data = data;
            return request;
        }
    }
}
