using System;
using System.Collections.Generic;
using System.IO;
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

            //var list = new List<Guid>() { Guid.Parse("dc2648b4-a6bd-4092-a491-afc5b4cb1781") };
            //client.CreateUser("test8", new List<string> { "test8" }, "123456", "test8", "test8", list, list, list);


            //var name = "test";
            //var stackGuid = Guid.Parse("c9cf45fe-ee2e-4157-acdf-a5915b9bb15d");
            //var memory = 128;
            //var numberInstance = 1;
            //var spaceGuid = Guid.Parse("93fad9b7-db0b-4c4d-8ffe-d456d91af608");
            //var path = @"d:\MvcAltorosApplication\MvcAltorosApplication";
            //client.PushApplication(name, stackGuid, spaceGuid, memory, numberInstance, path, "test", "vcap.me");

            //Console.WriteLine("Start downloading");
            //Console.ReadLine();

            //var app = client.GetApplication("test");
            //var stream = client.DownloadApplication(app.Metadata.ObjectId);
            //DownloadFile(stream);

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


        public static void DownloadFile(Stream stream)
        {
            using (Stream file = File.Create("123"))
            {
                CopyStream(stream, file);
            }
        }

        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }
    }
}
