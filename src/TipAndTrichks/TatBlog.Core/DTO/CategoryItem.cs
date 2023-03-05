using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO;

public class CategoryItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string UrlSlug { get; set; }

    public string Description { get; set; }
    public bool ShowOnMenu { get; set; }
    public int PostCount { get; set; }

    public override string ToString()
    {
        return string.Format("{0,-5}{1,-30}{2,-10}{3,30}{4,50,},{5, 60}", Id, Name, UrlSlug, Description, ShowOnMenu, PostCount);
    }

}
