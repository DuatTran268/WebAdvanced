using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs;

public class AuthorRepository :IAuthorRepository
{
    // phuong thuc khoi tao
    private readonly BlogDbContext _context;
	private readonly IMemoryCache _memoryCache;
    public AuthorRepository(BlogDbContext context, IMemoryCache memoryCache)
    {
        _context = context;
		_memoryCache = memoryCache;
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

    private IQueryable<Author> FilterAuthor(AuthorQuery query)
    {
        IQueryable<Author> authorQuery = _context.Set<Author>().Include(a => a.Posts);

        if (!string.IsNullOrEmpty(query.Keyword))
        {
            authorQuery = authorQuery.Where(a => a.FullName.Contains(query.Keyword));
        }

        if (query.JoinMonth > 0)
        {
            authorQuery = authorQuery.Where(a => a.JoinedDate.Month == query.JoinMonth);
        }

        if (query.JoinYear > 0)
        {
            authorQuery = authorQuery.Where(a => a.JoinedDate.Year == query.JoinYear);
        }
        return authorQuery;
    }

	public async Task<IPagedList<Author>> GetPageAuthorAsync(AuthorQuery condition, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
	{
		return await FilterAuthor(condition).ToPageListAsync(pageNumber, pageSize, "Id", "DESC", cancellationToken);
	}

	public async Task<Author> GetAuthorByIdAsync(int authorId, bool includeDetails = false, CancellationToken cancellationToken = default)
	{
        if (!includeDetails)
        {
            return await _context.Set<Author>().FindAsync(authorId);
        }
        return await _context.Set<Author>().FirstOrDefaultAsync(x => x.Id == authorId, cancellationToken);
	}

	public async Task<Author> CreateOrUpdateAuthorAsync(Author author, CancellationToken cancellationToken = default)
	{
        if (_context.Set<Author>().Any(a => a.Id == author.Id))
        {
            _context.Entry(author).State = EntityState.Modified;
        }
        else
        {
            _context.Authors.Add(author);
        }
        await _context.SaveChangesAsync(cancellationToken);

        return author;
	}

	public async Task<bool> DeleteAuthorById(int id, CancellationToken cancellationToken = default)
	{
        var removeAuthorById = await _context.Set<Author>()
            .Where(a => a.Id == id).FirstOrDefaultAsync(cancellationToken);
        _context.Set<Author>().Remove(removeAuthorById);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
	}

	public async Task<int> CountAuthorAsync(CancellationToken cancellationToken = default)
	{
        return await _context.Set<Author>().CountAsync(cancellationToken);
	}

	public async Task<Author> GetAuthorBySlugAsync(string slug, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Author>()
			.FirstOrDefaultAsync(a => a.UrlSlug == slug, cancellationToken);
	}

	public async Task<Author> GetCachedAuthorBySlugAsync(string slug, CancellationToken cancellationToken = default)
	{
		return await _memoryCache.GetOrCreateAsync(
			$"author.by-slug.{slug}",
			async (entry) =>
			{
				entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
				return await GetAuthorBySlugAsync(slug, cancellationToken);
			});
	}

	public async Task<Author> GetAuthorByIdAsync(int authorId)
	{
		return await _context.Set<Author>().FindAsync(authorId);
	}

	public async Task<Author> GetCachedAuthorByIdAsync(int authorId)
	{
		return await _memoryCache.GetOrCreateAsync(
			$"author.by-id.{authorId}",
			async (entry) =>
			{
				entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
				return await GetAuthorByIdAsync(authorId);
			});
	}

	public async Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default)
	{
		return await _context.Set<Author>()
			.OrderBy(a => a.FullName)
			.Select(a => new AuthorItem()
			{
				Id = a.Id,
				FullName = a.FullName,
				Email = a.Email,
				JoinedDate = a.JoinedDate,
				ImageUrl = a.ImageUrl,
				UrlSlug = a.UrlSlug,
				PostCount = a.Posts.Count(p => p.Published)
			})
			.ToListAsync(cancellationToken);
	}

	public async Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
		IPagingParams pagingParams, 
		string name = null, 
		CancellationToken cancellationToken = default)
	{
	
		return await _context.Set<Author>()
			.AsNoTracking()
			.WhereIf(!string.IsNullOrWhiteSpace(name),
				x => x.FullName.Contains(name))
			.Select(a => new AuthorItem()
			{
				Id = a.Id,
				FullName = a.FullName,
				Email = a.Email,
				JoinedDate = a.JoinedDate,
				ImageUrl = a.ImageUrl,
				UrlSlug = a.UrlSlug,
				Notes = a.Notes,
				PostCount = a.Posts.Count(p => p.Published)
			})
			.ToPagedListAsync(pagingParams, cancellationToken)
			
			;
	}

	public async Task<IPagedList<T>> GetPagedAuthorsAsync<T>(
		Func<IQueryable<Author>, 
			IQueryable<T>> mapper, 
		IPagingParams pagingParams, 
		string name = null, CancellationToken cancellationToken = default)
	{
		var authorQuery = _context.Set<Author>().AsNoTracking();

		if (!string.IsNullOrEmpty(name))
		{
			authorQuery = authorQuery.Where(x => x.FullName.Contains(name));
		}

		return await mapper(authorQuery)
			.ToPagedListAsync(pagingParams, cancellationToken);
	}

	public async Task<bool> AddOrUpdateAsync(Author author, CancellationToken cancellationToken = default)
	{
		if (author.Id > 0)
		{
			_context.Authors.Update(author);
			_memoryCache.Remove($"author.by-id.{author.Id}");
		}
		else
		{
			_context.Authors.Add(author);
		}

		return await _context.SaveChangesAsync(cancellationToken) > 0;

	}

	public async Task<bool> DeleteAuthorAsync(int authorId, CancellationToken cancellationToken = default)
	{
		return await _context.Authors
			.Where(x => x.Id == authorId)
			.ExecuteDeleteAsync(cancellationToken) > 0;
	}

	public async Task<bool> IsAuthorSlugExistedAsync(int authorId, string slug, CancellationToken cancellationToken = default)
	{
		return await _context.Authors
			.AnyAsync(x => x.Id != authorId && x.UrlSlug == slug, cancellationToken);

	}

	public async Task<bool> SetImageUrlAsync(int authorId, string imageUrl, CancellationToken cancellationToken = default)
	{
		return await _context.Authors
			.Where(x => x.Id == authorId)
			.ExecuteUpdateAsync(x =>
				x.SetProperty(a => a.ImageUrl, a => imageUrl),
				cancellationToken) > 0;

	}

	public async Task<List<AuthorItem>> GetTopAuthorMostPosts(int numAuthor, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Author>().Select(a => new AuthorItem()
		{
			Id = a.Id,
			FullName = a.FullName,
			Email = a.Email,
			UrlSlug = a.UrlSlug,
			ImageUrl = a.ImageUrl,
			JoinedDate = a.JoinedDate,
			Notes = a .Notes,
			PostCount = a.Posts.Count(p => p.Published)

		}).Take(numAuthor).ToListAsync(cancellationToken);
	}
}
