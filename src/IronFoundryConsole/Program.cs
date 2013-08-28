using System;
using System.Configuration;
using IronFoundry.VcapClient.V2;

namespace IronFoundryConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = ConfigurationManager.AppSettings["url"];
            var login = ConfigurationManager.AppSettings["login"];
            var password = ConfigurationManager.AppSettings["password"];

            ConnectToCloudFoundry(url, login, password);
        }

        private static void ConnectToCloudFoundry(string url, string login, string password)
        {
            if (String.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            if (String.IsNullOrEmpty(login))
            {
                throw new ArgumentNullException("login");
            }

            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }

            var client = new VcapClient(new Uri(url), new StableDataStorage());
            client.Login(login, password);

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
