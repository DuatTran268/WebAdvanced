using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TatBlog.Core.Contracts;
namespace TatBlog.Core.Entities;

public class Post :IEntity
{
    // ma bai viet
    public int Id { get; set; }
    // tieu de bai viet
    public string Title { get; set; }
    // mo ta hay gioi thieu ngan ve noi dung
    public string ShortDescription { get; set; }

    // noi dung chi tiet cua bai viet
    public string Description { get; set; }
    // metadata
    public string Meta { get; set; }
    // ten dinh danh de tao url
    public string UrlSlug { get; set; }
    // ten dinh danh de tao tap tin hinh anh
    public string ImageUrl { get; set; }
    // so luot xem, doc bai viet
    public int ViewCount { get; set; }
    // trang thai cua bai viet
    public bool Published { get; set; }
    // ngay gio dang bai
    public DateTime PostedDate { get; set; }
    // ngay gio cap nhat lan cuoi
    public DateTime? ModifiedDate { get; set; }
    // ma chuyen muc
    public int CategoryId { get; set; }
    // ma tac gia cua bai viet
    public int AuthorId { get; set; }
    // chuyen muc bai viet
    public Category Category { get; set; }
    // tac gia bai viet
    public Author Author { get; set; }
    // danh sach cac tu khoa cua bai viet
    public IList<Tag> Tags { get; set; }

	// list comment
	//public IList<Comment> Comments { get; set; }

}
