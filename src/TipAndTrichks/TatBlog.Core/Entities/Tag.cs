using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
namespace TatBlog.Core.Entities;
public class Tag : IEntity
{
    // ma tu khoa
    public int Id { get; set; }
    // noi dung tu khoa
    public string Name { get; set; }
    // ten dinh danh de tao url
    public string UrlSlug { get; set; }
    // mo ta them ve tu khoa
    public string Description { get; set; }
    // danh sach bai viet co chua tu khoa
    public IList<Post> Posts { get; set; }




}
