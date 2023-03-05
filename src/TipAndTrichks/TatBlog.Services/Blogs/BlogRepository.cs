using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

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



    // lay danh sach chuyen muc va so luong bai viet nam thuco tung chuyen muc
    

    public async Task<IList<CategoryItem>> GetCategoryAsync(bool showOnMenu = false, 
        CancellationToken cancellationToken = default)
    {
        IQueryable<Category> categories = _context.Set<Category>();
        if (showOnMenu)
        {
            categories = categories.Where(x => x.ShowOnMenu);
        }

        return await categories.OrderBy(x => x.Name).Select(x => new CategoryItem()
        {
            Id = x.Id,
            Name = x.Name,
            UrlSlug = x.UrlSlug,
            Description = x.Description,
            ShowOnMenu = x.ShowOnMenu,
            PostCount = x.Posts.Count(p => p.Published)
        }).ToListAsync(cancellationToken);
    }

    public async Task<IPagedList<TagItem>> GetPagedTageAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
    {
        var tagQuery = _context.Set<Tag>().Select(x => new TagItem()
        {
            Id = x.Id,
            Name = x.Name,
            UrlSlug = x.UrlSlug,
            Description = x.Description,
            PostCount = x.Posts.Count(p => p.Published)

        });

        return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
    }

    // a)
    public async Task<Tag> GetTagSlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        IQueryable<Tag> tagQuery = _context.Set<Tag>()
            .Where(t => t.UrlSlug == slug);
        return await tagQuery.FirstOrDefaultAsync(cancellationToken);

    }

    // c)
    public async Task<IList<TagItem>> GetAllTagsAttachPost(CancellationToken cancellationToken = default)
    {
        IQueryable<Tag> tagsQueryPost = _context.Set<Tag>();
        return await tagsQueryPost.OrderBy(t => t.Name).Select(t => new TagItem()
        {
            Id = t.Id,
            Name = t.Name,
            UrlSlug = t.UrlSlug,
            Description = t.Description,
            PostCount = t.Posts.Count(p => p.Published)
        }).ToListAsync(cancellationToken);
    }

    // d)
    public async Task<bool> RemoveTagById(int id, CancellationToken cancellationToken = default)
    {
        var delByID = await _context.Set<Tag>()
            .Include(t => t.Posts)
            .Where(t => t.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        _context.Set<Tag>().Remove(delByID);
        await _context.SaveChangesAsync(cancellationToken); // lưu lại thay đổi

        return true;
    }
    // e)
    public async Task<Category> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        IQueryable<Category> categoryQuery = _context.Set<Category>()
            .Where(c => c.UrlSlug== slug);
        return await categoryQuery.FirstOrDefaultAsync(cancellationToken);
    }

    // f)
    public async Task<Category> FindCategoryById(int id, CancellationToken cancellationToken = default)
    {
        IQueryable<Category> cateFindId = _context.Set<Category>()
            .Where(c => c.Id==id);
        return await cateFindId.FirstOrDefaultAsync(cancellationToken);
    }

    // g)
    public async Task AddOrUpdateCategory(Category category, CancellationToken cancellationToken = default)
    {
        Category categoriUpdate = await _context.Set<Category>()
            .Where(c => c.UrlSlug == category.UrlSlug)
            .FirstOrDefaultAsync(cancellationToken);
        if (categoriUpdate != null)
        {
            if (category.Id <= 0)
            {
                await Console.Out.WriteLineAsync("Da ton tai ID nay duoc them roi");
                return;
            }
            _context.Entry(categoriUpdate).CurrentValues.SetValues(category);
        }
        else
        {
            _context.Set<Category>().Add(category);
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
 
    // h) 
    public async Task<bool> DeleteCategoryByID(int id, CancellationToken cancellationToken = default)
    {
        var cateDelete = await _context.Set<Category>()
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (cateDelete == null) return false;

        _context.Set<Category>().Remove(cateDelete);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }


    // i)
    public async Task<bool> CheckIDSlugOfCategoryExist(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Category>().AnyAsync(c => c.UrlSlug == slug, cancellationToken);
    }


    // l)
    public async Task<Post> FindPostById(int id, CancellationToken cancellationToken = default)
    {
        IQueryable<Post> postFindId = _context.Set<Post>()
            .Where(p => p.Id == id);
        return await postFindId.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddOrUpdatePost(Post post, CancellationToken cancellationToken = default)
    {
        Post postUpdate = await _context.Set<Post>()
            .Where(p => p.UrlSlug == post.UrlSlug)
            .FirstOrDefaultAsync(cancellationToken);

        if (postUpdate != null)
        {
            if (post.Id <= 0)
            {
                await Console.Out.WriteLineAsync("Da ton tai ID nay duoc them roi");
                return;
            }
        }
        else
        {
            _context.Set<Post>().Add(post);
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}
