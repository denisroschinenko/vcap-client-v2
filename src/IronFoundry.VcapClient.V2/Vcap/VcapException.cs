using System;

namespace IronFoundry.VcapClient.V2
{
    public class VcapException : Exception
    {
        public VcapException() { }

        public VcapException(string message)
            : base(message)
        {

        }

    }

    public class VcapNotFoundException : VcapException
    {
        public VcapNotFoundException() { }

        public VcapNotFoundException(string message)
            : base(message) { }
    }
}
