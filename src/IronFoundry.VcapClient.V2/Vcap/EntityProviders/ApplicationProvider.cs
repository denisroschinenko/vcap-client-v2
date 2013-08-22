using System;
using System.IO;
using System.Net.Http;
using IronFoundry.VcapClient.V2.Models;
using PortableRest;


namespace IronFoundry.VcapClient.V2
{
    internal class ApplicationProvider : BaseProvider<Application>
    {
        private readonly IStableDataStorage StableDataStorage;

        public ApplicationProvider(VcapCredentialManager credentialManager) :
            base(credentialManager) { }

        public ApplicationProvider(VcapCredentialManager credentialManager, IStableDataStorage stableDataStorage)
            : this(credentialManager)
        {
            StableDataStorage = stableDataStorage;
        }

        protected override string Constant
        {
            get { return Constants.Application; }
        }

        public void StartApplication(Guid applicationId)
        {
            var application = GetById(applicationId);
            application.Entity.State = Application.ApplicationStates.Started;
            Update(application.Entity);
        }
        public void StopApplication(Guid applicationId)
        {
            var application = GetById(applicationId);
            application.Entity.State = Application.ApplicationStates.Stopped;
            Update(application.Entity);

        }
        public void RestartApplication(Guid applicationId)
        {
            StopApplication(applicationId);
            StartApplication(applicationId);
        }

        public void PushApplication(Application application, string projectPath)
        {
            var entity = Create(application);

            var tempDirectoryPath = StableDataStorage.CopyProjectToTempDirectory(projectPath);
            var resources = StableDataStorage.FilteringResources(tempDirectoryPath, CheckResources);
            byte[] fileBytes = StableDataStorage.CreateZipFile(tempDirectoryPath);

            UploadApplicationBits(entity, fileBytes, resources);

            //StartApplication(entity.Metadata.ObjectId);
        }

        private ResourceFile[] CheckResources(ResourceFile[] resourcesArray)
        {
            VcapRequest.BuildRequest(HttpMethod.Put, ContentTypes.Json, Constants.ResourceMatch);
            VcapRequest.AddBodyParameter("resources", resourcesArray);
            return VcapRequest.Execute<ResourceFile[]>();
        }

        private void UploadApplicationBits(Resource<Application> application, byte[] fileBytes, ResourceFile[] resourcesArray)
        {
            VcapRequest.BuildRequest(HttpMethod.Put, ContentTypes.MultipartFormData, Constant, application.Metadata.ObjectId, Constants.Bits);
            VcapRequest.AddFile("application", fileBytes, application.Metadata.ObjectId.ToString());
            VcapRequest.AddBodyParameter("resources", resourcesArray);
            VcapRequest.Execute<object>();
        }
    }
}
