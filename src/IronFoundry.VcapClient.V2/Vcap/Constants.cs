using System;


namespace IronFoundry.VcapClient.V2
{
    public class Constants
    {
        public static readonly Uri DefaultLocalTarget = new Uri("http://api.vcap.me");
        public static readonly Uri LoginTarget = new Uri("http://login.vcap.me");
        public static readonly string DefaultLogin = "cf:";

        public static readonly string OuthToken = "oauth/token";
        public static readonly string Application = "apps";
        public static readonly string Stack = "stacks";
        public static readonly string Space = "spaces";
        public static readonly string User = "users";
        public static readonly string Organization = "organizations";
        public static readonly string Service = "services";
        public static readonly string ServiceInstance = "service_instances";
        public static readonly string ServiceBinding = "service_bindings";
        public static readonly string ServicePlan = "service_plans";
        public static readonly string Route = "routes";
        public static readonly string Domain = "domains";
        public static readonly string Event = "events";
        public static readonly string Info = "info";

        public static readonly string Bits = "bits";
        public static readonly string ResourceMatch = "resource_match";
        public static readonly string Download = "download";


        public static readonly string ParamName = "name";
    }
}
