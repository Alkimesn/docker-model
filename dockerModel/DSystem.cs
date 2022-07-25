using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dockerModel
{
    class DSystem
    {
        OuterComponent oc1 = new OuterComponent();
        OuterComponent oc2 = new OuterComponent();
        public event EventHandler<string> ResponseReceived;
        public DSystem()
        {

            TransferComponent tc0 = new TransferComponent();

            TransferComponent tc11 = new TransferComponent();
            TransferComponent tc12 = new TransferComponent();

            TransferComponent tc21 = new TransferComponent();

            InnerComponent ic = new InnerComponent();

            oc1.outputs.Add(tc0);
            oc2.outputs.Add(tc0);

            tc0.outputs.Add(tc11);
            tc0.outputs.Add(tc12);

            tc11.outputs.Add(tc21);
            tc12.outputs.Add(tc21);

            tc21.outputs.Add(ic);

            ic.evaluator = x => DResponse.CreateResponse((2.5 * double.Parse(x.data)).ToString());
        }
        public async void DoStep(string input)
        {
            DRequest request = DRequest.CreateRequest(input);
            DResponse response = await oc1.GetResponse(request);
            if (response.isError) ResponseReceived(this, response.errorMessages.Aggregate((x, y) => x + '\n' + y));
            else ResponseReceived(this, response.data);
        }
    }
}
