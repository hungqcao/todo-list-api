using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApplication.Infra
{
    public interface IEtaggable
    {
        string GetEtag();
    }
}
