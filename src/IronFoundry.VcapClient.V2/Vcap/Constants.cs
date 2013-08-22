using System;


namespace IronFoundry.VcapClient.V2
{
    public class Constants
    {
        public static readonly Uri DefaultLocalTarget = new Uri("http://api.vcap.me");
        public static readonly Uri LoginTarget = new Uri("http://login.vcap.me");
        public static readonly string DefaultLogin = "cf:";

        public static readonly string OuthToken = "oauth/token";
        public static readonly string Application = "v2/apps";
        public static readonly string Stack = "v2/stacks";
        public static readonly string Space = "v2/spaces";
        public static readonly string User = "v2/users";
        public static readonly string Organization = "v2/organizations";
        public static readonly string Service = "v2/services";
        public static readonly string ServiceInstance = "v2/service_instances";
        public static readonly string ServiceBinding = "v2/service_bindings";
        public static readonly string ServicePlan = "v2/service_plans";
        public static readonly string Route = "v2/routes";
        public static readonly string Domain = "v2/domains";
        public static readonly string Event = "v2/events";
        public static readonly string Info = "v2/info";

        public static readonly string Bits = "bits";
        public static readonly string ResourceMatch = "v2/resource_match";


        public static readonly string ParamName = "name";
    }
}
