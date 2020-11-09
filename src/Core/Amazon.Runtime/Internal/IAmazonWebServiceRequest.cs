using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amazon.Runtime.Internal
{
    public interface IAmazonWebServiceRequest
    {
        EventHandler<StreamTransferProgressArgs> StreamUploadProgressCallback { get; set; }

        void AddBeforeRequestHandler(RequestEventHandler handler);

        void RemoveBeforeRequestHandler(RequestEventHandler handler);

        Dictionary<string, object> RequestState { get; }

        bool UseSigV4 { get; set; }
    }
}
