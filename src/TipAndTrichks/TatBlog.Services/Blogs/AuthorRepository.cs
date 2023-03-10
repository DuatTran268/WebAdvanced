using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs;

public class AuthorRepository :IAuthorRepository
{
    // phuong thuc khoi tao
    private readonly BlogDbContext _context;

    public AuthorRepository(BlogDbContext context)
    {
        _context = context;
    }

   


    //b)
    public async Task<Author> FindAuthorById(int id, CancellationToken cancellationToken = default)
    {
        IQueryable<Author> findAuthorById = _context.Set<Author>()
            .Where(a => a.Id == id);
        return await findAuthorById.FirstOrDefaultAsync(cancellationToken);

    }


    // c)
    public async Task<Author> FindAuthorBySlug(string slug, CancellationToken cancellationToken = default)
    {
        IQueryable<Author> findAuthorSlug = _context.Set<Author>()
            .Where(c => c.UrlSlug == slug);
        return await findAuthorSlug.FirstOrDefaultAsync(cancellationToken);
    }


    // e
    public async Task AddOrUpdateInfoAuthor(Author author, CancellationToken cancellationToken = default)
    {
        Author authorUpdate = await _context.Set<Author>()
            .Where(c => c.UrlSlug == author.UrlSlug)
            .FirstOrDefaultAsync(cancellationToken);
        if (authorUpdate != null)
        {
            if (author.Id <= 0)
            {
                await Console.Out.WriteLineAsync("Da ton tai ID");
                return;
            }
            _context.Entry(authorUpdate).CurrentValues.SetValues(author);
        }
        else
        {
            _context.Set<Author>().Add(author);
        }
        await _context.SaveChangesAsync(cancellationToken);

    }

    public async Task<IPagedList<Author>> GetNAuthorTopPosts(int n, IPagingParams pagingParams, CancellationToken cancellationToken = default)
    {
        Author authorTop = _context.Set<Author>()
            .Include(a => a.Posts)
            .OrderByDescending(a => a.Posts.Count(p => p.Published)).First();

        int maxPost = authorTop.Posts.Count(p => p.Published);

        return await _context.Set<Author>()
            .Include(a => a.Posts)
            .Where(a => a.Posts.Count(p => p.Published) == maxPost)
            .Take(n)
            .ToPagedListAsync(pagingParams, cancellationToken);
            
            
    }
}
