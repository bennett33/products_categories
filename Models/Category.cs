using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using products_categories.Models;

namespace products_categories.Models
{
    public class Category
    {
        [Key] // the below prop is the primary key, [Key] is not needed if named with pattern: ModelNameId
        public int CategoryId { get; set; }


        [Required(ErrorMessage = "is required")]
        [MinLength(2, ErrorMessage = "must be at least 2 characters")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<ProdCatMany> CategoryProducts { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
