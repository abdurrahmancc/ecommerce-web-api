using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_web_api.Models
{
public class Category  {
    public Guid CategoryId {get; set;}
    public string Name {get; set;}
    public string? Description {get; set;} = string.Empty;
    public DateTime CreateAt {get; set;}
};
}