﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurSports1.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}
