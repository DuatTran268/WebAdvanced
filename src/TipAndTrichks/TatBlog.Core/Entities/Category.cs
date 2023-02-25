using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;

namespace TatBlog.Core.Entities;
public class Category : IEntity
{
    // ma chuyen muc
    public int Id { get; set; }
    // ten chuyen muc chu de
    public string Name { get; set; }
    // ten dinh danh dung de tao url    
    public string UrlSlug { get; set; }
    // mo ta them ve chuyen muc
    public string Description { get; set; }
    // danh dau chuyen muc hien thi tren menu
    public bool ShowOnMenu { get; set; }

    // danh sach cac bai viet hien thi tren chuyen muc
    public IList<Post> Posts { get; set; }

}
