using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApplication.Infra
{
    public interface IEtagHandlerFeature
    {
        bool NoneMatch(IEtaggable entity);
        bool NoneMatch(IEnumerable<IEtaggable> entity);
    }
}
