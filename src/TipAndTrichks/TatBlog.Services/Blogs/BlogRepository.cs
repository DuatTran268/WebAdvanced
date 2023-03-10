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
		CancellationToken cancellationToken = default)
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
			.Where(c => c.UrlSlug == slug);
		return await categoryQuery.FirstOrDefaultAsync(cancellationToken);
	}

	


	// f)
	public async Task<Category> FindCategoryById(int id, CancellationToken cancellationToken = default)
	{
		IQueryable<Category> cateFindId = _context.Set<Category>()
			.Where(c => c.Id == id);
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

	// j
	public async Task<IPagedList<CategoryItem>> GetPageShareCategory(IPagingParams pagingParams, CancellationToken cancellationToken = default)
	{
		IQueryable<CategoryItem> categoryItems = _context.Set<Category>()
			.Select(c => new CategoryItem()
			{
				Id = c.Id,
				Description = c.Description,
				Name = c.Name,
				ShowOnMenu = c.ShowOnMenu,
				UrlSlug = c.UrlSlug,
				PostCount = c.Posts.Count(p => p.Published)
			});
		return await categoryItems.ToPagedListAsync(pagingParams, cancellationToken);
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

	// n
	public async Task ConvertStatusPostToPublished(int id, bool published, CancellationToken cancellationToken = default)
	{
		await _context.Set<Post>().Where(p => p.Id == id)
			.ExecuteUpdateAsync(p => p.SetProperty(p => p.Published, published), cancellationToken);
	}

	// o
	public async Task<IList<Post>> GetRandomNPost(int n, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Post>().OrderBy(p => Guid.NewGuid()).Take(n).ToListAsync(cancellationToken);
	}




	private IQueryable<Post> FilterPost(PostQuery query)
	{
		IQueryable<Post> postsQuery = _context.Set<Post>()
		  .Include(p => p.Author)
		  .Include(p => p.Category)
		  .Include(p => p.Tags);

		if (!string.IsNullOrEmpty(query.Keyword))
		{
			postsQuery = postsQuery
			  .Where(p => p.Title.Contains(query.Keyword)
				|| p.Description.Contains(query.Keyword)
				|| p.ShortDescrption.Contains(query.Keyword)
				|| p.UrlSlug.Contains(query.Keyword)
				|| p.Tags.Any(t => t.Name.Contains(query.Keyword))
			  );
		}
		if (!string.IsNullOrWhiteSpace(query.CategorySlug))
		{
			postsQuery = postsQuery
					.Where(p => p.Category.UrlSlug == query.CategorySlug);
		}

		if (!string.IsNullOrWhiteSpace(query.AuthorSlug))
		{
			postsQuery = postsQuery.Where(a => a.Author.UrlSlug == query.AuthorSlug);
		}

		if (!string.IsNullOrWhiteSpace(query.TagSlug))
		{
			postsQuery = postsQuery
			  .Where(p => p.Tags.Any(t => t.UrlSlug == query.TagSlug));
		}

		if (query.PostedMonths > 0)
		{
			postsQuery = postsQuery
			  .Where(p => p.PostedDate.Month == query.PostedMonths);
		}

		if (query.CategoriesId > 0)
		{
			postsQuery = postsQuery
			  .Where(p => p.CategoryId == query.CategoriesId);
		}

		if (query.AuthorsId > 0)
		{
			postsQuery = postsQuery
			  .Where(p => p.AuthorId == query.AuthorsId);
		}

		if (!string.IsNullOrEmpty(query.CategoryName))
		{
			postsQuery = postsQuery
				.Where(p => p.Category.Name == query.CategoryName);
		}

		if (query.PublishedOnly)
		{
			postsQuery = postsQuery.Where(p => p.Published);
		}

		if (query.NotPublished)
		{
			postsQuery = postsQuery.Where(p => !p.Published);
		}


		var selectedTags = query.GetSelectedTags();
		if (selectedTags.Count > 0)
		{
			foreach (var tag in selectedTags)
			{
				postsQuery = postsQuery.Include(p => p.Tags)
				  .Where(p => p.Tags.Any(t => t.Name == tag));
			}
		}

		return postsQuery;
	}


	////1s: cách 1
	//public async Task<IPagedList<Post>> GetPagePostsAsync(PostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default)
	//{
	//	IQueryable<Post> postQuery = FilterPost(query);
	//	return await postQuery.ToPagedListAsync(pagingParams, cancellationToken);
	//}

	//public async Task<IPagedList<T>> GetPagePostsAsync<T>(PostQuery query, IPagingParams pagingParams, Func<IQueryable<Post>, IQueryable<T>> mapper, CancellationToken cancellationToken)
	//{
	//	IQueryable<Post> postFindQuery = FilterPost(query);
	//	IQueryable<T> tQueryResult = mapper(postFindQuery);
	//	return await tQueryResult.ToPagedListAsync(pagingParams, cancellationToken);
	//}

	public async Task<Author> GetAuthorBySlugAsync(string slug, CancellationToken cancellationToken = default)
	{
		IQueryable<Author> authorQuery = _context.Set<Author>()
			.Where(a => a.UrlSlug == slug);
		return await authorQuery.FirstOrDefaultAsync(cancellationToken);
	}


	//// 1s: cách của thầy
	public async Task<IPagedList<Post>> GetPagePostAsync(
		PostQuery condition,
		int pageNumber = 1,
		int pageSize = 2,
		CancellationToken cancellationToken = default)
	{
		return await FilterPost(condition)
			.ToPageListAsync(
			pageNumber,
			pageSize, nameof(Post.PostedDate), "DESC",
			cancellationToken);
	}

	public async Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default)
	{
		IQueryable<Tag> tagQuery = _context.Set<Tag>()
			.Where(t => t.UrlSlug == slug);
		return await tagQuery.FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<Post> GetPostSlugAsync(int year, int month, string slug, CancellationToken cancellationToken = default)
	{
		IQueryable<Post> postQuery = _context.Set<Post>()
			.Include(p => p.Category)
			.Include(p => p.Author)
			.Include(p => p.Tags);
		{
			if (year > 0)
			{
				postQuery = postQuery.Where(p => p.PostedDate.Year == year);
			}
			if(month > 0)
			{
				postQuery = postQuery.Where(p => p.PostedDate.Month == month);
			}
			if(!string.IsNullOrEmpty(slug))
			{
				postQuery = postQuery.Where(p => p.UrlSlug == slug);
			}

			return await postQuery.FirstOrDefaultAsync(cancellationToken);
		}
			
	}

	public async Task<IList<TagItem>> GetTagsAllAsync(CancellationToken cancellationToken = default)
	{
		IQueryable<Tag> tagsQuery = _context.Set<Tag>();
		return await tagsQuery.OrderBy(t => t.Name).Select(t => new TagItem()
		{
			Id = t.Id,
			Name = t.Name,
			UrlSlug = t.UrlSlug,
			Description = t.Description,
			PostCount = t.Posts.Count(p => p.Published)
		}).ToListAsync(cancellationToken);
	}

	public async Task<IList<PostMonth>> PostCountInMonth(int month, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Post>()
			.Select(p => new PostMonth()
			{
				Year = p.PostedDate.Year,
				Month = p.PostedDate.Month,
				PostCount = _context.Set<Post>()
				.Where(x => x.PostedDate == p.PostedDate)
				.Count()
			})
			.Distinct()
			.OrderByDescending(p => p.Year).ThenByDescending(p => p.Month)
			.Take(month)
			.ToListAsync(cancellationToken);
	}
}
