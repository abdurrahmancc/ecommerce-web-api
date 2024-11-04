using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_web_api.DTOs
{
    public class CategoryUpdateDto
    {
        [Required( ErrorMessage = "Category name is required")]
        [StringLength(100, MinimumLength =2, ErrorMessage ="Category name is must be at least 2 character long")]
        public string Name {get; set;} = string.Empty;
        [StringLength(100, MinimumLength =10, ErrorMessage ="Category description is must be at least 10 character long")]
        public string? Description {get; set;} = string.Empty;

    }
}