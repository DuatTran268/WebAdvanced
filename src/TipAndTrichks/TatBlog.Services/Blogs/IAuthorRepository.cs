using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;



namespace TatBlog.Services.Blogs;

public interface IAuthorRepository
{
    // b, tim mot tac gia theo ma so
    Task<Author> FindAuthorById (int id, CancellationToken cancellationToken = default);

    // c, tìm mộ tác giả theo định danh slu
    Task<Author> FindAuthorBySlug(string slug, CancellationToken cancellationToken = default);


    //e) them hoac cap nhat thong tin mot tac gia
    Task AddOrUpdateInfoAuthor(Author author, CancellationToken cancellationToken = default);


    // f
    Task<IPagedList<Author>> GetNAuthorTopPosts(int n, IPagingParams pagingParams, CancellationToken cancellationToken = default);


	Task<IPagedList<Author>> GetPageAuthorAsync(
		AuthorQuery condition, int pageNumber, int pageSize, CancellationToken cancellationToken = default);


	// get author by id async
    public Task<Author> GetAuthorByIdAsync(
        int authorId, bool includeDetails = false, CancellationToken cancellationToken = default);

	Task<Author> CreateOrUpdateAuthorAsync(Author author, CancellationToken cancellationToken = default);

	// remove
	Task<bool> DeleteAuthorById(int id, CancellationToken cancellationToken = default);

    // count author
    Task<int> CountAuthorAsync(CancellationToken cancellationToken = default);


	#region Code Teacher

	Task<Author> GetAuthorBySlugAsync(
		string slug,
		CancellationToken cancellationToken = default);

	Task<Author> GetCachedAuthorBySlugAsync(
		string slug, CancellationToken cancellationToken = default);

	Task<Author> GetAuthorByIdAsync(int authorId);

	Task<Author> GetCachedAuthorByIdAsync(int authorId);

	Task<IList<AuthorItem>> GetAuthorsAsync(
		CancellationToken cancellationToken = default);

	Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
		IPagingParams pagingParams,
		string name = null,
		CancellationToken cancellationToken = default);

	Task<IPagedList<T>> GetPagedAuthorsAsync<T>(
		Func<IQueryable<Author>, IQueryable<T>> mapper,
		IPagingParams pagingParams,
		string name = null,
		CancellationToken cancellationToken = default);

	Task<bool> AddOrUpdateAsync(
		Author author,
		CancellationToken cancellationToken = default);

	Task<bool> DeleteAuthorAsync(
		int authorId,
		CancellationToken cancellationToken = default);

	Task<bool> IsAuthorSlugExistedAsync(
		int authorId, string slug,
		CancellationToken cancellationToken = default);

	Task<bool> SetImageUrlAsync(
		int authorId, string imageUrl,
		CancellationToken cancellationToken = default);

	#endregion
}
