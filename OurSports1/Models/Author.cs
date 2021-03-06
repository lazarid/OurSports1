﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OurSports1.Models
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [RegularExpression(@"^[a-zA-Z' ']+$", ErrorMessage = "Use letters only please")]

        public string AuthorName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

        private string image;
        public string Image
        {
            get
            {
                return this.image;
            }
            set
            {
                if (value == null)
                {
                    this.image = "Author.jpg";
                }
                else
                {
                    image = value;
                }
            }
        }
    }
}
