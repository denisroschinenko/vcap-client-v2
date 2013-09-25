using IronFoundry.VcapClient.V2.Models;

namespace IronFoundry.VcapClient.V2
{
    internal class StackProvider : BaseProvider<Stack, Stack>
    {
        public StackProvider(VcapCredentialManager credentialManager)
            : base(credentialManager)
        {
        }

        protected override string Constant
        {
            get { return Constants.Stack; }
        }
    }
}
