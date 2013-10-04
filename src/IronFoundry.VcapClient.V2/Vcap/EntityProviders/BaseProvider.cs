
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using IronFoundry.VcapClient.V2.Models;
using PortableRest;

namespace IronFoundry.VcapClient.V2
{
    internal abstract class BaseProvider<T, U>
    {
        protected readonly VcapCredentialManager CredentialManager;
        protected readonly VcapRequest VcapRequest;
        private readonly string _queryParameter = "q=";

        protected abstract string EntityName { get; }


        protected BaseProvider(VcapCredentialManager credentialManager, bool isLogin, bool isAuthentication)
        {
            CredentialManager = credentialManager;
            VcapRequest = new VcapRequest(CredentialManager, isLogin, isAuthentication);
        }

        protected BaseProvider(VcapCredentialManager credentialManager)
            : this(credentialManager, false, true) { }

        public virtual IEnumerable<Resource<T>> GetAll()
        {
            VcapRequest.BuildRequest(HttpMethod.Get, ContentTypes.Json, GetEntityNameV2());
            return VcapRequest.Execute<ResponseData<T>>().Resources;
        }

        public virtual Resource<T> GetById(Guid entityId)
        {
            VcapRequest.BuildRequest(HttpMethod.Get, ContentTypes.Json, GetEntityNameV2(), entityId);
            return VcapRequest.Execute<Resource<T>>();
        }

        public virtual Resource<T> GetByParam(string paramName, object paramValue)
        {
            var args = BuildFilteringArgs(new KeyValuePair<string, object>(paramName, paramValue));
            VcapRequest.BuildRequest(HttpMethod.Get, ContentTypes.Json, args);
            var responceData = VcapRequest.Execute<ResponseData<T>>();
            return responceData.Resources.SingleOrDefault();
        }

        public virtual Resource<T> Create(U entity)
        {
            VcapRequest.BuildRequest(HttpMethod.Post, ContentTypes.Json, GetEntityNameV2());
            VcapRequest.AddBodyParameter(EntityName, entity);
            return VcapRequest.Execute<Resource<T>>();
        }

        public virtual Resource<T> Update(Resource<T> resource)
        {
            VcapRequest.BuildRequest(HttpMethod.Put, ContentTypes.Json, GetEntityNameV2(), resource.Metadata.ObjectId);
            VcapRequest.AddBodyParameter(EntityName, resource.Entity);
            return VcapRequest.Execute<Resource<T>>();
        }

        public virtual void Delete(Guid entityId)
        {
            VcapRequest.BuildRequest(HttpMethod.Delete, ContentTypes.Json, GetEntityNameV2(), entityId);
            VcapRequest.Execute<Resource<T>>();
        }

        protected string GetEntityNameV2(string entityName = null)
        {
            return string.Format("v2/{0}", !string.IsNullOrWhiteSpace(entityName) ? entityName : EntityName);
        }

        protected bool EntityExists(string name)
        {
            var resource = GetByParam(Constants.ParamName, name);
            return resource != null;
        }

        #region Auxillary methods
        private string BuildFilteringArgs(KeyValuePair<string, object> param)
        {
            return string.Format("{0}?{1}{2}:{3}", GetEntityNameV2(), _queryParameter, param.Key, param.Value);
        }
        #endregion
    }
}
