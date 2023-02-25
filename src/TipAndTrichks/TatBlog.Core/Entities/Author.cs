using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TatBlog.Core.Contracts;
namespace TatBlog.Core.Entities;

public class Author : IEntity
{
    // ma tac gia bai viet
    public int Id { get; set; }
    // ten tac gia
    public string FullNames { get; set; }
    // ten dinh danh dung de tao URL
    public string UrlSlug { get; set; }
    // duong dan toi hinh anh
    public string ImageUrl { get; set; }
    // ngay bat dau
    public DateTime JoinedDate { get; set; }
    // dia chi email
    public string Email { get; set; }
    // ghi chu
    public string Notes { get; set; }
    // danh sach cac bai viet cua tac gia
    public IList<Post> Posts { get; set; }
}
