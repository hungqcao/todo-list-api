using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApplication.Models
{
    public class QueryOptions
    {
        public string SearchString { get; set; }

        [Range(1, 1000, ErrorMessage = "Skip number must be from 1 - 1000")]
        public int? Skip { get; set; }

        [Range(1, 500, ErrorMessage = "Limit number must be from 1 - 500")]
        public int? Limit { get; set; }
    }
}
