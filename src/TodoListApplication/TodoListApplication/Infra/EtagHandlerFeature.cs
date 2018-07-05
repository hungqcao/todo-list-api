using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApplication.Infra
{
    public class EtagHandlerFeature : IEtagHandlerFeature
    {
        private IHeaderDictionary _headers;

        public EtagHandlerFeature(IHeaderDictionary headers)
        {
            _headers = headers;
        }

        public bool NoneMatch(IEtaggable entity)
        {
            if (!_headers.TryGetValue("If-None-Match", out var etags)) return true;

            var etag = entity.GetEtag();

            if (string.IsNullOrEmpty(etag)) return true;

            if (!etag.Contains('"'))
            {
                etag = $"\"{etag}\"";
            }

            return !etags.Contains(etag);
        }

        public bool NoneMatch(IEnumerable<IEtaggable> entity)
        {
            if (!_headers.TryGetValue("If-None-Match", out var etags)) return true;

            var etag = EtagHelpers.GetEtagForCollection(entity); // For sake of demoing

            if (string.IsNullOrEmpty(etag)) return true;

            if (!etag.Contains('"'))
            {
                etag = $"\"{etag}\"";
            }

            return !etags.Contains(etag);
        }
    }
}
