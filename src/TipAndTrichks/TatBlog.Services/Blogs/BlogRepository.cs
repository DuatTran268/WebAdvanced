using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Services.Blogs;

public class BlogRepository : IBlogRepository
{
    // Cai dat phuong thuc khoi tao cua lop 
    private readonly BlogDbContext _context;
    public BlogRepository(BlogDbContext context)
    {
        _context = context;
    }


    // tim bai viet co ten dinh danh la slug va duoc dang vao thang nam
    public async Task<Post> GetPostAsync(
        int year,
        int month,
        string slug,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Post> postsQueryy = _context.Set<Post>().Include(x => x.Category).Include(x => x.Author);
        if (year > 0)
        {
            postsQueryy = postsQueryy.Where(x => x.PostedDate.Year == year);
        }

        if (month > 0)
        {
            postsQueryy = postsQueryy.Where(x => x.PostedDate.Month == month);

        }
        if (!string.IsNullOrEmpty(slug))
        {
            postsQueryy = postsQueryy.Where(x => x.UrlSlug == slug);
        }

        return await postsQueryy.FirstOrDefaultAsync(cancellationToken);
    }

    // tim top n bai viet pho bien nhieu nguoi xem nhat
    public async Task<IList<Post>> GetPopularArticlesAsync(
        int numPosts, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<Post>()
            .Include(x => x.Author)
            .Include(x => x.Category)
            .OrderByDescending(p => p.ViewCount)
            .Take(numPosts)
            .ToListAsync(cancellationToken);
    }

    // kiem tra xem ten dinh danh cua bai viet da co hay chua
    public async Task<bool> IsPostSlugExistedAsync(
        int postId,
        string slug,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<Post>()
            .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
    }

    // tang so luot xem cua mot bai viet
    public async Task IncreaseViewCountAsync(
        int postId,
        CancellationToken cancellationToken= default)
    {
        await _context.Set<Post>()
            .Where(x => x.Id == postId)
            .ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1), cancellationToken);
    }



}
