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
}
