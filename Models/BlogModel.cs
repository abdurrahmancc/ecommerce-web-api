using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_web_api.DTOs.Blog;


namespace Ecommerce_web_api.Models
{
    public class Blog
{
    public int Id { get; set; }
    public ICollection<Post> Posts { get; } = new List<Post>();
}

}