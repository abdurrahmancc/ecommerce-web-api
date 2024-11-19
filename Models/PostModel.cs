using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_web_api.Models;



namespace Ecommerce_web_api.Models
{
    public class Post
{
    public int Id { get; set; }
    public int BlogId { get; set; }  // Foreign key to the Blog
    public Blog Blog { get; set; } = null!;  // Navigation property back to the parent Blog
}
}