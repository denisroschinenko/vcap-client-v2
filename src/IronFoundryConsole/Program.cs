using System;
using System.IO;
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

            var client = new VcapClient(new Uri(string.IsNullOrWhiteSpace(url) ? "http://api.192.168.1.77.xip.io" : url), new StableDataStorage());
            client.Login(string.IsNullOrWhiteSpace(login) ? "admin" : login, string.IsNullOrWhiteSpace(password) ? "c1oudc0w" : password);


            var application = new ApplicationManifest();
            application.Name = "test";
            application.StackGuid = Guid.Parse("c9cf45fe-ee2e-4157-acdf-a5915b9bb15d");
            application.Memory = 128;
            application.NumberInstance = 1;
            application.SpaceGuid = Guid.Parse("93fad9b7-db0b-4c4d-8ffe-d456d91af608");
            var path = @"d:\MvcAltorosApplication\MvcAltorosApplication";
            client.PushApplication(application, path, "test", "vcap.me");

            //Console.WriteLine("Start downloading");
            //Console.ReadLine();

            //var app = client.GetApplication("test");
            //var stream = client.DownloadApplication(app.Metadata.ObjectId);
            //DownloadFile(stream);

            //Console.WriteLine("--- Organizations: ---");
            //foreach (var organization in client.GetOrganizations())
            //{
            //    Console.WriteLine(organization.Entity.Name);
            //}
            //Console.WriteLine();

            //Console.WriteLine("--- Spaces: ---");
            //foreach (var space in client.GetSpaces())
            //{
            //    Console.WriteLine(space.Entity.Name);
            //}
            //Console.WriteLine();

            //Console.WriteLine("--- Apps: ---");
            //foreach (var app in client.GetApplications())
            //{
            //    Console.WriteLine(app.Entity.Name);
            //}
            //Console.WriteLine();

            //Console.WriteLine("Everything is OK.");
            //Console.ReadLine();
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
