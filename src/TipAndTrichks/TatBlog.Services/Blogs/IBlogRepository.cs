using Microsoft.EntityFrameworkCore;
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

    Task<IList<AuthorItem>> GetAuthorAsync(
        CancellationToken cancellationToken = default);


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

    //g) thêm hoặc cập nhật một chuyên mục chủ đề
    Task AddOrUpdateCategory(Category category, CancellationToken cancellationToken = default);



    // h) xoa mot chuyen muc theo ma so cho truoc
    Task<bool> DeleteCategoryByID(int id, CancellationToken cancellationToken = default);

    // i) kiểm tra tên định danh slug của một chuyên mục đã tồn tại chưa
    Task<bool> CheckIDSlugOfCategoryExist(string slug, CancellationToken cancellationToken = default);


    //j) lấy phân trang danh sách chuyên mục kết quả trả về kiểu IPageList
    Task<IPagedList<CategoryItem>> GetPageShareCategory(IPagingParams pagingParams, CancellationToken cancellationToken = default);


    // l) tìm một bài viêt theo mã số
    Task<Post> FindPostById(int id, CancellationToken cancellationToken = default);

    // m) thêm hoặc cập nhật một bài viết
    Task AddOrUpdatePost(Post post, CancellationToken cancellationToken = default);


    // n) chuyen đổi trạng thái public của bài viế
    Task ConvertStatusPostToPublished(int id, bool published, CancellationToken cancellationToken = default);


    // o) lấy ngẫu nhiên N bài viết N là tham số đầu vào.
    Task<IList<Post>> GetRandomNPost(int n, CancellationToken cancellationToken = default);

    //// q) tìm tất cả các bài viết thoả mã điều kiện tìm kiếm được cho đối tượng PostQuery
    //Task<IList<Post>> FindAllPostsConditionFindObjectOfPostQuery(PostQuery postQuery, CancellationToken cancellation = default);



    ////1e) tim va phan trang
    //Task<IPagedList<Post>> GetPagePostsAsync(
    //    PostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);

    //Task<IPagedList<T>> GetPagePostsAsync<T>(
    //    PostQuery query,
    //    IPagingParams pagingParams,
    //    Func<IQueryable<Post>, IQueryable<T>> mapper,
    //    CancellationToken cancellationToken
    //    );


    Task<IPagedList<Post>> GetPagePostAsync(
        PostQuery condition, int pageNumber, int pageSize, CancellationToken cancellationToken = default);


    // chuc nang moi
    Task<Author> GetAuthorBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default);


    Task<Post> GetPostSlugAsync(int year, int month,string slug, CancellationToken cancellationToken= default);

    Task<IList<TagItem>> GetTagsAllAsync(CancellationToken cancellationToken = default);

    Task<IList<PostMonth>> PostCountInMonth(int month, CancellationToken cancellationToken = default);

}
