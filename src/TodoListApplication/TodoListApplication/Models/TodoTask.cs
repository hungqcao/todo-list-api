﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApplication.Models
{
    public class TodoTask
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Completed { get; set; }
    }
}
