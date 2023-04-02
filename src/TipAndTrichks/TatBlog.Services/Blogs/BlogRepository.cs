using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using SlugGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TatBlog.Services.Blogs;

public class BlogRepository : IBlogRepository
{
	// Cai dat phuong thuc khoi tao cua lop 
	private readonly BlogDbContext _context;
	private readonly IMemoryCache _memoryCache;

	public BlogRepository(BlogDbContext context, IMemoryCache memoryCache)
	{
		_context = context;
		_memoryCache = memoryCache;
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

	// kiem tra xem ten dinh danh cua category da co chua
	public async Task<bool> IsCategoriesSlugExistedAsync(int categoryId, string slug, CancellationToken cancellationToken = default)
	{
		return await _context.Categories
			.AnyAsync(c => c.Id != categoryId && c.UrlSlug == slug, cancellationToken);
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


	public async Task<IList<AuthorItem>> GetAuthorAsync(CancellationToken cancellationToken = default)
	{
		IQueryable<Author> authors = _context.Set<Author>();
		return await authors.OrderBy(x => x.FullName).Select(x => new AuthorItem()
		{
			Id = x.Id,
			FullName = x.FullName,
			UrlSlug = x.UrlSlug,
			ImageUrl = x.ImageUrl,
			Email = x.Email,
			JoinedDate = x.JoinedDate,
			PostCount = x.Posts.Count(p => p.Published)
		}).OrderByDescending(s => s.PostCount)
		.ToListAsync(cancellationToken);
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
				|| p.ShortDescription.Contains(query.Keyword)
				|| p.UrlSlug.Contains(query.Keyword)
				|| p.Tags.Any(t => t.Name.Contains(query.Keyword))
			  );
		}
		if (query.CategoryId > 0)
		{
			postsQuery = postsQuery.Where(p => p.CategoryId == query.CategoryId);
		}
		if (query.AuthorId > 0)
		{
			postsQuery = postsQuery.Where(p => p.AuthorId == query.AuthorId);
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

		if (query.PostedMonth > 0)
		{
			postsQuery = postsQuery
			  .Where(p => p.PostedDate.Month == query.PostedMonth);
		}
		if (query.PostedYear > 0)
		{
			postsQuery = postsQuery
				.Where(p => p.PostedDate.Year == query.PostedYear);
		}

		if (query.CategoryId > 0)
		{
			postsQuery = postsQuery
			  .Where(p => p.CategoryId == query.CategoryId);
		}

		if (query.AuthorId > 0)
		{
			postsQuery = postsQuery
			  .Where(p => p.AuthorId == query.AuthorId);
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

	public async Task<IPagedList<T>> GetPagePostsAsync<T>(PostQuery query,
		IPagingParams pagingParams,
		Func<IQueryable<Post>,
		IQueryable<T>> mapper,
		CancellationToken cancellationToken = default)
	{
		IQueryable<Post> postFindQuery = FilterPost(query);
		IQueryable<T> tQueryResult = mapper(postFindQuery);
		return await tQueryResult.ToPagedListAsync(pagingParams, cancellationToken);
	}

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
			if (month > 0)
			{
				postQuery = postQuery.Where(p => p.PostedDate.Month == month);
			}
			if (!string.IsNullOrEmpty(slug))
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

	public async Task<IList<PostMonth>> CountPostInMonth(int month, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Post>()
			.GroupBy(p => new { p.PostedDate.Year, p.PostedDate.Month })
			.Select(p => new PostMonth()
			{
				Year = p.Key.Year,
				Month = p.Key.Month,
				PostCount = p.Count()
			})
			.OrderByDescending(p => p.Year).ThenByDescending(p => p.Month)
			.Take(month)
			.ToListAsync(cancellationToken);
	}


	public async Task<bool> CreateOrUpdatePostAsync(Post post, IEnumerable<string> tags, CancellationToken cancellationToken = default)
	{
		if (post.Id > 0)
		{
			await _context.Entry(post).Collection(x => x.Tags).LoadAsync(cancellationToken);
		}
		else
		{
			post.Tags = new List<Tag>();
		}

		var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
			.Select(x => new
			{
				Name = x,
				Slug = x.GenerateSlug()
			})
			.GroupBy(x => x.Slug)
			.ToDictionary(g => g.Key, g => g.First().Name);


		foreach (var kv in validTags)
		{
			if (post.Tags.Any(x => string.Compare(x.UrlSlug, kv.Key, StringComparison.InvariantCultureIgnoreCase) == 0)) continue;

			var tag = await GetTagSlugAsync(kv.Key, cancellationToken) ?? new Tag()
			{
				Name = kv.Value,
				Description = kv.Value,
				UrlSlug = kv.Key
			};

			post.Tags.Add(tag);
		}

		post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.UrlSlug)).ToList();

		if (post.Id > 0)
			_context.Update(post);
		else
			_context.Add(post);

		return await _context.SaveChangesAsync(cancellationToken) > 0;

		
	}

	//public async Task<Post> GetPostByIdAsync(Guid postId, CancellationToken cancellationToken = default)
	//{
	//	return await _context.Set<Post>()
	//		.Include(s => s.Author)
	//		.Include(s => s.Tags)
	//		.Include(s => s.Category)
	//		.FirstOrDefaultAsync(s => s.Id.Equals(postId), cancellationToken);
	//}

	public async Task<Post> GetPostByIdAsync(int postId, bool includeDetails = false, CancellationToken cancellationToken = default)
	{
		if (!includeDetails)
		{
			return await _context.Set<Post>().FindAsync(postId);
		}

		return await _context.Set<Post>()
			.Include(x => x.Category)
			.Include(x => x.Author)
			.Include(x => x.Tags)
			.FirstOrDefaultAsync(x => x.Id == postId, cancellationToken);
	}

	// change status post
	public async Task ChangePublicStatusPostAsync(int postId, CancellationToken cancellationToken = default)
	{
		await _context.Set<Post>().Where(x => x.Id == postId)
			.ExecuteUpdateAsync(p => p.SetProperty(x => x.Published, x => !x.Published), cancellationToken);
	}


	public async Task<bool> DeletePostById(int id, CancellationToken cancellationToken = default)
	{
		var delPostId = await _context.Set<Post>()
			.Include(p => p.Tags).Where(p => p.Id == id).FirstOrDefaultAsync(cancellationToken);
		_context.Set<Post>().Remove(delPostId);
		await _context.SaveChangesAsync(cancellationToken);
		return true;
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



	private IQueryable<Category> FilterCategory(CategoryQuery query)
	{
		IQueryable<Category> cateQuery = _context.Set<Category>().Include(c => c.Posts);

		//Console.WriteLine(cateQuery);

		if (!string.IsNullOrEmpty(query.Keyword))
		{
			cateQuery = cateQuery
			  .Where(c => c.Name.Contains(query.Keyword));

		}
		return cateQuery;
	}


	public async Task<IPagedList<Category>> GetPageCategoryAsync(
		CategoryQuery condition,
		int pageNumber,
		int pageSize,
		CancellationToken cancellationToken = default)
	{
		return await FilterCategory(condition).ToPageListAsync(
			pageNumber, pageSize, "Id", "DESC", cancellationToken);
	}

	public async Task<Category> GetCategoryByIdAsync(int categoryId, bool includeDetails = false, CancellationToken cancellationToken = default)
	{
		if (!includeDetails)
		{
			return await _context.Set<Category>().FindAsync(categoryId);
		}
		return await _context.Set<Category>().FirstOrDefaultAsync(x => x.Id == categoryId, cancellationToken);

	}

	public async Task<Category> CreateOrUpdateCategoryAsync(Category category, CancellationToken cancellationToken = default)
	{
		if (_context.Set<Category>().Any(c => c.Id == category.Id))
		{
			_context.Entry(category).State = EntityState.Modified;
		}
		else
		{
			_context.Categories.Add(category);
		}

		await _context.SaveChangesAsync(cancellationToken);

		return category;
	}

	public async Task<bool> DeleteCategoryById(int id, CancellationToken cancellationToken = default)
	{
		var delCategoryId = await _context.Set<Category>().Where(c => c.Id == id).FirstOrDefaultAsync(cancellationToken);
		_context.Set<Category>().Remove(delCategoryId);
		await _context.SaveChangesAsync(cancellationToken);
		return true;
	}



	// Tag

	private IQueryable<Tag> FilterTag(TagQuery query)
	{
		IQueryable<Tag> tagQuery = _context.Set<Tag>().Include(t => t.Posts);

		if (!string.IsNullOrEmpty(query.Keyword))
		{
			tagQuery = tagQuery
			  .Where(t => t.Name.Contains(query.Keyword));

		}
		return tagQuery;
	}


	public async Task<IPagedList<Tag>> GetPageTagAsync(TagQuery condition, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
	{
		return await FilterTag(condition)
			.ToPageListAsync(pageNumber, pageSize, "Id", "DESC", cancellationToken);
	}

	public async Task<Tag> GetTagByIdAsync(int tagId, bool includeDetails = false, CancellationToken cancellationToken = default)
	{
		if (!includeDetails)
		{
			return await _context.Set<Tag>().FindAsync(tagId);
		}
		return await _context.Set<Tag>().FirstOrDefaultAsync(x => x.Id == tagId, cancellationToken);
	}

	public async Task<Tag> CreateOrUpdateTagAsync(Tag tag, CancellationToken cancellationToken = default)
	{
		if (_context.Set<Tag>().Any(t => t.Id == tag.Id))
		{
			_context.Entry(tag).State = EntityState.Modified;
		}
		else
		{
			_context.Tags.Add(tag);
		}
		await _context.SaveChangesAsync(cancellationToken);
		return tag;
	}

	public async Task<bool> DeleteTagById(int id, CancellationToken cancellationToken = default)
	{
		var deleteTag = await _context.Set<Tag>().Where(c => c.Id == id).FirstOrDefaultAsync(cancellationToken);
		_context.Set<Tag>().Remove(deleteTag);
		await _context.SaveChangesAsync(cancellationToken);
		return true;
	}

	public async Task<int> PostCountAsync(CancellationToken cancellationToken = default)
	{
		return await _context.Set<Post>().CountAsync(cancellationToken);
	}

	public async Task<int> PostCountNonPublicAsync(CancellationToken cancellationToken = default)
	{
		return await _context.Set<Post>().CountAsync(p => !p.Published, cancellationToken);
	}

	public async Task<int> CountCategoryAsync(CancellationToken cancellationToken = default)
	{
		return await _context.Set<Category>().CountAsync(cancellationToken);
	}

	public async Task<IPagedList<T>> GetPagedCategoryAsync<T>(Func<IQueryable<Category>, IQueryable<T>> mapper, IPagingParams pagingParams, string name = null, CancellationToken cancellationToken = default)
	{
		var categoryQuery = _context.Set<Category>().AsNoTracking();

		if (!string.IsNullOrEmpty(name))
		{
			categoryQuery = categoryQuery.Where(x => x.Name.Contains(name));
		}

		return await mapper(categoryQuery)
			.ToPagedListAsync(pagingParams, cancellationToken);
	}

	public async Task<IPagedList<CategoryItem>> GetPagedCategoryAsync(IPagingParams pagingParams, string name = null, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Category>()
			.AsNoTracking()
			.WhereIf(!string.IsNullOrWhiteSpace(name),
				x => x.Name.Contains(name))
			.Select(c => new CategoryItem()
			{
				Id = c.Id,
				UrlSlug = c.UrlSlug,
				Name = c.Name,
				Description = c.Description,
				ShowOnMenu = c.ShowOnMenu
			}).ToPagedListAsync(pagingParams, cancellationToken);
	}

	public async Task<Category> GetCachedCategoryByIdAsync(int categoryId)
	{
		return await _memoryCache.GetOrCreateAsync($"category.by-id.{categoryId}",
			async (entry) =>
			{
				entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
				return await GetCategoryByIdAsync(categoryId);
			});
	}

	public async Task<bool> AddOrUpdateCateAsync(Category category, CancellationToken cancellationToken = default)
	{
		if (category.Id > 0)
		{
			_context.Categories.Update(category);
			_memoryCache.Remove($"category.by-id.{category.Id}");
		}
		else
		{
			_context.Categories.Add(category);
		}
		return await _context.SaveChangesAsync(cancellationToken) > 0;
	}

	public async Task<IPagedList<PostItem>> GetPagedPostsAsync(IPagingParams pagingParams, string name = null, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Post>()
			.Include(t => t.Tags)
			.Include(t => t.Category)
			.Include(t => t.Author)
			.AsNoTracking()
			.WhereIf(!string.IsNullOrWhiteSpace(name), x => x.Title.Contains(name))
			.Select(p => new PostItem
			{
				Id = p.Id,
				Title = p.Title,
				UrlSlug = p.UrlSlug,
				ShortDescription = p.ShortDescription,
				Description = p.Description,
				PostDate = p.PostedDate,
				ViewCount = p.ViewCount,
				Published = p.Published,
				CategoryName = p.Category.Name,
				ImageUrl = p.ImageUrl,

			}).ToPagedListAsync(pagingParams, cancellationToken);
	}

	public async Task<IPagedList<T>> GetPagedPostsAsync<T>(Func<IQueryable<Post>, IQueryable<T>> mapper, IPagingParams pagingParams, string name = null, CancellationToken cancellationToken = default)
	{
		var postQuery = _context.Set<Post>().AsNoTracking();

		if (!string.IsNullOrEmpty(name))
		{
			postQuery = postQuery.Where(x => x.Title.Contains(name));
		}

		return await mapper(postQuery)
			.ToPagedListAsync(pagingParams, cancellationToken);
	}

	public async Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, string name = null, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Tag>()
			.AsNoTracking()
			.WhereIf(!string.IsNullOrWhiteSpace(name),
				x => x.Name.Contains(name))
			.Select(t => new TagItem()
			{
				Id = t.Id,
				Name = t.Name,
				UrlSlug = t.UrlSlug,
				Description = t.Description,
				PostCount = t.Posts.Count(p => p.Published)
			}).ToPagedListAsync(pagingParams, cancellationToken);
	}

	public async Task<Tag> GetCachedTagByIdAsync(int authorId)
	{
		return await _memoryCache.GetOrCreateAsync(
			$"tag.by-id.{authorId}",
			async (entry) =>
			{
				entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
				return await GetTagByIdAsync(authorId);
			});
	}

	public async Task<IPagedList<T>> GetPagedTagsTAsync<T>(Func<IQueryable<Tag>, IQueryable<T>> mapper, IPagingParams pagingParams, string name = null, CancellationToken cancellationToken = default)
	{
		var tagQuery = _context.Set<Tag>().AsNoTracking();

		if (!string.IsNullOrEmpty(name))
		{
			tagQuery = tagQuery.Where(x => x.Name.Contains(name));
		}

		return await mapper(tagQuery)
			.ToPagedListAsync(pagingParams, cancellationToken);
	}


	public async Task<Post> GetCachedPostByIdAsync(int postId, bool postDetails = false,
			CancellationToken cancellationToken = default)
	{
		return await _memoryCache.GetOrCreateAsync($"posts.by-id.{postId}",
			async (entry) =>
			{
				entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
				return await GetPostByIdAsync(postId, postDetails);
			});
	}

	public async Task<bool> SetImageUrlAsync(int postsId, string imageUrl, CancellationToken cancellationToken = default)
	{
		return await _context.Posts.Where(p => p.Id == postsId)
			.ExecuteUpdateAsync(x => x.SetProperty(p => p.ImageUrl, p => imageUrl)
			, cancellationToken) > 0;
	}

	public async Task<bool> IsTagSlugExistedAsync(int tagId, string slug, CancellationToken cancellationToken = default)
	{
		return await _context.Tags.AnyAsync(
			x => x.Id != tagId && x.UrlSlug == slug, cancellationToken);
	}

	public async Task<bool> AddOrUpdateTagAsync(Tag tag, CancellationToken cancellationToken = default)
	{
		if (tag.Id > 0)
		{
			_context.Tags.Update(tag);
			_memoryCache.Remove($"tag.by-id.{tag.Id}");
		}
		else
		{
			_context.Tags.Add(tag);
		}
		return await _context.SaveChangesAsync(cancellationToken) > 0;
	}

	// get n post top reader
	public async Task<IList<T>> GetNPostTopReaderAsync<T>(
		int n, 
		Func<IQueryable<Post>, 
			IQueryable<T>> mapper, 
		CancellationToken cancellationToken = default)
	{
		var topPost = _context.Set<Post>()
			.Include(p => p.Author)
			.Include(p => p.Tags)
			.Include(p => p.Category)
			.OrderByDescending(p => p.ViewCount)
			.Take(n);

		return await mapper(topPost).ToListAsync(cancellationToken);
	}

	public async Task<IList<T>> GetNRandomPostAsync<T>(int n, Func<IQueryable<Post>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
	{
		IQueryable<Post> rdPostQuery = _context.Set<Post>()
			.Include(p => p.Author)
			.Include(p => p.Tags)
			.Include(p => p.Category)
			.OrderBy(c => Guid.NewGuid())
			.Take(n);

		return await mapper(rdPostQuery).ToListAsync(cancellationToken);
	}
}