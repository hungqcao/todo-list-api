using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApplication.Infra;

namespace TodoListApplication.Models
{
    public class TodoList : IEtaggable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<TodoTask> Tasks { get; set; }

        public string GetEtag()
        {
            var serialized = JsonConvert.SerializeObject(this);
            return serialized.GetHashCode().ToString(); // Poor man implementation, for sake of demoing
        }
    }
}
