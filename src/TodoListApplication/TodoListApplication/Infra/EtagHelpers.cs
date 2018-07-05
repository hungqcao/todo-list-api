using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApplication.Infra
{
    public static class EtagHelpers
    {
        public static string GetEtagForCollection(IEnumerable<IEtaggable> etaggables)
        {
            // For demo only
            if (etaggables == null) return string.Empty;

            var str = JsonConvert.SerializeObject(etaggables);
            return str.GetHashCode().ToString();
        }
    }
}
