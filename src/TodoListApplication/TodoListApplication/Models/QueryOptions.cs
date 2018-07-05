using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApplication.Models
{
    /// <summary>
    /// Query options
    /// </summary>
    public class QueryOptions
    {
        /// <summary>
        /// Keywords for name field
        /// </summary>
        public string SearchString { get; set; }

        /// <summary>
        /// Skip number (Range 1-1000)
        /// </summary>
        [Range(1, 1000, ErrorMessage = "Skip number must be from 1 - 1000")]
        public int? Skip { get; set; }

        /// <summary>
        /// Limit number 1-500
        /// </summary>
        [Range(1, 500, ErrorMessage = "Limit number must be from 1 - 500")]
        public int? Limit { get; set; }
    }
}
