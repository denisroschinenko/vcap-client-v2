using System;

namespace IronFoundry.VcapClient.V2
{
    public class VcapException : Exception
    {
        public VcapException(string message)
            : base(message)
        {

        }
    }
}
