using System;
using System.Collections.Generic;
using System.Net.Http;
using IronFoundry.VcapClient.V2.Models;
using PortableRest;

namespace IronFoundry.VcapClient.V2
{
    internal class InfoProvider : BaseProvider<Info, Info>
    {
        public InfoProvider(VcapCredentialManager credentialManager)
            : base(credentialManager)
        {
        }

        protected override string EntityName
        {
            get { return Constants.Info; }
        }

        public override IEnumerable<Resource<Info>> GetAll()
        {
            throw new NotSupportedException();
        }

        public override Resource<Info> Create(Info entity)
        {
            throw new NotSupportedException();
        }

        public override Resource<Info> Update(Resource<Info> resource)
        {
            throw new NotSupportedException();
        }

        public override void Delete(Guid entityId)
        {
            throw new NotSupportedException();
        }

        public override Resource<Info> GetById(Guid entityId)
        {
            throw new NotSupportedException();
        }

        public Info GetInfo()
        {
            VcapRequest.BuildRequest(HttpMethod.Get, ContentTypes.Json, GetEntityNameV2());
            return VcapRequest.Execute<Info>();
        }
    }
}
