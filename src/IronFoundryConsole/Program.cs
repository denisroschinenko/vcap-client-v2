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
            Console.WriteLine("Enter CFv2 data:");
            Console.WriteLine("Url:");
            var url = Console.ReadLine();

            Console.WriteLine("Login:");
            var login = Console.ReadLine();

            Console.WriteLine("Password");
            var password = Console.ReadLine();

            var client = new VcapClient(new Uri(url), new StableDataStorage());
            client.Login(login ?? "micro@vcap.me", password ?? "micr0@micr0");

            Console.WriteLine("--- Organizations: ---");
            foreach (var organization in client.GetOrganizations())
            {
                Console.WriteLine(organization.Entity.Name);
            }
            Console.WriteLine();

            Console.WriteLine("--- Spaces: ---");
            foreach (var space in client.GetSpaces())
            {
                Console.WriteLine(space.Entity.Name);
            }
            Console.WriteLine();

            Console.WriteLine("--- Apps: ---");
            foreach (var app in client.GetApplications())
            {
                Console.WriteLine(app.Entity.Name);
            }
            Console.WriteLine();

            Console.WriteLine("Everything is OK.");
            Console.ReadLine();
        }
    }
}
