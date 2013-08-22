using System;
using System.Collections.Generic;
using IronFoundry.VcapClient.V2.Models;
using Newtonsoft.Json;


namespace IronFoundry.VcapClient.V2
{
    internal class TokenManager
    {
        private IDictionary<Uri, AccessToken> _tokenDict = new Dictionary<Uri, AccessToken>();
        private IStableDataStorage _dataStorage;

        public TokenManager(IStableDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
        }

        public AccessToken GetToken(Uri target)
        {
            AccessToken token;
            if (_tokenDict.ContainsKey(target))
            {
                token = _tokenDict[target];
            }
            else
            {
                _tokenDict = JsonConvert.DeserializeObject<Dictionary<Uri, AccessToken>>(_dataStorage.ReadToken());
                token = _tokenDict != null ? _tokenDict[target] : null;

            }
            return token;
        }

        public void RegisterToken(AccessToken token, Uri currentTarget)
        {
            if (_tokenDict.ContainsKey(currentTarget))
            {
                _tokenDict[currentTarget] = token;
            }
            else
            {
                _tokenDict.Add(currentTarget, token);
            }
            _dataStorage.WriteToken(JsonConvert.SerializeObject(_tokenDict));
        }

    }
}
