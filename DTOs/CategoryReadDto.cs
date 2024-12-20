using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_web_api.DTOs
{
    public class CategoryReadDto
    {
        public Guid CategoryId {get; set;}
        public string Name {get; set;}
        public string? Description {get; set;} = string.Empty;
        public DateTime CreateAt {get; set;}
    }
}