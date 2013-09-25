using System;
using System.IO;
using IronFoundry.VcapClient.V2.Models;

namespace IronFoundry.VcapClient.V2
{
    public interface IStableDataStorage
    {
        void WriteToken(string tokenInfo);
        string ReadToken();

        string CopyProjectToTempDirectory(string projectDirectoryPath);
        ResourceFile[] FilteringResources(string tempDirectoryPath, Func<ResourceFile[], ResourceFile[]> checkResourcesFunction);
        byte[] CreateZipFile(string tempDirectoryPath);
    }
}
