using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Bulky.Models
{
    public class Product
    {
        public int Id { get; set; }
        [MinLength(3)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        [MinLength(3, ErrorMessage = "Minimum length is 3")]
        public string Author { get; set; }
        [DisplayName("List Price")]
        [Range(1, 1000, ErrorMessage = "Range of price must be between 1-1000")]
        public double ListPrice { get; set; }
        [DisplayName("Price for 1-50")]
        [Range(1, 1000, ErrorMessage = "Range of price must be between 1-1000")]
        public double Price { get; set; }
        [DisplayName("Price for 50+")]
        [Range(1, 1000, ErrorMessage = "Range of price must be between 1-1000")]
        public double Price50 { get; set; }
        [DisplayName("Price for 100+")]
        [Range(1, 1000, ErrorMessage = "Range of price must be between 1-1000")]
        public double Price100 { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
    }
}
