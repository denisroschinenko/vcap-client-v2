using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronFoundry.VcapClient.V2;
using IronFoundry.VcapClient.V2.Models;

namespace IronFoundryConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            VcapClient client = new VcapClient(new Uri("http://api.192.168.1.77.xip.io"), new StableDataStorage());
            client.Login("micro@vcap.me", "micr0@micr0");
            //client.GetApplication("caldecott");
            var application = new Application()
                {
                    Name = "Testtesttest",
                    SpaceGuid = Guid.Parse("96400672-c897-4545-aadd-79181833ca6a"),
                    StackGuid = Guid.Parse("2becc7fd-db45-461e-9d94-8dc1243e1bf7"),
                    NumberInstance = 1,
                    Memory = 128,
                    DiskQuota = 1024
                };

            //client.PushApplication(application, @"d:\MvcAltorosApplication\MvcAltorosApplication ");
            Console.ReadLine();
        }
    }
}
