using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface IBlogRepository
{
    // tim bai viet co dinh danh la slug va duoc dang  vao thang nam 
    Task<Post> GetPostAsync(
        int year,
        int month,
        string slug,
        CancellationToken cancellationToken = default);

    // tim top N bai viet pho bien nhieu nguoi xem nhat
    Task<IList<Post>> GetPopularArticlesAsync(
        int numPosts,
        CancellationToken cancellationToken = default);

    // kiem tra xem ten dinh danh cua bai viet da co hay chua
    Task<bool> IsPostSlugExistedAsync(
        int postId, string slug,
        CancellationToken cancellationToken = default
        );

    // tang so luot xem cua mot bai viet
    Task IncreaseViewCountAsync(
        int postId,
        CancellationToken cancellationToken = default );


    // lay danh sach chuyen muc va so luong bai viet nam thuoc tung chuyen muc

    Task<IList<CategoryItem>> GetCategoryAsync(
        bool showOnMenu = false, CancellationToken cancellationToken = default);




    //// lay danh sach tu khoa / the phan trang theo cac tham so pagingParams
    Task<IPagedList<TagItem>> GetPagedTageAsync(

        IPagingParams pagingParams, CancellationToken cancellation = default);


    // a) tim mot the tag theo dinh danh slug
    Task<Tag> GetTagSlugAsync(
        string slug, CancellationToken cancellationToken = default);

    // c) lấy danh sách tất cả các thẻ tag
    Task<IList<TagItem>> GetAllTagsAttachPost (CancellationToken cancellationToken = default);

    // d) xoá một thẻ cho trước
    Task<bool> RemoveTagById (int id, CancellationToken cancellationToken = default);

    // e) tìm một chuyên muc category theo dinh danh slug
    Task<Category> GetCategoryBySlugAsync(
        string slug, CancellationToken cancellationToken = default);

    // f) tìm một chuyên mục theo mã số cho trước
    Task<Category> FindCategoryById (int id, CancellationToken cancellationToken = default);


    // h) xoa mot chuyen muc theo ma so cho truoc
    Task<bool> DeleteCategoryByID(int id, CancellationToken cancellationToken = default);

}
