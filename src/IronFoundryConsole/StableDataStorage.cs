using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using IronFoundry.VcapClient.V2;
using IronFoundry.VcapClient.V2.Models;
using Microsoft.VisualBasic.Devices;

namespace IronFoundryConsole
{
    /// <summary>
    /// This class is an example how to work with storage
    /// </summary>
    internal class StableDataStorage : IStableDataStorage
    {
        private const string TokenFile = ".v2_token";
        private readonly string _tokenFile;

        public static Func<string, string> FileReaderFunc = fileName => File.ReadAllText(fileName);
        public static Action<string, string> FileWriterAction = (fileName, text) => File.WriteAllText(fileName, text);

        public StableDataStorage()
        {
            string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            _tokenFile = Path.Combine(userProfilePath, TokenFile);
        }

        public void WriteToken(string tokenInfo)
        {
            try
            {
                FileWriterAction(_tokenFile, tokenInfo);
            }
            catch (IOException)
            {
            }
        }

        public string ReadToken()
        {
            string rv = null;

            try
            {
                rv = FileReaderFunc(_tokenFile);
            }
            catch (FileNotFoundException) { }

            return rv;
        }

        public string CopyProjectToTempDirectory(string projectDirectoryPath)
        {
            DirectoryInfo tempDirectory = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

            var directoryInfo = new DirectoryInfo(projectDirectoryPath);
            CopyDirectory(directoryInfo, tempDirectory);

            return tempDirectory.FullName;
        }

        public ResourceFile[] FilteringResources(string tempDirectoryPath, Func<ResourceFile[], ResourceFile[]> checkResourcesFunction)
        {
            var explodeDir = new DirectoryInfo(tempDirectoryPath);
            ResourceFile[] appcloudResources = null;
            try
            {
                var resources = new List<ResourceFile>();
                ulong totalSize = AddDirectoryToResources(resources, explodeDir, explodeDir.FullName);

                if (resources.Any())
                {
                    if (totalSize > (64 * 1024))
                    {
                        appcloudResources = checkResourcesFunction(resources.ToArray());
                    }
                    if (appcloudResources == null || !appcloudResources.Any())
                    {
                        appcloudResources = resources.ToArray();
                    }
                    else
                    {
                        foreach (ResourceFile r in appcloudResources)
                        {
                            string localPath = Path.Combine(explodeDir.FullName, r.FN);
                            var localFileInfo = new FileInfo(localPath);
                            localFileInfo.Delete();
                            resources.Remove(r);
                        }
                    }
                    if (!resources.Any())
                    {
                        File.WriteAllText(Path.Combine(explodeDir.FullName, ".__empty__"), String.Empty);
                    }
                }
            }
            catch
            {
            }
            return appcloudResources;
        }

        public byte[] CreateZipFile(string tempDirectoryPath)
        {
            var explodeDir = new DirectoryInfo(tempDirectoryPath);
            string uploadFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            byte[] fileBytes = null;
            try
            {
                ZipFile.CreateFromDirectory(explodeDir.FullName, uploadFile);
                fileBytes = FileToByteArray(uploadFile);
            }
            catch { }

            finally
            {
                Directory.Delete(explodeDir.FullName, true);
                File.Delete(uploadFile);

            }

            return fileBytes;
        }

        #region Auxillary methods

        private ulong AddDirectoryToResources(ICollection<ResourceFile> resources, DirectoryInfo directory, string rootFullName)
        {
            ulong totalSize = 0;

            var fileTrimStartChars = new[] { '\\', '/' };

            foreach (FileInfo file in directory.GetFiles())
            {
                totalSize += (ulong)file.Length;

                string hash = Hexdigest(file);
                string filename = file.FullName;

                filename = filename.Replace(rootFullName, String.Empty);
                filename = filename.TrimStart(fileTrimStartChars);
                filename = filename.Replace('\\', '/');
                resources.Add(new ResourceFile((ulong)file.Length, hash, filename));
            }

            foreach (DirectoryInfo subdirectory in directory.GetDirectories())
            {
                totalSize += AddDirectoryToResources(resources, subdirectory, rootFullName);
            }

            return totalSize;
        }

        private void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
        {
            var c = new Computer();
            c.FileSystem.CopyDirectory(source.FullName, target.FullName, true);
        }

        public string Hexdigest(FileInfo file)
        {
            using (FileStream fs = File.OpenRead(file.FullName))
            {
                using (var sha1 = SHA1.Create())
                {
                    return BitConverter.ToString(sha1.ComputeHash(fs)).Replace("-", String.Empty).ToLowerInvariant();
                }
            }
        }

        private byte[] FileToByteArray(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }

        #endregion

    }
}
